using Thoth.Domain.Entities;
using Thoth.Domain.Views;

namespace Thoth.Domain.Extensions {
	public static class OrganizationExtensions {
		public static OrganizationView ToView(this Organization organization) {
			return new OrganizationView {
				Id = organization.Id,
				Name = organization.Name
			};
		}

		public static List<OrganizationView> ToView(this List<Organization> organizations) {
			return organizations.Select(o => o.ToView()).ToList();
		}
	}
}
