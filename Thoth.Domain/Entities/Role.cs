using Microsoft.AspNetCore.Identity;

namespace Thoth.Domain.Entities {
	public class Role : IdentityRole<int> {
		public DateTime CreatedAt { get; private set; }
		public DateTime ModifiedAt { get; private set; }
		public ICollection<RolePermission> RolePermissions { get; private set; }

		public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
		public void SetModifiedAt(DateTime modifiedAt) => ModifiedAt = modifiedAt;

		public Role(string name) {
			Name = name;
			RolePermissions = new List<RolePermission>();
		}

		public void Update(string name) {
			Name = name;
		}

		public void AddPermission(int permissionId) {
			if (RolePermissions.Any(rp => rp.PermissionId == permissionId))
				return;
			RolePermissions.Add(new RolePermission(Id, permissionId));
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
