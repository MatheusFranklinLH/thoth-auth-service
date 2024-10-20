using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Extensions {
	public static class PermissionExtensions {
		public static PermissionView ToView(this Permission permission) {
			return new PermissionView {
				Id = permission.Id,
				Name = permission.Name
			};
		}

		public static List<PermissionView> ToView(this List<Permission> permissions) {
			return permissions.Select(p => p.ToView()).ToList();
		}
	}
}
