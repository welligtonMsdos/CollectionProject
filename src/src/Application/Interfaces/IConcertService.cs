using Collection10Api.src.Application.Dtos.Concert;

namespace Collection10Api.src.Application.Interfaces;

public interface IConcertService : IService<ConcertDto>
{
    Task<ICollection<ConcertDto>> GetAllConcertsUpcomingAsync();

    Task<ICollection<ConcertDto>> GetAllConcertsPastAsync();

    Task<ConcertDto> CreateAsync(ConcertCreateDto dto);

    Task<ConcertDto> UpdateAsync(ConcertUpdateDto dto);
}
