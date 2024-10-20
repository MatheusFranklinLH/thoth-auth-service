using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class RoleRepository : IRoleRepository {
		private readonly ThothDbContext _context;

		public RoleRepository(ThothDbContext context) {
			_context = context;
		}

		public async Task<IEnumerable<Role>> GetAllAsync() {
			return await _context.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Role> GetByIdAsync(int id) {
			return await _context.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<Role> GetByNameAsync(string name) {
			return await _context.Roles
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Name == name);
		}

		public async Task AddAsync(Role role) {
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Role role) {
			_context.Roles.Update(role);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Role role) {
			_context.Roles.Remove(role);
			await _context.SaveChangesAsync();
		}

		public async Task AddRolePermissionsAsync(List<RolePermission> rolePermissions) {
			_context.RolePermissions.AddRange(rolePermissions);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteRolePermissionsAsync(int roleId, List<int> permissionIdsToKeep) {
			var rolePermissionsToDelete = await _context.RolePermissions
				.Where(rp => rp.RoleId == roleId && !permissionIdsToKeep.Contains(rp.PermissionId))
				.ToListAsync();

			_context.RolePermissions.RemoveRange(rolePermissionsToDelete);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId) {
			return await _context.RolePermissions
				.Where(rp => rp.RoleId == roleId)
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
