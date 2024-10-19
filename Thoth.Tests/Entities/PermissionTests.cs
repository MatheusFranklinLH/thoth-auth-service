using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class PermissionTests {
		[Fact]
		public void Should_Create_Permission_When_Valid_Data() {
			var permission = new Permission("Read");

			Assert.True(permission.IsValid);
			Assert.Empty(permission.Notifications);
		}

		[Fact]
		public void Should_Return_Error_When_Name_Is_Empty() {
			var permission = new Permission("");

			Assert.False(permission.IsValid);
			Assert.Contains(permission.Notifications, n => n.Key == "Name");
		}
	}
}
