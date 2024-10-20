using Thoth.Domain.Requests;

namespace Thoth.Tests.Requests {
	public class UpdateRoleRequestTests {
		[Fact]
		public void Should_Return_Valid_When_Id_Name_And_PermissionIds_Are_Valid() {
			var request = new UpdateRoleRequest {
				Id = 1,
				Name = "Admin",
				PermissionIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Return_Invalid_When_Id_Is_Zero() {
			var request = new UpdateRoleRequest {
				Id = 0,
				Name = "Admin",
				PermissionIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Id");
		}

		[Fact]
		public void Should_Return_Invalid_When_PermissionIds_Are_Empty() {
			var request = new UpdateRoleRequest {
				Id = 1,
				Name = "Admin",
				PermissionIds = new List<int>()
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "PermissionIds");
		}
	}
}
