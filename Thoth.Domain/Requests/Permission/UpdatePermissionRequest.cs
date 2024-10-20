using Flunt.Notifications;
using Flunt.Validations;
using Thoth.API.Interfaces;

namespace Thoth.API.Requests {
	public class UpdatePermissionRequest : Notifiable<Notification>, IRequest {
		public int Id { get; set; }
		public string Name { get; set; }

		public void Validate() {
			AddNotifications(new Contract<UpdatePermissionRequest>()
				.Requires()
				.IsGreaterThan(Id, 0, "Id", "Permission Id must be greater than zero")
				.IsNotNullOrEmpty(Name, "Name", "Permission name is required")
				.IsGreaterThan(Name, 2, "Name", "Permission name must have more than 2 characters"));
		}
	}
}
