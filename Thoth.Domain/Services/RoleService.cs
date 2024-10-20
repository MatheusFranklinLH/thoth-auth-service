using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class RoleService {
		private readonly IRoleRepository _roleRepository;
		private readonly IPermissionRepository _permissionRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ILoggerService _logger;

		public RoleService(IRoleRepository roleRepository, IPermissionRepository permissionRepository, ITransactionRepository transactionRepository, ILoggerService logger) {
			_roleRepository = roleRepository;
			_permissionRepository = permissionRepository;
			_logger = logger;
			_transactionRepository = transactionRepository;
		}

		public async Task<IEnumerable<RoleView>> GetAllRolesAsync() {
			var roles = await _roleRepository.GetAllAsync();
			return roles.Select(r => new RoleView {
				Id = r.Id,
				Name = r.Name,
				Permissions = r.RolePermissions.Select(rp => new PermissionView {
					Id = rp.Permission.Id,
					Name = rp.Permission.Name
				}).ToList()
			});
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

			foreach (var permissionId in request.PermissionIds)
				role.AddPermission(permissionId);

			await _roleRepository.AddAsync(role);

			_logger.Insert("Role created successfully");
			return true;
		}

		public async Task<bool> UpdateRoleAsync(UpdateRoleRequest request) {
			_logger.Insert("Starting role update");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for role update");
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
			if (!role.IsValid) {
				request.AddNotifications(role.Notifications);
				_logger.Insert("Validation failed for role update");
				return false;
			}

			role.SetPermissions(request.PermissionIds);

			await _roleRepository.UpdateAsync(role);

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
			await _transactionRepository.BeginTransactionAsync();
			try {
				await _roleRepository.DeleteRolePermissionsAsync(id, new List<int>());

				await _roleRepository.DeleteAsync(role);
				await _transactionRepository.CommitAsync();
			}
			catch {
				await _transactionRepository.RollbackAsync();
				_logger.Insert("Role deletion failed: an error occurred while deleting in the database");
				return false;
			}
			_logger.Insert("Role deleted successfully");
			return true;
		}
	}
}
