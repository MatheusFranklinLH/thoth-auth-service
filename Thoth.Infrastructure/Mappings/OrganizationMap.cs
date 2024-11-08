using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class OrganizationMap : IEntityTypeConfiguration<Organization> {
		public void Configure(EntityTypeBuilder<Organization> builder) {
			builder.ToTable("organizations", "thoth");

			builder.HasKey(o => o.Id);

			builder.Property(o => o.Id)
				.HasColumnName("id");

			builder.Property(o => o.Name)
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
