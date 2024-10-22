using Thoth.Domain.Entities;

namespace Thoth.Domain.Repositories {
	public interface IUserRepository {
		Task<IEnumerable<User>> GetAllAsync();
		Task<User> GetByIdAsync(int id);
		Task<User> GetByEmailAsync(string email);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(User user);
		Task DeleteUserRolesAsync(int userId);
	}
}
