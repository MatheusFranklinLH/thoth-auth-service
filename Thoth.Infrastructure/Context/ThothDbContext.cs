using Flunt.Notifications;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thoth.Domain.Entities;
using Thoth.Infrastructure.Mappings;

namespace Thoth.Infrastructure.Context {
	public class ThothDbContext : IdentityDbContext<User, Role, int> {
		public ThothDbContext(DbContextOptions<ThothDbContext> options) : base(options) { }

		public DbSet<Organization> Organizations { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new OrganizationMap());
			modelBuilder.ApplyConfiguration(new PermissionMap());
			modelBuilder.ApplyConfiguration(new RolePermissionMap());

			modelBuilder.Ignore<Notification>();

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges() {
			UpdateTimestamps();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) {
			UpdateTimestamps();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void UpdateTimestamps() {
			var now = DateTime.UtcNow;

			foreach (var entry in ChangeTracker.Entries<User>()) {
				if (entry.State == EntityState.Added) {
					entry.Entity.SetCreatedAt(now);
					entry.Entity.SetModifiedAt(now);
				}
				else if (entry.State == EntityState.Modified) {
					entry.Entity.SetModifiedAt(now);
				}
			}

			foreach (var entry in ChangeTracker.Entries<Role>()) {
				if (entry.State == EntityState.Added) {
					entry.Entity.SetCreatedAt(now);
					entry.Entity.SetModifiedAt(now);
				}
				else if (entry.State == EntityState.Modified) {
					entry.Entity.SetModifiedAt(now);
				}
			}

			var entries = ChangeTracker.Entries<BaseEntity>();
			foreach (var entry in entries) {
				if (entry.State == EntityState.Added) {
					entry.Entity.SetCreatedAt(now);
					entry.Entity.SetModifiedAt(now);
				}
				else if (entry.State == EntityState.Modified) {
					entry.Entity.SetModifiedAt(now);
				}
			}
		}
	}
}
