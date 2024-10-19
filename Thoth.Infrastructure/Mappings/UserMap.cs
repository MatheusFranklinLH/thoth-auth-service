using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class UserMap : IEntityTypeConfiguration<User> {
		public void Configure(EntityTypeBuilder<User> builder) {
			builder.ToTable("users", "thoth");

			builder.HasKey(u => u.Id);

			builder.Property(u => u.Id)
				.HasColumnName("id");

			builder.Property(u => u.Name)
				.HasColumnName("name")
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.Email)
				.HasColumnName("email")
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.PasswordHash)
				.HasColumnName("password_hash")
				.IsRequired();

			builder.Property(u => u.OrganizationId)
				.HasColumnName("organization_id")
				.IsRequired();

			builder.Property(o => o.CreatedAt)
				.HasColumnName("created_at")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(o => o.ModifiedAt)
				.HasColumnName("modified_at")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasOne(u => u.Organization)
				.WithMany(o => o.Users)
				.HasForeignKey(u => u.OrganizationId);
		}
	}
}
