using Collection10Api.src.Application.Dtos.Vinyl;

namespace Collection10Api.src.Application.Interfaces;

public interface IVinylService: IService<VinylDto>
{
    Task<VinylDto> CreateAsync(VinylCreateDto dto);

    Task<VinylDto> UpdateAsync(VinylUpdateDto dto);
}
