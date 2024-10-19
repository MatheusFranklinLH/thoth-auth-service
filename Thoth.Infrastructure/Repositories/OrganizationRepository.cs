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
		public async Task<List<Organization>> GetAllAsync() {
			return await _context.Organizations.AsNoTracking().ToListAsync();
		}

		public async Task<Organization> GetByIdAsync(int id) {
			return await _context.Organizations.FindAsync(id);
		}

		public async Task UpdateAsync(Organization organization) {
			_context.Organizations.Update(organization);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Organization organization) {
			_context.Organizations.Remove(organization);
			await _context.SaveChangesAsync();
		}

	}
}
