using Collection10Api.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collection10Api.src.Infrastructure.Mapping;

public class VinylMap : IEntityTypeConfiguration<Vinyl>
{
    public void Configure(EntityTypeBuilder<Vinyl> builder)
    {
        builder.ToTable(nameof(Vinyl));

        builder.HasKey(v => v.Guid);

        builder.Property(v => v.Artist)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.Album)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(v => v.Year)
            .IsRequired();  

        builder.Property(v => v.Photo)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(v => v.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasQueryFilter(p => p.Active);
    }
}
