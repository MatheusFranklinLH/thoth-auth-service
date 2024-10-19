using Flunt.Notifications;
using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class Permission : Notifiable<Notification> {
		public int Id { get; private set; }
		public string Name { get; private set; }
		public ICollection<RolePermission> RolePermissions { get; private set; }

		public Permission(string name) {
			Name = name;
			RolePermissions = new List<RolePermission>();

			AddNotifications(new Contract<Permission>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Permission name is required"));
		}
	}
}
