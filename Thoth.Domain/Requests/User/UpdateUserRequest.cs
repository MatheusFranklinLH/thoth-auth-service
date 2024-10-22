using System.Text.RegularExpressions;
using Flunt.Notifications;
using Flunt.Validations;
using Thoth.Domain.Interfaces;

namespace Thoth.Domain.Requests {
	public class UpdateUserRequest : Notifiable<Notification>, IRequest {
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
				.IsGreaterThan(Password, 8, "Password", "Password must be at least 8 characters long")
				.IsNotNullOrEmpty(Password, "Password", "Password is required")
				.IsNotNull(RoleIds, "RoleIds", "Role list is required")
				.IsGreaterThan(RoleIds.Count, 0, "RoleIds", "At least one role must be provided"));

			if (!PasswordHasValidFormat())
				AddNotification("Password", "Password must contain uppercase, lowercase, number, and special character");
		}

		private bool PasswordHasValidFormat() {
			var hasUpperCase = Regex.IsMatch(Password, @"[A-Z]");
			var hasLowerCase = Regex.IsMatch(Password, @"[a-z]");
			var hasNumber = Regex.IsMatch(Password, @"\d");
			var hasSpecialChar = Regex.IsMatch(Password, @"[!@#$%^&*(),.?\"":{}|<>]");

			return hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;
		}
	}
}
