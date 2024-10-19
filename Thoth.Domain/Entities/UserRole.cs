namespace Thoth.Domain.Entities {
	public class UserRole : BaseEntity {
		public int UserId { get; private set; }
		public User User { get; private set; }

		public int RoleId { get; private set; }
		public Role Role { get; private set; }

		public UserRole(int userId, int roleId) {
			UserId = userId;
			RoleId = roleId;
		}
	}
}
