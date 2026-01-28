using Collection10Api.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collection10Api.src.Infrastructure.Mapping;

public class ShowMap : IEntityTypeConfiguration<Show>
{
    public void Configure(EntityTypeBuilder<Show> builder)
    {
        builder.ToTable(nameof(Show));

        builder.HasKey(s => s.Guid);

        builder.Property(s => s.Artist)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Venue)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(s => s.ShowDate)
            .IsRequired();

        builder.Property(s => s.Photo)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasQueryFilter(p => p.Active);

    }
}
