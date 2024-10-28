using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class RoleRepository : IRoleRepository {
		private readonly RoleManager<Role> _roleManager;
		private readonly ThothDbContext _context;
		private readonly ILoggerService _logger;

		public RoleRepository(RoleManager<Role> roleManager, ThothDbContext context, ILoggerService logger) {
			_roleManager = roleManager;
			_context = context;
			_logger = logger;
		}

		public async Task AddAsync(Role role) {
			var result = await _roleManager.CreateAsync(role);
			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"Role creation failed: {errors}");
				throw new Exception($"Role creation failed: {errors}");
			}
		}

		public async Task DeleteAsync(Role role) {
			var result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"Role update failed: {errors}");
				throw new Exception($"Role update failed: {errors}");
			}
			var resultDelete = await _roleManager.DeleteAsync(role);
			if (!resultDelete.Succeeded) {
				var errors = string.Join(", ", resultDelete.Errors.Select(e => e.Description));
				_logger.Insert($"Role deletion failed: {errors}");
				throw new Exception($"Role deletion failed: {errors}");
			}
		}

		public async Task<List<Role>> GetAllAsync() {
			return await _roleManager.Roles
				.Include(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.ToListAsync();
		}

		public async Task<Role> GetByIdAsync(int id) {
			return await _roleManager.Roles
				.Include(r => r.RolePermissions)
				.FirstOrDefaultAsync(r => r.Id == id);
		}


		public async Task<Role> GetByNameAsync(string name) {
			return await _roleManager.FindByNameAsync(name);
		}

		public async Task UpdateAsync(Role role) {
			var result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"Role update failed: {errors}");
				throw new Exception($"Role update failed: {errors}");
			}
		}

		public async Task UpdatePermissionsAsync(Role role, List<int> permissionIds) {
			var rolePermissionsToRemove = role.RolePermissions.Where(rp => !permissionIds.Contains(rp.PermissionId)).ToList();
			_context.RolePermissions.RemoveRange(rolePermissionsToRemove);

			var permissionsToAdd = permissionIds.Except(role.RolePermissions.Select(rp => rp.PermissionId)).ToList();
			await _context.RolePermissions.AddRangeAsync(permissionsToAdd.Select(pid => new RolePermission(role.Id, pid)));

			await _context.SaveChangesAsync();
		}
	}
}
