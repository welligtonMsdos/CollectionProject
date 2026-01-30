using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public interface IConcertDapperRepository : IRepository
{
    Task<ICollection<Concert>> GetAllConcertsAsync();

    Task<ICollection<Concert>> GetAllConcertsUpcomingAsync();

    Task<ICollection<Concert>> GetAllConcertsPastAsync();

    Task<Concert> GetConcertByGuidAsync(Guid guid);
}
