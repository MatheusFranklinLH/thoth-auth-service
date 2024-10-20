namespace Thoth.Domain.Views {
	public class RoleView {
		public int Id { get; set; }
		public string Name { get; set; }
		public List<PermissionView> Permissions { get; set; }
	}
}
