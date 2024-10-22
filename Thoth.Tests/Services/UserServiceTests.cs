using Moq;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class UserServiceTests {
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IRoleRepository> _roleRepositoryMock;
		private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
		private readonly Mock<ILoggerService> _loggerMock;
		private readonly UserService _userService;
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";
		private const string Password = "Password@123";

		public UserServiceTests() {
			_userRepositoryMock = new Mock<IUserRepository>();
			_roleRepositoryMock = new Mock<IRoleRepository>();
			_transactionRepositoryMock = new Mock<ITransactionRepository>();
			_loggerMock = new Mock<ILoggerService>();
			_userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object, _transactionRepositoryMock.Object, _loggerMock.Object);
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

			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

			var result = await _userService.CreateUserAsync(request);

			Assert.True(result);
			_userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
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

			var existingUser = new User("Existing User", Email, 1, Password);

			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

			var result = await _userService.CreateUserAsync(request);

			Assert.False(result);
			_userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
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

			var existingUser = new User(UserName, Email, 1, Password);

			_userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);
			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

			var result = await _userService.UpdateUserAsync(request);

			Assert.True(result);
			_userRepositoryMock.Verify(r => r.UpdateAsync(existingUser), Times.Once);
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

			var existingUser = new User(UserName, Email, 1, Password);
			var otherUserWithSameEmail = new User("Other User", "existingemail@example.com", 2, Password);

			_userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);
			_userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(otherUserWithSameEmail);

			var result = await _userService.UpdateUserAsync(request);

			Assert.False(result);
			_userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
		}

		[Fact]
		public async Task Should_Delete_User_Successfully_With_Transaction() {
			var existingUser = new User(UserName, Email, 1, Password);

			_userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);

			var result = await _userService.DeleteUserAsync(1);

			Assert.True(result);
			_transactionRepositoryMock.Verify(t => t.BeginTransactionAsync(), Times.Once);
			_transactionRepositoryMock.Verify(t => t.CommitAsync(), Times.Once);
			_userRepositoryMock.Verify(r => r.DeleteAsync(existingUser), Times.Once);
		}

		[Fact]
		public async Task Should_Rollback_Transaction_On_Delete_Error() {
			var existingUser = new User(UserName, Email, 1, Password);

			_userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingUser);
			_userRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<User>())).ThrowsAsync(new System.Exception());

			var result = await _userService.DeleteUserAsync(1);

			Assert.False(result);
			_transactionRepositoryMock.Verify(t => t.BeginTransactionAsync(), Times.Once);
			_transactionRepositoryMock.Verify(t => t.RollbackAsync(), Times.Once);
		}
	}
}
