using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class Permission : BaseEntity {
		public string Name { get; private set; }
		public ICollection<RolePermission> RolePermissions { get; private set; }

		public Permission(string name) {
			Name = name;
			RolePermissions = new List<RolePermission>();

			Validate();
		}

		public void Validate() {
			AddNotifications(new Contract<Permission>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Permission name is required"));
		}

		public void Update(string name) {
			Name = name;
			Validate();
		}
	}
}
