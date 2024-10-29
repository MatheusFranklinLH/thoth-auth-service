using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;

namespace Thoth.Domain.Services {
	public class LoginService {
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;

		public LoginService(IUserRepository userRepository, IConfiguration configuration) {
			_userRepository = userRepository;
			_configuration = configuration;
		}

		public async Task<(bool Success, string Token)> LoginAsync(LoginRequest request) {
			request.Validate();
			if (!request.IsValid) {
				return (false, null);
			}

			var user = await _userRepository.GetByEmailAsync(request.Email);
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
			var roles = await _userRepository.GetRolesAsync(user);
			var permissions = await _userRepository.GetPermissionsAsync(user);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("roles", string.Join(",", roles)),
				new Claim("permissions", string.Join(",", permissions))
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
