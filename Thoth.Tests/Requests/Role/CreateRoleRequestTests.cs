using Thoth.Domain.Requests;

namespace Thoth.Tests.Requests {
	public class CreateRoleRequestTests {
		[Fact]
		public void Should_Return_Valid_When_Name_And_PermissionIds_Are_Valid() {
			var request = new CreateRoleRequest {
				Name = "Admin",
				PermissionIds = new List<int> { 1, 2, 3 }
			};

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Return_Invalid_When_Name_Is_Empty() {
			var request = new CreateRoleRequest {
				Name = "",
				PermissionIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Return_Invalid_When_PermissionIds_Are_Empty() {
			var request = new CreateRoleRequest {
				Name = "Admin",
				PermissionIds = new()
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "PermissionIds");
		}
	}
}
