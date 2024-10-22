using Thoth.Domain.Entities;
using Thoth.Domain.Extensions;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class UserService {
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ILoggerService _logger;

		public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ITransactionRepository transactionRepository, ILoggerService logger) {
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_transactionRepository = transactionRepository;
			_logger = logger;
		}

		public async Task<IEnumerable<UserView>> GetAllUsersAsync() {
			var users = await _userRepository.GetAllAsync();
			return users.ToView();
		}

		public async Task<bool> CreateUserAsync(CreateUserRequest request) {
			_logger.Insert("Starting user creation");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for user creation");
				return false;
			}

			var existingUser = await _userRepository.GetByEmailAsync(request.Email);
			if (existingUser != null) {
				request.AddNotification("Email", "A user with this email already exists.");
				_logger.Insert("User creation failed: duplicate email");
				return false;
			}

			var user = new User(request.Name, request.Email, request.OrganizationId, request.Password);

			foreach (var roleId in request.RoleIds) {
				user.AddRole(roleId);
			}

			await _userRepository.AddAsync(user);
			_logger.Insert("User created successfully");

			return true;
		}

		public async Task<bool> UpdateUserAsync(UpdateUserRequest request) {
			_logger.Insert("Starting user update");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed for user update");
				return false;
			}

			var user = await _userRepository.GetByIdAsync(request.Id);
			if (user == null) {
				_logger.Insert("User update failed: user not found");
				return false;
			}

			var existingUser = await _userRepository.GetByEmailAsync(request.Email);
			if (existingUser != null && existingUser.Id != request.Id) {
				request.AddNotification("Email", "A user with this email already exists.");
				_logger.Insert("User update failed: duplicate email");
				return false;
			}

			user.Update(request.Name, request.Email, request.OrganizationId);
			user.SetPassword(request.Password);
			user.SetRoles(request.RoleIds);

			await _userRepository.UpdateAsync(user);
			_logger.Insert("User updated successfully");

			return true;
		}

		public async Task<bool> DeleteUserAsync(int id) {
			_logger.Insert("Starting user deletion");

			var user = await _userRepository.GetByIdAsync(id);
			if (user == null) {
				_logger.Insert("User deletion failed: user not found");
				return false;
			}

			await _transactionRepository.BeginTransactionAsync();
			try {
				await _userRepository.DeleteUserRolesAsync(id);
				await _userRepository.DeleteAsync(user);
				await _transactionRepository.CommitAsync();
			}
			catch {
				await _transactionRepository.RollbackAsync();
				_logger.Insert("User deletion failed: an error occurred while deleting in the database");
				return false;
			}

			_logger.Insert("User deleted successfully");
			return true;
		}
	}
}
