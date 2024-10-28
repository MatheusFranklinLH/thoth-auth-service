using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Thoth.Domain.Entities;
using Thoth.Domain.Requests;

namespace Thoth.Domain.Services {
	public class LoginService {
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public LoginService(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration) {
			_signInManager = signInManager;
			_userManager = userManager;
			_configuration = configuration;
		}

		public async Task<(bool Success, string Token)> LoginAsync(LoginRequest request) {
			request.Validate();
			if (!request.IsValid) {
				return (false, null);
			}

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null) {
				request.AddNotification("Login", "User not found");
				return (false, null);
			}

			var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
			var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
			if (result == PasswordVerificationResult.Failed) {
				request.AddNotification("Login", "Invalid credentials");
				return (false, null);
			}

			var token = await GenerateJwtToken(user);

			return (true, token);
		}

		private async Task<string> GenerateJwtToken(User user) {
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
			var roles = await _userManager.GetRolesAsync(user);
			var permissions = await _userManager.GetClaimsAsync(user);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("roles", string.Join(",", roles)),
				new Claim("permissions", string.Join(",", permissions.Select(p => p.Value)))
			};

			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
