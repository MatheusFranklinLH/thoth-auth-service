namespace Thoth.Domain.Views {
	public class UserView {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public OrganizationView Organization { get; set; }
		public List<string> Roles { get; set; }
	}
}
