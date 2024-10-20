using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories {
	public interface IPermissionRepository {
		Task AddAsync(Permission permission);
		Task<Permission> GetByIdAsync(int id);
		Task<Permission> GetByNameAsync(string name);
		Task<List<Permission>> GetAllAsync();
		Task UpdateAsync(Permission permission);
		Task DeleteAsync(Permission permission);
	}
}
