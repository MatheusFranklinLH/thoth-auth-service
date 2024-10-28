using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Extensions {
	public static class UserExtensions {
		public static UserView ToView(this User user, List<string> roles) {
			return new UserView {
				Id = user.Id,
				Name = user.UserName,
				Email = user.Email,
				Organization = user.Organization.ToView(),
				Roles = roles
			};
		}
	}
}
