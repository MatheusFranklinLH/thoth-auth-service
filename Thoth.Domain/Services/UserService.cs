using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class UserService {
		private readonly IUserRepository _userRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ILoggerService _logger;

		public UserService(
			ITransactionRepository transactionRepository,
			ILoggerService logger,
			IUserRepository userRepository
		) {
			_userRepository = userRepository;
			_transactionRepository = transactionRepository;
			_logger = logger;
		}

		public async Task<List<UserView>> GetAllUsersAsync() {
			return await _userRepository.GetAllUsersViewsAsync();
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

			var user = new User(request.Name, request.Email, request.OrganizationId);
			await _transactionRepository.BeginTransactionAsync();

			try {
				await _userRepository.AddAsync(user, request.Password);
				await _userRepository.AddToRolesAsync(user, request.RoleIds);
				await _transactionRepository.CommitAsync();
				_logger.Insert("User created successfully");
				return true;
			}
			catch {
				await _transactionRepository.RollbackAsync();
				request.AddNotification("User", "An error occurred while creating the user.");
				_logger.Insert("User creation failed due to an exception.");
				return false;
			}
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

			await _transactionRepository.BeginTransactionAsync();
			try {
				await _userRepository.UpdateAsync(user);

				if (!string.IsNullOrEmpty(request.Password)) {
					await _userRepository.UpdatePasswordAsync(user, request.Password);
				}

				await _userRepository.RemoveUserRolesAsync(user);
				await _userRepository.AddToRolesAsync(user, request.RoleIds);

				await _transactionRepository.CommitAsync();
				_logger.Insert("User updated successfully");
				return true;
			}
			catch {
				await _transactionRepository.RollbackAsync();
				request.AddNotification("User", "An error occurred while updating the user.");
				_logger.Insert("User update failed due to an exception.");
				return false;
			}
		}

		public async Task<bool> DeleteUserAsync(int id) {
			_logger.Insert("Starting user deletion");

			var user = await _userRepository.GetByIdAsync(id);
			if (user == null) {
				_logger.Insert("User deletion failed: user not found");
				return false;
			}

			await _userRepository.DeleteAsync(user);
			_logger.Insert("User deleted successfully");
			return true;
		}
	}
}
