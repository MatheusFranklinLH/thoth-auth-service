using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class OrganizationTests {
		[Fact]
		public void Should_Create_Organization_When_Valid_Data() {
			var organization = new Organization("Wayne Corp");

			Assert.True(organization.IsValid);
			Assert.Empty(organization.Notifications);
		}

		[Fact]
		public void Should_Return_Error_When_Name_Is_Empty() {
			var organization = new Organization("");

			Assert.False(organization.IsValid);
			Assert.Contains(organization.Notifications, n => n.Key == "Name");
		}
	}
}
