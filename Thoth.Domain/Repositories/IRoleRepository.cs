using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories {
	public interface IRoleRepository {
		Task<IEnumerable<Role>> GetAllAsync();
		Task<Role> GetByIdAsync(int id);
		Task<Role> GetByNameAsync(string name);
		Task AddAsync(Role role);
		Task UpdateAsync(Role role);
		Task DeleteAsync(Role role);
		Task AddRolePermissionsAsync(List<RolePermission> rolePermissions);
		Task DeleteRolePermissionsAsync(int roleId, List<int> permissionIdsToKeep);
		Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId);
	}
}
