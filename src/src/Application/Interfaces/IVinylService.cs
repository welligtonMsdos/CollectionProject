using Collection10Api.src.Application.Dtos.Vinyl;

namespace Collection10Api.src.Application.Interfaces;

public interface IVinylService: IService<VinylDto>
{
    Task<VinylDto> PostAsync(VinylCreateDto vinylCreateDto, string userId);

    Task<VinylDto> PutAsync(Guid guid, VinylUpdateDto vinylUpdateDto, string userId);
}
