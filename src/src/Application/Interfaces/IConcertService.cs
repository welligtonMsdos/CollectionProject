using Collection10Api.src.Application.Dtos.Concert;

namespace Collection10Api.src.Application.Interfaces;

public interface IConcertService : IService<ConcertDto>
{
    Task<ICollection<ConcertDto>> GetUpcomingAsync(string email);

    Task<ICollection<ConcertDto>> GetPastAsync(string email);

    Task<ConcertDto> PostAsync(ConcertCreateDto dto, string email);

    Task<ConcertDto> PutAsync(ConcertUpdateDto dto, string email);
}
