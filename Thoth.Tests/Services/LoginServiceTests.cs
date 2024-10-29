using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class LoginServiceTests {
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly LoginService _loginService;

		public LoginServiceTests() {
			_userRepositoryMock = new Mock<IUserRepository>();
			_configurationMock = new Mock<IConfiguration>();

			_configurationMock.Setup(c => c["Jwt:Key"]).Returns("YourSuperSecure32CharacterSigningKeyOrMore!");
			_configurationMock.Setup(c => c["Jwt:ExpireMinutes"]).Returns("30");

			_loginService = new LoginService(_userRepositoryMock.Object, _configurationMock.Object);
		}

		[Fact]
		public async Task Should_Return_False_When_Request_Is_Invalid() {
			var request = new LoginRequest { Email = "", Password = "" };
			var result = await _loginService.LoginAsync(request);

			Assert.False(result.Success);
			Assert.Null(result.Token);
			Assert.Contains(request.Notifications, n => n.Key == "Email");
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}

		[Fact]
		public async Task Should_Return_False_When_User_Not_Found() {
			var request = new LoginRequest { Email = "user@example.com", Password = "Password123!" };

			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

			var result = await _loginService.LoginAsync(request);

			Assert.False(result.Success);
			Assert.Null(result.Token);
			Assert.Contains(request.Notifications, n => n.Message == "User not found");
		}

		[Fact]
		public async Task Should_Return_False_When_Password_Is_Incorrect() {
			var request = new LoginRequest { Email = "user@example.com", Password = "WrongPassword" };
			var user = new User("User", "user@example.com", 1);

			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

			var passwordHasher = new PasswordHasher<User>();
			var correctPasswordHash = passwordHasher.HashPassword(user, "CorrectPassword");
			user.PasswordHash = correctPasswordHash;

			var result = await _loginService.LoginAsync(request);

			Assert.False(result.Success);
			Assert.Null(result.Token);
			Assert.Contains(request.Notifications, n => n.Message == "Invalid credentials");
		}

		[Fact]
		public async Task Should_Return_True_And_Token_When_Credentials_Are_Correct() {
			var request = new LoginRequest { Email = "user@example.com", Password = "CorrectPassword" };
			var user = new User("User", "user@example.com", 1);

			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
			_userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin", "User" });
			_userRepositoryMock.Setup(r => r.GetPermissionsAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Read", "Write" });

			var passwordHasher = new PasswordHasher<User>();
			user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

			var result = await _loginService.LoginAsync(request);

			Assert.True(result.Success);
			Assert.NotNull(result.Token);

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(result.Token);
			Assert.Equal(user.Id.ToString(), token.Subject);
			Assert.Contains(token.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
			Assert.Contains(token.Claims, c => c.Type == "roles" && c.Value == "Admin,User");
			Assert.Contains(token.Claims, c => c.Type == "permissions" && c.Value == "Read,Write");
		}
	}
}
