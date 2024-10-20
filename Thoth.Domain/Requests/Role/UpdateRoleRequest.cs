using Flunt.Notifications;
using Flunt.Validations;
using Thoth.API.Interfaces;

namespace Thoth.Domain.Requests {
	public class UpdateRoleRequest : Notifiable<Notification>, IRequest {
		public int Id { get; set; }
		public string Name { get; set; }
		public List<int> PermissionIds { get; set; }

		public void Validate() {
			AddNotifications(new Contract<UpdateRoleRequest>()
				.Requires()
				.IsGreaterThan(Id, 0, "Id", "Role ID must be greater than zero")
				.IsNotNullOrEmpty(Name, "Name", "Role name is required")
				.IsGreaterThan(Name, 2, "Name", "Role name must have more than 2 characters")
				.IsNotNull(PermissionIds, "PermissionIds", "Permission list is required")
				.IsGreaterThan(PermissionIds.Count, 0, "PermissionIds", "At least one permission must be provided"));
		}
	}
}
