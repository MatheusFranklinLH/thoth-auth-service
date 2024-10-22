using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class UserRepository : IUserRepository {
		private readonly ThothDbContext _context;

		public UserRepository(ThothDbContext context) {
			_context = context;
		}

		public async Task<IEnumerable<User>> GetAllAsync() {
			return await _context.Users
				.Include(u => u.Organization)
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.ThenInclude(r => r.RolePermissions)
				.ThenInclude(rp => rp.Permission)
				.AsSplitQuery()
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<User> GetByIdAsync(int id) {
			return await _context.Users
				.Include(u => u.Organization)
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<User> GetByEmailAsync(string email) {
			return await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email == email);
		}


		public async Task AddAsync(User user) {
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(User user) {
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(User user) {
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserRolesAsync(int userId) {
			var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();
			_context.UserRoles.RemoveRange(userRoles);
			await _context.SaveChangesAsync();
		}
	}
}
