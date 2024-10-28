using Microsoft.AspNetCore.Identity;

namespace Thoth.Domain.Entities {
	public class User : IdentityUser<int> {
		public int OrganizationId { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime ModifiedAt { get; private set; }
		public Organization Organization { get; private set; }

		public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
		public void SetModifiedAt(DateTime modifiedAt) => ModifiedAt = modifiedAt;

		public User(string name, string email, int organizationId) {
			UserName = name;
			Email = email;
			OrganizationId = organizationId;
		}
		protected User() { }

		public void Update(string name, string email, int organizationId) {
			UserName = name;
			Email = email;
			OrganizationId = organizationId;
		}
	}
}
