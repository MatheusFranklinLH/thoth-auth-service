using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Infrastructure.Mappings;

namespace Thoth.Infrastructure.Context {
	public class ThothDbContext : DbContext {
		public ThothDbContext(DbContextOptions<ThothDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new UserMap());
			modelBuilder.ApplyConfiguration(new OrganizationMap());
			modelBuilder.ApplyConfiguration(new RoleMap());
			modelBuilder.ApplyConfiguration(new PermissionMap());
			modelBuilder.ApplyConfiguration(new UserRoleMap());
			modelBuilder.ApplyConfiguration(new RolePermissionMap());

			modelBuilder.Ignore<Notification>();

			base.OnModelCreating(modelBuilder);
		}
	}
}
