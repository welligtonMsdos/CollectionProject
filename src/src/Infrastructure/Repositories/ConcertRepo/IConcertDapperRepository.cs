using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public interface IConcertDapperRepository : IDapperRepository<Concert>
{    
    Task<ICollection<Concert>> GetAllConcertsUpcomingAsync();

    Task<ICollection<Concert>> GetAllConcertsPastAsync();
}
