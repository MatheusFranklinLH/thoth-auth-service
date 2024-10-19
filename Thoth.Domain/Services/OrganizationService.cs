using Thoth.API.Requests;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;

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
				_logger.Insert("Validation failed for organization creation");
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
	}
}
