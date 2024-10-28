using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Extensions;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Views;

namespace Thoth.Infrastructure.Repositories {
	public class UserRepository : IUserRepository {
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ILoggerService _logger;

		public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager, ILoggerService logger) {
			_userManager = userManager;
			_roleManager = roleManager;
			_logger = logger;
		}

		public async Task AddAsync(User user, string password) {
			var result = await _userManager.CreateAsync(user, password);

			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"User creation failed: {errors}");
				throw new Exception($"User creation failed: {errors}");
			}
		}

		public async Task AddToRolesAsync(User user, IList<int> rolesIds) {
			var roles = await _roleManager.Roles.Where(r => rolesIds.Contains(r.Id)).Select(r => r.NormalizedName).ToListAsync();
			var result = await _userManager.AddToRolesAsync(user, roles);

			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"Adding roles failed: {errors}");
				throw new Exception($"Adding roles failed: {errors}");
			}
		}

		public async Task DeleteAsync(User user) {
			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"User deletion failed: {errors}");
				throw new Exception($"User deletion failed: {errors}");
			}
		}

		public async Task<List<UserView>> GetAllUsersViewsAsync() {
			var usersViews = new List<UserView>();
			var users = await _userManager.Users.Include(u => u.Organization).ToListAsync();
			foreach (var user in users) {
				var roles = await _userManager.GetRolesAsync(user);
				usersViews.Add(user.ToView([.. roles]));
			}
			return usersViews;
		}

		public async Task<User> GetByIdAsync(int id) {
			return await _userManager.FindByIdAsync(id.ToString());
		}

		public async Task<User> GetByEmailAsync(string email) {
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task UpdateAsync(User user) {
			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"User update failed: {errors}");
				throw new Exception($"User update failed: {errors}");
			}
		}

		public async Task UpdatePasswordAsync(User user, string password) {
			var removeResult = await _userManager.RemovePasswordAsync(user);

			if (!removeResult.Succeeded) {
				var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
				_logger.Insert($"Password removal failed: {errors}");
				throw new Exception($"Password removal failed: {errors}");
			}

			var addResult = await _userManager.AddPasswordAsync(user, password);

			if (!addResult.Succeeded) {
				var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
				_logger.Insert($"Password addition failed: {errors}");
				throw new Exception($"Password addition failed: {errors}");
			}
		}

		public async Task RemoveUserRolesAsync(User user) {
			var currentRoles = await _userManager.GetRolesAsync(user);
			var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				_logger.Insert($"Removing user roles failed: {errors}");
				throw new Exception($"Removing user roles failed: {errors}");
			}
		}
	}
}
