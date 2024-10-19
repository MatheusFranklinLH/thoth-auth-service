using Flunt.Validations;

namespace Thoth.Domain.Entities {
	public class User : BaseEntity {
		public string Name { get; private set; }
		public string Email { get; private set; }
		public string PasswordHash { get; private set; }
		public int OrganizationId { get; private set; }
		public Organization Organization { get; private set; }
		public ICollection<UserRole> UserRoles { get; private set; }

		public User(string name, string email, string passwordHash, int organizationId) {
			Name = name;
			Email = email;
			PasswordHash = passwordHash;
			OrganizationId = organizationId;
			UserRoles = new List<UserRole>();

			AddNotifications(new Contract<User>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Name is required")
				.IsEmail(Email, "Email", "Invalid email format")
				.IsGreaterOrEqualsThan(PasswordHash, 6, "Password", "Password must be at least 6 characters long"));
		}

		public void UpdatePassword(string newPasswordHash) {
			PasswordHash = newPasswordHash;
		}
	}
}
