using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class PermissionMap : IEntityTypeConfiguration<Permission> {
		public void Configure(EntityTypeBuilder<Permission> builder) {
			builder.ToTable("permissions", "thoth");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				.HasColumnName("id");

			builder.Property(p => p.Name)
				.HasColumnName("name")
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(o => o.CreatedAt)
				.HasColumnName("created_at")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(o => o.ModifiedAt)
				.HasColumnName("modified_at")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}
