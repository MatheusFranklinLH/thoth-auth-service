using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Extensions {
	public static class UserExtensions {
		public static UserView ToView(this User user) {
			return new UserView {
				Name = user.Name,
				Email = user.Email,
				Organization = user.Organization.ToView(),
				Roles = user.UserRoles.Select(ur => ur.Role.ToView()).ToList()
			};
		}

		public static List<UserView> ToView(this IEnumerable<User> users) {
			return users.Select(u => u.ToView()).ToList();
		}
	}
}
