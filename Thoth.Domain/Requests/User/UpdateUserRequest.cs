using System.Text.RegularExpressions;
using Flunt.Notifications;
using Flunt.Validations;
using Thoth.Domain.Interfaces;

namespace Thoth.Domain.Requests {
	public partial class UpdateUserRequest : Notifiable<Notification>, IRequest {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int OrganizationId { get; set; }
		public List<int> RoleIds { get; set; }

		public void Validate() {
			AddNotifications(new Contract<UpdateUserRequest>()
				.Requires()
				.IsGreaterThan(Id, 0, "Id", "User ID must be greater than zero")
				.IsNotNullOrEmpty(Name, "Name", "User name is required")
				.IsEmail(Email, "Email", "Invalid email")
				.IsNotNull(RoleIds, "RoleIds", "Role list is required")
				.IsGreaterThan(RoleIds.Count, 0, "RoleIds", "At least one role must be provided"));

			if (!string.IsNullOrWhiteSpace(Password)) {
				AddNotifications(new Contract<UpdateUserRequest>()
					.IsGreaterThan(Password, 8, "Password", "Password must be at least 8 characters long"));

				if (!PasswordHasValidFormat())
					AddNotification("Password", "Password must contain uppercase, lowercase, number, and special character");
			}
		}

		private bool PasswordHasValidFormat() {
			var hasUpperCase = MyRegex().IsMatch(Password);
			var hasLowerCase = MyRegex1().IsMatch(Password);
			var hasNumber = MyRegex2().IsMatch(Password);
			var hasSpecialChar = MyRegex3().IsMatch(Password);

			return hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;
		}

		[GeneratedRegex(@"[A-Z]")]
		private static partial Regex MyRegex();
		[GeneratedRegex(@"[a-z]")]
		private static partial Regex MyRegex1();
		[GeneratedRegex(@"\d")]
		private static partial Regex MyRegex2();
		[GeneratedRegex(@"[!@#$%^&*(),.?\"":{}|<>]")]
		private static partial Regex MyRegex3();
	}
}
