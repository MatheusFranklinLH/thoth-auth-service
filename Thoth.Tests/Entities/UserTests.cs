using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class UserTests {
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";
		private const string Password = "hashedpassword";
		private const int OrganizationId = 1;

		[Fact]
		public void Should_Create_User_When_Valid_Data() {
			var user = new User(UserName, Email, Password, OrganizationId);

			Assert.True(user.IsValid);
			Assert.Empty(user.Notifications);
		}

		[Fact]
		public void Should_Return_Error_When_Name_Is_Empty() {
			var user = new User("", Email, Password, OrganizationId);

			Assert.False(user.IsValid);
			Assert.Contains(user.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Return_Error_When_Email_Is_Invalid() {
			var user = new User(UserName, "invalidemail", Password, OrganizationId);

			Assert.False(user.IsValid);
			Assert.Contains(user.Notifications, n => n.Key == "Email");
		}

		[Fact]
		public void Should_Return_Error_When_Password_Is_Less_Than_6_Characters() {
			var user = new User(UserName, Email, "123", OrganizationId);

			Assert.False(user.IsValid);
			Assert.Contains(user.Notifications, n => n.Key == "Password");
		}
	}
}

