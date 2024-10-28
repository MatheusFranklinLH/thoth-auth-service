using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Domain.Extensions;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class RoleService {
		private readonly IRoleRepository _roleRepository;
		private readonly ILoggerService _logger;
		private readonly ITransactionRepository _transactionRepository;

		public RoleService(
			ILoggerService logger,
			IRoleRepository roleRepository,
			ITransactionRepository transactionRepository
		) {
			_roleRepository = roleRepository;
			_logger = logger;
			_transactionRepository = transactionRepository;
		}

		public async Task<IEnumerable<RoleView>> GetAllRolesAsync() {
			var roles = await _roleRepository.GetAllAsync();
			return roles.ToView();
		}

		public async Task<bool> CreateRoleAsync(CreateRoleRequest request) {
			_logger.Insert("Starting role creation");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed for role creation");
				return false;
			}

			var existingRole = await _roleRepository.GetByNameAsync(request.Name);
			if (existingRole != null) {
				request.AddNotification("Name", "A role with this name already exists.");
				_logger.Insert("Role creation failed: duplicate name");
				return false;
			}

			var role = new Role(request.Name);

			foreach (var permissionId in request.PermissionIds) {
				role.AddPermission(permissionId);
			}

			await _roleRepository.AddAsync(role);

			_logger.Insert("Role created successfully");
			return true;
		}

		public async Task<bool> UpdateRoleAsync(UpdateRoleRequest request) {
			_logger.Insert("Starting role update");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed for role update");
				return false;
			}

			var role = await _roleRepository.GetByIdAsync(request.Id);
			if (role == null) {
				request.AddNotification("Id", "Role not found");
				_logger.Insert("Role update failed: role not found");
				return false;
			}

			var existingRole = await _roleRepository.GetByNameAsync(request.Name);
			if (existingRole != null && existingRole.Id != request.Id) {
				request.AddNotification("Name", "A role with this name already exists.");
				_logger.Insert("Role update failed: duplicate name");
				return false;
			}

			role.Update(request.Name);
			role.SetPermissions(request.PermissionIds);
			await _transactionRepository.BeginTransactionAsync();
			try {
				await _roleRepository.UpdateAsync(role);
				await _roleRepository.UpdatePermissionsAsync(role, request.PermissionIds);
				await _transactionRepository.CommitAsync();
			}
			catch {
				await _transactionRepository.RollbackAsync();
				request.AddNotification("Role", "An error occurred while updating the role.");
				_logger.Insert("Role update failed");
				return false;
			}

			_logger.Insert("Role updated successfully");
			return true;
		}

		public async Task<bool> DeleteRoleAsync(int id) {
			_logger.Insert("Starting role deletion");

			var role = await _roleRepository.GetByIdAsync(id);
			if (role == null) {
				_logger.Insert("Role deletion failed: role not found");
				return false;
			}
			role.SetPermissions(new List<int>());
			await _roleRepository.DeleteAsync(role);

			_logger.Insert("Role deleted successfully");
			return true;
		}
	}
}
