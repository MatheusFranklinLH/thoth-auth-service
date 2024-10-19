using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class RolePermissionMap : IEntityTypeConfiguration<RolePermission> {
		public void Configure(EntityTypeBuilder<RolePermission> builder) {
			builder.ToTable("role_permissions", "thoth");

			builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

			builder.Property(rp => rp.RoleId)
				.HasColumnName("role_id");

			builder.Property(rp => rp.PermissionId)
				.HasColumnName("permission_id");

			builder.HasOne(rp => rp.Role)
				.WithMany(r => r.RolePermissions)
				.HasForeignKey(rp => rp.RoleId);

			builder.HasOne(rp => rp.Permission)
				.WithMany(p => p.RolePermissions)
				.HasForeignKey(rp => rp.PermissionId);
		}
	}
}
