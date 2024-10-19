using Flunt.Notifications;
using Flunt.Validations;
using Thoth.API.Interfaces;

namespace Thoth.API.Requests {
	public class CreateOrganizationRequest : Notifiable<Notification>, IRequest {
		public string Name { get; set; }

		public void Validate() {
			AddNotifications(new Contract<CreateOrganizationRequest>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Organization name is required")
				.IsGreaterThan(Name, 2, "Name", "Organization name must have more than 2 characters"));
		}
	}
}
