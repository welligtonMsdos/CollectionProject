using Collection10Api.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collection10Api.src.Infrastructure.Mapping;

public class VinilMap : IEntityTypeConfiguration<Vinil>
{
    public void Configure(EntityTypeBuilder<Vinil> builder)
    {
        builder.ToTable(nameof(Vinil));

        builder.HasKey(v => v.Id);

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
