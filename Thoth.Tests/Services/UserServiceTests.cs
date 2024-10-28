using Microsoft.AspNetCore.Identity;
using Moq;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class UserServiceTests {
		private readonly Mock<IUserRepository> _userRepository;
		private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
		private readonly Mock<ILoggerService> _loggerMock;
		private readonly UserService _userService;
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";
		private const string Password = "Password@123";

		public UserServiceTests() {
			_userRepository = new Mock<IUserRepository>();
			_transactionRepositoryMock = new Mock<ITransactionRepository>();
			_loggerMock = new Mock<ILoggerService>();
			_userService = new UserService(_transactionRepositoryMock.Object, _loggerMock.Object, _userRepository.Object);
		}

		[Fact]
		public async Task Should_Create_User_Successfully() {
			var request = new CreateUserRequest {
				Name = UserName,
				Email = Email,
				Password = Password,
				OrganizationId = 1,
				RoleIds = new List<int> { 1, 2 }
			};

			_userRepository.Setup(u => u.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

			var result = await _userService.CreateUserAsync(request);

			Assert.True(result);
			_userRepository.Verify(u => u.AddAsync(It.IsAny<User>(), Password), Times.Once);
			_userRepository.Verify(u => u.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<int>>()), Times.Once);
			_transactionRepositoryMock.Verify(t => t.CommitAsync(), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Creating_User_With_Existing_Email() {
			var request = new CreateUserRequest {
				Name = UserName,
				Email = Email,
				Password = Password,
				OrganizationId = 1,
				RoleIds = new List<int> { 1, 2 }
			};

			var existingUser = new User(UserName, Email, 1);

			_userRepository.Setup(u => u.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

			var result = await _userService.CreateUserAsync(request);

			Assert.False(result);
			_userRepository.Verify(u => u.AddAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
		}

		[Fact]
		public async Task Should_Update_User_Successfully() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = "Bruce Updated",
				Email = "bruceupdated@example.com",
				Password = Password,
				OrganizationId = 1,
				RoleIds = new List<int> { 1, 3 }
			};

			var existingUser = new User(UserName, Email, 1);

			_userRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);
			_userRepository.Setup(u => u.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

			var result = await _userService.UpdateUserAsync(request);

			Assert.True(result);
			_userRepository.Verify(u => u.UpdateAsync(existingUser), Times.Once);
			_userRepository.Verify(u => u.AddToRolesAsync(existingUser, It.IsAny<List<int>>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Updating_User_With_Existing_Email() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = "Bruce Updated",
				Email = "existingemail@example.com",
				Password = Password,
				OrganizationId = 1,
				RoleIds = new List<int> { 1, 3 }
			};

			var existingUser = new User(UserName, Email, 1);
			var otherUserWithSameEmail = new User("Other User", "existingemail@example.com", 2);

			_userRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);
			_userRepository.Setup(u => u.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(otherUserWithSameEmail);

			var result = await _userService.UpdateUserAsync(request);

			Assert.False(result);
			_userRepository.Verify(u => u.UpdateAsync(It.IsAny<User>()), Times.Never);
		}

		[Fact]
		public async Task Should_Delete_User_Successfully() {
			var existingUser = new User(UserName, Email, 1);

			_userRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);

			var result = await _userService.DeleteUserAsync(1);

			Assert.True(result);
			_userRepository.Verify(u => u.DeleteAsync(existingUser), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_User_Not_Found_On_Delete() {
			_userRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

			var result = await _userService.DeleteUserAsync(1);

			Assert.False(result);
			_userRepository.Verify(u => u.DeleteAsync(It.IsAny<User>()), Times.Never);
		}
	}
}
