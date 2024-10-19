using Flunt.Notifications;
using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class Role : Notifiable<Notification> {
		public int Id { get; private set; }
		public string Name { get; private set; }
		public ICollection<UserRole> UserRoles { get; private set; }
		public ICollection<RolePermission> RolePermissions { get; private set; }

		public Role(string name) {
			Name = name;
			UserRoles = new List<UserRole>();
			RolePermissions = new List<RolePermission>();

			AddNotifications(new Contract<Role>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Role name is required"));
		}
	}
}
