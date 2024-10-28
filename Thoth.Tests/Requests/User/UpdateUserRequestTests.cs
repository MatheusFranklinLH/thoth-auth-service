using Thoth.Domain.Requests;

namespace Thoth.Tests.Requests {
	public class UpdateUserRequestTests {
		private const string UserName = "Bruce Wayne";
		private const string Email = "bruce.wayne@example.com";
		private const string Password = "Password@123";
		private const int OrganizationId = 1;
		[Fact]
		public void Should_Return_Valid_When_All_Fields_Are_Valid() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = Password,
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Validate_Without_Password() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.True(request.IsValid);
			Assert.Empty(request.Notifications);
		}

		[Fact]
		public void Should_Return_Invalid_When_Id_Is_Zero() {
			var request = new UpdateUserRequest {
				Id = 0,
				Name = UserName,
				Email = Email,
				Password = Password,
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Id");
		}

		[Fact]
		public void Should_Return_Invalid_When_Email_Is_Invalid() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = "invalid-email",
				Password = Password,
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Email");
		}

		[Fact]
		public void Should_Return_Invalid_When_Password_Is_Too_Short() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = "Short1!",
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}

		[Fact]
		public void Should_Return_Invalid_When_Password_Does_Not_Contain_Uppercase() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = "nottooshort1!",
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}

		[Fact]
		public void Should_Return_Invalid_When_Password_Does_Not_Contain_Lowercase() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = "NOTTOOSHORT1!",
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}

		[Fact]
		public void Should_Return_Invalid_When_Password_Does_Not_Contain_Numbers() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = "nottooSHORT!",
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}

		[Fact]
		public void Should_Return_Invalid_When_Password_Does_Not_Contain_Special_Char() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = "nottooSHORT23",
				OrganizationId = OrganizationId,
				RoleIds = new List<int> { 1, 2 }
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "Password");
		}


		[Fact]
		public void Should_Return_Invalid_When_RoleIds_Are_Empty() {
			var request = new UpdateUserRequest {
				Id = 1,
				Name = UserName,
				Email = Email,
				Password = Password,
				OrganizationId = OrganizationId,
				RoleIds = new List<int>()
			};

			request.Validate();

			Assert.False(request.IsValid);
			Assert.Contains(request.Notifications, n => n.Key == "RoleIds");
		}
	}
}
