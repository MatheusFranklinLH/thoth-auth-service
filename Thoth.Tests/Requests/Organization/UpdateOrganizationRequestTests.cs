using Thoth.Domain.Requests;

namespace Thoth.Tests.Requests {
	public class UpdateOrganizationRequestTests {
		[Fact]
		public void Should_Return_Valid_When_Data_Is_Correct() {
			var request = new UpdateOrganizationRequest {
				Id = 1,
				Name = "Updated Organization"
			};

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Return_Invalid_When_Id_Is_Zero() {
			var request = new UpdateOrganizationRequest {
				Id = 0,
				Name = "Valid Name"
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Id");
		}

		[Fact]
		public void Should_Return_Invalid_When_Name_Is_Empty() {
			var request = new UpdateOrganizationRequest {
				Id = 1,
				Name = ""
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Return_Invalid_When_Name_Is_Too_Short() {
			var request = new UpdateOrganizationRequest {
				Id = 1,
				Name = "A"
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
		}
	}
}
