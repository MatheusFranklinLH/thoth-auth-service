using Moq;
using Thoth.API.Requests;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class PermissionServiceTests {
		private readonly Mock<IPermissionRepository> _repositoryMock;
		private readonly Mock<ILoggerService> _loggerMock;
		private readonly PermissionService _permissionService;

		public PermissionServiceTests() {
			_repositoryMock = new Mock<IPermissionRepository>();
			_loggerMock = new Mock<ILoggerService>();
			_permissionService = new PermissionService(_repositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Should_Create_Permission_When_Request_Is_Valid_And_Not_Exists() {
			var request = new CreatePermissionRequest { Name = "New Permission" };
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Permission)null);

			var result = await _permissionService.CreatePermissionAsync(request);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Permission>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Permission_With_Same_Name_Already_Exists() {
			var request = new CreatePermissionRequest { Name = "Existing Permission" };
			var existingPermission = new Permission("Existing Permission");
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingPermission);

			var result = await _permissionService.CreatePermissionAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
			_repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Permission>()), Times.Never);
		}

		[Fact]
		public async Task Should_Return_All_Permissions() {
			var permissions = new List<Permission>
			{
				new Permission("Permission 1"),
				new Permission("Permission 2")
			};
			_repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(permissions);

			var result = await _permissionService.GetAllPermissionsAsync();

			Assert.Equal(2, result.Count);
			_repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task Should_Update_Permission_When_Request_Is_Valid_And_Permission_Exists() {
			var request = new UpdatePermissionRequest { Id = 1, Name = "Updated Permission" };
			var existingPermission = new Permission("Old Permission");
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPermission);
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Permission)null);

			var result = await _permissionService.UpdatePermissionAsync(request);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Permission>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Updating_Permission_That_Does_Not_Exist() {
			var request = new UpdatePermissionRequest { Id = 1, Name = "Non-Existent Permission" };
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Permission)null);

			var result = await _permissionService.UpdatePermissionAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Id");
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Permission>()), Times.Never);
		}
		[Fact]
		public async Task Should_Return_False_When_Updating_Permission_And_Name_Is_Duplicate() {
			var request = new UpdatePermissionRequest { Id = 1, Name = "Duplicate Permission" };
			var existingPermission = new Permission("Old Permission");
			var duplicatePermission = new Permission("Duplicate Permission") { Id = 2 };
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPermission);
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(duplicatePermission);

			var result = await _permissionService.UpdatePermissionAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Permission>()), Times.Never);
		}

		[Fact]
		public async Task Should_Delete_Permission_When_Exists() {
			var existingPermission = new Permission("Permission to Delete");
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPermission);

			var result = await _permissionService.DeletePermissionAsync(1);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.DeleteAsync(existingPermission), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Deleting_Permission_That_Does_Not_Exist() {
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Permission)null);

			var result = await _permissionService.DeletePermissionAsync(1);

			Assert.False(result);
			_repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Permission>()), Times.Never);
		}
	}
}
