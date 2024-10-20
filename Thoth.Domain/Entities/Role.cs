using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class Role : BaseEntity {
		public string Name { get; private set; }
		public ICollection<UserRole> UserRoles { get; private set; }
		public ICollection<RolePermission> RolePermissions { get; private set; }

		public Role(string name) {
			Name = name;
			UserRoles = new List<UserRole>();
			RolePermissions = new List<RolePermission>();

			Validate();
		}

		private void Validate() {
			AddNotifications(new Contract<Role>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Role name is required"));
		}

		public void Update(string name) {
			Name = name;

			Validate();
		}

		public void AddPermission(int permissionId) {
			if (RolePermissions.Any(rp => rp.PermissionId == permissionId))
				return;
			RolePermissions.Add(new RolePermission(Id, permissionId));
		}

		public void RemovePermission(int permissionId) {
			var rolePermission = RolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
			if (rolePermission is null)
				return;
			RolePermissions.Remove(rolePermission);
		}

		public void SetPermissions(List<int> permissionIds) {
			var existingPermissions = RolePermissions.Select(rp => rp.PermissionId).ToList();

			foreach (var permissionId in permissionIds) {
				if (!existingPermissions.Contains(permissionId))
					AddPermission(permissionId);
			}

			var permissionsToRemove = RolePermissions.Where(rp => !permissionIds.Contains(rp.PermissionId)).ToList();
			foreach (var rolePermission in permissionsToRemove)
				RolePermissions.Remove(rolePermission);
		}
	}
}
