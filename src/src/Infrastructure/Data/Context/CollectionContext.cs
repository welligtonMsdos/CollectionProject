using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Collection10Api.src.Infrastructure.Data.Context;

public class CollectionContext: DbContext
{
    public CollectionContext(DbContextOptions<CollectionContext> options) : base(options)
    {
    }

    public DbSet<Vinyl> vinyls { get; set; }
    public DbSet<Concert> concerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VinylMap());
        modelBuilder.ApplyConfiguration(new ConcertMap());
    }
}
