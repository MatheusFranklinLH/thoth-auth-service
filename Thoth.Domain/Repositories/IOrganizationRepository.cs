using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories {
	public interface IOrganizationRepository {
		Task AddAsync(Organization organization);
		Task<Organization> GetByNameAsync(string name);
		Task<List<Organization>> GetAllAsync();
		Task<Organization> GetByIdAsync(int id);
		Task UpdateAsync(Organization organization);
		Task DeleteAsync(Organization organization);
	}
}