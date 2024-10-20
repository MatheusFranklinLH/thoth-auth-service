using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class RoleTests {
		[Fact]
		public void Should_Create_Role_When_Valid_Data() {
			var role = new Role("Admin");

			Assert.True(role.IsValid);
			Assert.Empty(role.Notifications);
		}

		[Fact]
		public void Should_Return_Error_When_Name_Is_Empty() {
			var role = new Role("");

			Assert.False(role.IsValid);
			Assert.Contains(role.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Update_Role_Name() {
			var role = new Role("Admin");

			role.Update("Super Admin");

			Assert.Equal("Super Admin", role.Name);
		}

		[Fact]
		public void Should_Add_New_Permission_To_Role() {
			var role = new Role("Admin");

			role.AddPermission(1);

			Assert.Single(role.RolePermissions);
			Assert.Contains(role.RolePermissions, rp => rp.PermissionId == 1);
		}

		[Fact]
		public void Should_Not_Add_Duplicate_Permission_To_Role() {
			var role = new Role("Admin");

			role.AddPermission(1);
			role.AddPermission(1);

			Assert.Single(role.RolePermissions);
		}

		[Fact]
		public void Should_Remove_Permission_From_Role() {
			var role = new Role("Admin");
			role.AddPermission(1);
			role.AddPermission(2);

			role.RemovePermission(1);

			Assert.Single(role.RolePermissions);
			Assert.DoesNotContain(role.RolePermissions, rp => rp.PermissionId == 1);
		}

		[Fact]
		public void Should_Set_Permissions_For_Role() {
			var role = new Role("Admin");
			role.AddPermission(1);

			role.SetPermissions([2, 3]);

			Assert.Equal(2, role.RolePermissions.Count);
			Assert.Contains(role.RolePermissions, rp => rp.PermissionId == 2);
			Assert.Contains(role.RolePermissions, rp => rp.PermissionId == 3);
			Assert.DoesNotContain(role.RolePermissions, rp => rp.PermissionId == 1);
		}
	}
}
