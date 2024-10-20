using Flunt.Notifications;
using Flunt.Validations;
using Thoth.Domain.Interfaces;

namespace Thoth.Domain.Requests {
	public class CreatePermissionRequest : Notifiable<Notification>, IRequest {
		public string Name { get; set; }

		public void Validate() {
			AddNotifications(new Contract<CreatePermissionRequest>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Permission name is required")
				.IsGreaterThan(Name, 2, "Name", "Permission name must have more than 2 characters"));
		}
	}
}
