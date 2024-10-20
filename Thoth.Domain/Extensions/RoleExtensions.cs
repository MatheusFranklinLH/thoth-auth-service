using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Extensions {
	public static class RoleExtensions {
		public static RoleView ToView(this Role role) {
			return new RoleView {
				Id = role.Id,
				Name = role.Name,
				Permissions = role.RolePermissions.Select(rp => rp.Permission.ToView()).ToList()
			};
		}

		public static List<RoleView> ToView(this IEnumerable<Role> roles) {
			return roles.Select(r => r.ToView()).ToList();
		}
	}
}
