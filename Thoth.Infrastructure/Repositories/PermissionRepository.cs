using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class PermissionRepository : IPermissionRepository {
		private readonly ThothDbContext _context;

		public PermissionRepository(ThothDbContext context) {
			_context = context;
		}

		public async Task AddAsync(Permission permission) {
			_context.Permissions.Add(permission);
			await _context.SaveChangesAsync();
		}

		public async Task<Permission> GetByIdAsync(int id) {
			return await _context.Permissions.FindAsync(id);
		}

		public async Task<Permission> GetByNameAsync(string name) {
			return await _context.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.Name == name);
		}

		public async Task<List<Permission>> GetAllAsync() {
			return await _context.Permissions.AsNoTracking().ToListAsync();
		}

		public async Task UpdateAsync(Permission permission) {
			_context.Permissions.Update(permission);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Permission permission) {
			_context.Permissions.Remove(permission);
			await _context.SaveChangesAsync();
		}
	}
}
