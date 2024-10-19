using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thoth.Domain.Entities;

namespace Thoth.Infrastructure.Mappings {
	public class RoleMap : IEntityTypeConfiguration<Role> {
		public void Configure(EntityTypeBuilder<Role> builder) {
			builder.ToTable("roles", "thoth");

			builder.HasKey(r => r.Id);

			builder.Property(r => r.Id)
				.HasColumnName("id");

			builder.Property(r => r.Name)
				.HasColumnName("name")
				.HasMaxLength(100)
				.IsRequired();
		}
	}
}
