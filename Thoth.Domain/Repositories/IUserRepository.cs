using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Repositories;

public interface IUserRepository {
	public Task<List<UserView>> GetAllUsersViewsAsync();
	public Task<User> GetByEmailAsync(string email);
	public Task<User> GetByIdAsync(int id);
	public Task AddAsync(User user, string password);
	public Task AddToRolesAsync(User user, IList<int> rolesIds);
	public Task RemoveUserRolesAsync(User user);
	public Task UpdateAsync(User user);
	public Task UpdatePasswordAsync(User user, string password);
	public Task DeleteAsync(User user);
}