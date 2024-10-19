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
	}
}
