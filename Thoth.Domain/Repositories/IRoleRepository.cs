using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories;

public interface IRoleRepository {
	public Task<List<Role>> GetAllAsync();
	public Task<Role> GetByNameAsync(string name);
	public Task<Role> GetByIdAsync(int id);
	public Task AddAsync(Role role);
	public Task UpdateAsync(Role role);
	public Task UpdatePermissionsAsync(Role role, List<int> permissionIds);
	public Task DeleteAsync(Role role);
}