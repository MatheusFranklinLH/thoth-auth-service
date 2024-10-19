using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories {
	public interface IOrganizationRepository {
		Task AddAsync(Organization organization);
		Task<Organization> GetByNameAsync(string name);
	}
}
