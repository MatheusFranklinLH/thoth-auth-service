namespace Thoth.Domain.Views {
	public class UserView {
		public string Name { get; set; }
		public string Email { get; set; }
		public OrganizationView Organization { get; set; }
		public List<RoleView> Roles { get; set; }
	}
}
