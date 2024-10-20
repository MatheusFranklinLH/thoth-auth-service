using Flunt.Notifications;
using Flunt.Validations;
using Thoth.Domain.Interfaces;

namespace Thoth.Domain.Requests {
	public class UpdateOrganizationRequest : Notifiable<Notification>, IRequest {
		public int Id { get; set; }
		public string Name { get; set; }

		public void Validate() {
			AddNotifications(new Contract<UpdateOrganizationRequest>()
				.Requires()
				.IsGreaterThan(Id, 0, "Id", "Organization Id must be greater than zero")
				.IsNotNullOrEmpty(Name, "Name", "Organization name is required")
				.IsGreaterThan(Name, 2, "Name", "Organization name must have more than 2 characters"));
		}
	}
}
