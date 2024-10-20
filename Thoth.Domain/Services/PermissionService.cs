using Thoth.Domain.Entities;
using Thoth.Domain.Extensions;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class PermissionService {
		private readonly IPermissionRepository _repository;
		private readonly ILoggerService _logger;

		public PermissionService(IPermissionRepository repository, ILoggerService logger) {
			_repository = repository;
			_logger = logger;
		}

		public async Task<bool> CreatePermissionAsync(CreatePermissionRequest request) {
			_logger.Insert("Starting permission creation");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for permission creation");
				return false;
			}

			var existingPermission = await _repository.GetByNameAsync(request.Name);
			if (existingPermission != null) {
				request.AddNotification("Name", "A permission with this name already exists.");
				_logger.Insert("Permission creation failed: duplicate name");
				return false;
			}

			var permission = new Permission(request.Name);
			if (!permission.IsValid) {
				request.AddNotifications(permission.Notifications);
				_logger.Insert("Permission creation failed: invalid permission");
				return false;
			}
			await _repository.AddAsync(permission);

			_logger.Insert("Permission created successfully");
			return true;
		}

		public async Task<List<PermissionView>> GetAllPermissionsAsync() {
			var permissions = await _repository.GetAllAsync();
			return permissions.ToView();
		}

		public async Task<bool> UpdatePermissionAsync(UpdatePermissionRequest request) {
			_logger.Insert("Starting permission update");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for permission update");
				return false;
			}

			var permission = await _repository.GetByIdAsync(request.Id);
			if (permission == null) {
				request.AddNotification("Id", "Permission not found");
				_logger.Insert("Permission update failed: permission not found");
				return false;
			}

			var existingPermission = await _repository.GetByNameAsync(request.Name);
			if (existingPermission != null && existingPermission.Id != permission.Id) {
				request.AddNotification("Name", "A permission with this name already exists.");
				_logger.Insert("Permission update failed: duplicate name");
				return false;
			}

			permission.Update(request.Name);

			if (!permission.IsValid) {
				request.AddNotifications(permission.Notifications);
				_logger.Insert("Permission update failed: invalid permission");
				return false;
			}

			await _repository.UpdateAsync(permission);

			_logger.Insert("Permission updated successfully");
			return true;
		}

		public async Task<bool> DeletePermissionAsync(int id) {
			_logger.Insert("Starting permission deletion");

			var permission = await _repository.GetByIdAsync(id);
			if (permission == null) {
				_logger.Insert("Permission deletion failed: permission not found");
				return false;
			}

			await _repository.DeleteAsync(permission);

			_logger.Insert("Permission deleted successfully");
			return true;
		}
	}
}
