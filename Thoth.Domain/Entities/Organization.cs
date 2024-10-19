using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class Organization : BaseEntity {
		public string Name { get; private set; }
		public ICollection<User> Users { get; private set; }

		public Organization(string name) {
			Name = name;
			Users = new List<User>();

			AddNotifications(new Contract<Organization>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Organization name is required"));
		}

		public void Update(string name) {
			Name = name;

			AddNotifications(new Contract<Organization>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Organization name is required"));
		}
	}
}
