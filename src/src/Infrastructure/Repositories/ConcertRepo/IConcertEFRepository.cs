using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public interface IConcertEFRepository : IRepository
{
    Task<Concert> CreateConcertAsync(Concert concert);

    Task<Concert> UpdateConcertAsync(Concert concert);

    Task<bool> DeleteConcertAsync(Concert concert);
}
