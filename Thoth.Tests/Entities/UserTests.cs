using Thoth.Domain.Entities;

namespace Thoth.Tests.Domain {
	public class UserTests {
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";
		private const string Password = "Password@123";
		private const int OrganizationId = 1;

		[Fact]
		public void Should_Create_User_When_Valid_Data() {
			var user = new User(UserName, Email, OrganizationId, Password);

			Assert.True(user.IsValid);
			Assert.Empty(user.Notifications);
		}

		[Fact]
		public void Should_Return_Error_When_Name_Is_Empty() {
			var user = new User("", Email, OrganizationId, Password);

			Assert.False(user.IsValid);
			Assert.Contains(user.Notifications, n => n.Key == "Name");
		}

		[Fact]
		public void Should_Return_Error_When_Email_Is_Invalid() {
			var user = new User(UserName, "invalidemail", OrganizationId, Password);

			Assert.False(user.IsValid);
			Assert.Contains(user.Notifications, n => n.Key == "Email");
		}

		[Fact]
		public void Should_Create_User_With_Hashed_Password() {
			var user = new User(UserName, Email, 1, Password);

			Assert.Equal(UserName, user.Name);
			Assert.Equal(Email, user.Email);
			Assert.NotEmpty(user.PasswordHash);
		}

		[Fact]
		public void Should_Update_User_Data() {
			var user = new User(UserName, Email, 1, Password);

			user.Update("Bruce Updated", "bruceupdated@example.com", 2);

			Assert.Equal("Bruce Updated", user.Name);
			Assert.Equal("bruceupdated@example.com", user.Email);
			Assert.Equal(2, user.OrganizationId);
		}

		[Fact]
		public void Should_Add_Role_To_User() {
			var user = new User(UserName, Email, 1, Password);

			user.AddRole(1);

			Assert.Single(user.UserRoles);
			Assert.Contains(user.UserRoles, ur => ur.RoleId == 1);
		}

		[Fact]
		public void Should_Not_Add_Duplicate_Role_To_User() {
			var user = new User(UserName, Email, 1, Password);

			user.AddRole(1);
			user.AddRole(1);

			Assert.Single(user.UserRoles);
		}

		[Fact]
		public void Should_Remove_Role_From_User() {
			var user = new User(UserName, Email, 1, Password);
			user.AddRole(1);
			user.AddRole(2);

			user.RemoveRole(1);

			Assert.Single(user.UserRoles);
			Assert.DoesNotContain(user.UserRoles, ur => ur.RoleId == 1);
		}

		[Fact]
		public void Should_Set_New_Roles_For_User() {
			var user = new User(UserName, Email, 1, Password);
			user.AddRole(1);
			user.AddRole(2);

			user.SetRoles(new List<int> { 3, 4 });

			Assert.Equal(2, user.UserRoles.Count);
			Assert.Contains(user.UserRoles, ur => ur.RoleId == 3);
			Assert.Contains(user.UserRoles, ur => ur.RoleId == 4);
			Assert.DoesNotContain(user.UserRoles, ur => ur.RoleId == 1);
			Assert.DoesNotContain(user.UserRoles, ur => ur.RoleId == 2);
		}
	}
}

