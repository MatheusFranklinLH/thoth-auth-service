using Thoth.Domain.Entities;
using Thoth.Domain.Extensions;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
using Thoth.Domain.Views;

namespace Thoth.Domain.Services {
	public class OrganizationService {
		private readonly IOrganizationRepository _repository;
		private readonly ILoggerService _logger;

		public OrganizationService(IOrganizationRepository repository, ILoggerService logger) {
			_repository = repository;
			_logger = logger;
		}

		public async Task<bool> CreateOrganizationAsync(CreateOrganizationRequest request) {
			_logger.Insert("Starting organization creation");

			request.Validate();

			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for organization creation");
				return false;
			}

			var existingOrganization = await _repository.GetByNameAsync(request.Name);
			if (existingOrganization != null) {
				request.AddNotification("Name", "An organization with this name already exists.");
				_logger.Insert("Organization creation failed: duplicate name");
				return false;
			}

			var organization = new Organization(request.Name);
			if (!organization.IsValid) {
				_logger.Insert("Validation failed for organization creation");
				request.AddNotifications(organization.Notifications);
				return false;
			}
			await _repository.AddAsync(organization);

			_logger.Insert("Organization created successfully");
			return true;
		}

		public async Task<List<OrganizationView>> GetAllOrganizationsAsync() {
			var organizations = await _repository.GetAllAsync();
			return organizations.ToView();
		}


		public async Task<bool> UpdateOrganizationAsync(UpdateOrganizationRequest request) {
			_logger.Insert("Starting organization update");

			request.Validate();
			if (!request.IsValid) {
				_logger.Insert("Validation failed fast for organization update");
				return false;
			}

			var organization = await _repository.GetByIdAsync(request.Id);
			if (organization == null) {
				request.AddNotification("Id", "Organization not found");
				_logger.Insert("Organization update failed: organization not found");
				return false;
			}

			var existingOrganization = await _repository.GetByNameAsync(request.Name);
			if (existingOrganization != null && existingOrganization.Id != organization.Id) {
				request.AddNotification("Name", "An organization with this name already exists.");
				_logger.Insert("Organization update failed: duplicate name");
				return false;
			}

			organization.Update(request.Name);
			if (!organization.IsValid) {
				_logger.Insert("Validation failed for organization update");
				request.AddNotifications(organization.Notifications);
				return false;
			}
			await _repository.UpdateAsync(organization);

			_logger.Insert("Organization updated successfully");
			return true;
		}

		public async Task<bool> DeleteOrganizationAsync(int id) {
			_logger.Insert("Starting organization deletion");

			var organization = await _repository.GetByIdAsync(id);
			if (organization == null) {
				_logger.Insert("Organization deletion failed: organization not found");
				return false;
			}

			await _repository.DeleteAsync(organization);

			_logger.Insert("Organization deleted successfully");
			return true;
		}
	}
}
