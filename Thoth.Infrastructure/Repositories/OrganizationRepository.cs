using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class OrganizationRepository : IOrganizationRepository {
		private readonly ThothDbContext _context;

		public OrganizationRepository(ThothDbContext context) {
			_context = context;
		}

		public async Task AddAsync(Organization organization) {
			_context.Organizations.Add(organization);
			await _context.SaveChangesAsync();
		}

		public async Task<Organization> GetByNameAsync(string name) {
			return await _context.Organizations
								 .AsNoTracking()
								 .FirstOrDefaultAsync(o => o.Name == name);
		}
	}
}
