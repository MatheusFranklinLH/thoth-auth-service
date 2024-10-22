using Flunt.Validations;
using Microsoft.AspNetCore.Identity;

namespace Thoth.Domain.Entities {
	public class User : BaseEntity {
		public string Name { get; private set; }
		public string Email { get; private set; }
		public string PasswordHash { get; private set; }
		public int OrganizationId { get; private set; }
		public Organization Organization { get; private set; }
		public ICollection<UserRole> UserRoles { get; private set; }

		public User(string name, string email, int organizationId, string password) {
			Name = name;
			Email = email;
			OrganizationId = organizationId;
			UserRoles = new List<UserRole>();
			SetPassword(password);

			Validate();
		}
		protected User() { }

		private void Validate() {
			AddNotifications(new Contract<User>()
				.Requires()
				.IsNotNullOrEmpty(Name, "Name", "Name is required")
				.IsEmail(Email, "Email", "Invalid email format"));
		}

		public void Update(string name, string email, int organizationId) {
			Name = name;
			Email = email;
			OrganizationId = organizationId;
		}

		public void SetPassword(string password) {
			PasswordHash = new PasswordHasher<User>().HashPassword(this, password);
		}

		public void AddRole(int roleId) {
			if (UserRoles.Any(ur => ur.RoleId == roleId))
				return;
			UserRoles.Add(new UserRole(Id, roleId));
		}

		public void RemoveRole(int roleId) {
			var userRole = UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
			if (userRole != null) {
				UserRoles.Remove(userRole);
			}
		}

		public void SetRoles(List<int> roleIds) {
			var existingRoles = UserRoles.Select(ur => ur.RoleId).ToList();

			foreach (var roleId in roleIds) {
				if (!existingRoles.Contains(roleId))
					AddRole(roleId);
			}

			var rolesToRemove = UserRoles.Where(ur => !roleIds.Contains(ur.RoleId)).ToList();
			foreach (var role in rolesToRemove)
				UserRoles.Remove(role);
		}
	}
}
