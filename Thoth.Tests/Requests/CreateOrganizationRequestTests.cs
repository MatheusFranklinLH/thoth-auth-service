using Thoth.API.Requests;

namespace Thoth.Tests.Requests {
	public class CreateOrganizationRequestTests {
		[Fact]
		public void Should_Return_Valid_When_Name_Is_Valid() {
			var request = new CreateOrganizationRequest { Name = "Valid Organization" };

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Return_Invalid_When_Name_Is_Empty() {
			var request = new CreateOrganizationRequest { Name = "" };

			request.Validate();

			Assert.False(request.IsValid);
			Assert.NotEmpty(request.Notifications);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Return_Invalid_When_Name_Is_Too_Short() {
			var request = new CreateOrganizationRequest { Name = "A" };

			request.Validate();

			Assert.False(request.IsValid);
			Assert.NotEmpty(request.Notifications);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
		}
	}
}
