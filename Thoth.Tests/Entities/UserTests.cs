using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class UserTests {
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";

		[Fact]
		public void Should_Set_Created_Date_Manually() {
			var user = new User(UserName, Email, 1);
			var modifiedAt = new DateTime(2023, 01, 02);

			user.SetModifiedAt(modifiedAt);

			Assert.Equal(modifiedAt, user.ModifiedAt);
		}

		[Fact]
		public void Should_Set_Modified_Date_Manually() {
			var user = new User(UserName, Email, 1);
			var modifiedAt = new DateTime(2023, 01, 02);

			user.SetModifiedAt(modifiedAt);

			Assert.Equal(modifiedAt, user.ModifiedAt);
		}
	}
}

