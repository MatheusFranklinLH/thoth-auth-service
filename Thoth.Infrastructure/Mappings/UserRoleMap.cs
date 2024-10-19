using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class UserRoleMap : IEntityTypeConfiguration<UserRole> {
		public void Configure(EntityTypeBuilder<UserRole> builder) {
			builder.ToTable("user_roles", "thoth");

			builder.HasKey(ur => new { ur.UserId, ur.RoleId });

			builder.Property(ur => ur.UserId)
				.HasColumnName("user_id");

			builder.Property(ur => ur.RoleId)
				.HasColumnName("role_id");

			builder.HasOne(ur => ur.User)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.UserId);

			builder.HasOne(ur => ur.Role)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.RoleId);
		}
	}
}
