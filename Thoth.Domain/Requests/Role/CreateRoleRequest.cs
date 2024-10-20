using Flunt.Notifications;
using Flunt.Validations;
using Thoth.API.Interfaces;

namespace Thoth.Domain.Requests {
	public class CreateRoleRequest : Notifiable<Notification>, IRequest {
		public string Name { get; set; }
		public List<int> PermissionIds { get; set; }

		public void Validate() {
			AddNotifications(new Contract<CreateRoleRequest>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Role name is required")
				.IsGreaterThan(Name, 2, "Name", "Role name must have more than 2 characters")
				.IsNotNull(PermissionIds, "PermissionIds", "Permission list is required")
				.IsGreaterThan(PermissionIds.Count, 0, "PermissionIds", "At least one permission must be provided"));
		}
	}
}
