using Moq;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class RoleServiceTests {
		private readonly Mock<IRoleRepository> _roleRepositoryMock;
		private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
		private readonly Mock<ILoggerService> _loggerMock;
		private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
		private readonly RoleService _roleService;

		public RoleServiceTests() {
			_roleRepositoryMock = new Mock<IRoleRepository>();
			_permissionRepositoryMock = new Mock<IPermissionRepository>();
			_loggerMock = new Mock<ILoggerService>();
			_transactionRepositoryMock = new Mock<ITransactionRepository>();
			_roleService = new RoleService(_roleRepositoryMock.Object, _permissionRepositoryMock.Object, _transactionRepositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Should_Create_Role_And_Permissions_Successfully() {
			var request = new CreateRoleRequest {
				Name = "Admin",
				PermissionIds = new List<int> { 1, 2 }
			};

			_roleRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Role)null);

			var result = await _roleService.CreateRoleAsync(request);

			Assert.True(result);
			_roleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Creating_Role_With_Existing_Name() {
			var request = new CreateRoleRequest {
				Name = "Admin",
				PermissionIds = new List<int> { 1, 2 }
			};

			var existingRole = new Role("Admin");

			_roleRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingRole);

			var result = await _roleService.CreateRoleAsync(request);

			Assert.False(result);
			_roleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Never);
		}

		[Fact]
		public async Task Should_Update_Role_And_Permissions_Successfully() {
			var request = new UpdateRoleRequest {
				Id = 1,
				Name = "Updated Admin",
				PermissionIds = new List<int> { 1, 3 }
			};

			var existingRole = new Role("Admin");
			existingRole.AddPermission(1);
			existingRole.AddPermission(2);

			_roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingRole);

			var result = await _roleService.UpdateRoleAsync(request);

			Assert.True(result);
			_roleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Role>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Updating_Role_With_Existing_Name() {
			var request = new UpdateRoleRequest {
				Id = 1,
				Name = "Existing Role",
				PermissionIds = new List<int> { 1, 2 }
			};

			var existingRole = new Role("Admin");
			var duplicateRole = new Role("Existing Role");
			duplicateRole.SetId(2);

			_roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingRole);
			_roleRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(duplicateRole);

			var result = await _roleService.UpdateRoleAsync(request);

			Assert.False(result);
			_roleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Role>()), Times.Never);
		}

		[Fact]
		public async Task Should_Delete_Role_And_Permissions_Within_Transaction() {
			var existingRole = new Role("Admin");

			_roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingRole);

			var result = await _roleService.DeleteRoleAsync(1);

			Assert.True(result);
			_transactionRepositoryMock.Verify(t => t.BeginTransactionAsync(), Times.Once);
			_transactionRepositoryMock.Verify(t => t.CommitAsync(), Times.Once);
			_roleRepositoryMock.Verify(r => r.DeleteAsync(existingRole), Times.Once);
			_roleRepositoryMock.Verify(r => r.DeleteRolePermissionsAsync(1, It.IsAny<List<int>>()), Times.Once);
		}

		[Fact]
		public async Task Should_Rollback_Transaction_On_Delete_Error() {
			var existingRole = new Role("Admin");

			_roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingRole);
			_roleRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Role>())).ThrowsAsync(new System.Exception());

			var result = await _roleService.DeleteRoleAsync(1);

			Assert.False(result);
			_transactionRepositoryMock.Verify(t => t.BeginTransactionAsync(), Times.Once);
			_transactionRepositoryMock.Verify(t => t.RollbackAsync(), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Deleting_Non_Existent_Role() {
			_roleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Role)null);

			var result = await _roleService.DeleteRoleAsync(1);

			Assert.False(result);
			_transactionRepositoryMock.Verify(t => t.BeginTransactionAsync(), Times.Never);
		}
	}
}
