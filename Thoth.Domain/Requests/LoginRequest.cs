using Flunt.Notifications;
using Flunt.Validations;
using Thoth.Domain.Interfaces;

namespace Thoth.Domain.Requests {
	public class LoginRequest : Notifiable<Notification>, IRequest {
		public string Email { get; set; }
		public string Password { get; set; }

		public void Validate() {
			AddNotifications(new Contract<LoginRequest>()
				.Requires()
				.IsEmail(Email, "Email", "Invalid email address")
				.IsNotNullOrEmpty(Password, "Password", "Password is required"));
		}
	}
}
