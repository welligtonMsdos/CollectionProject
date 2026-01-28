using Collection10Api.src.Application.Dtos.Vinyl;

namespace Collection10Api.src.Application.Interfaces;

public interface IVinylService: IService
{
    Task<ICollection<VinylDto>> GetAllVinylsAsync();
    Task<VinylDto> GetVinylByIdAsync(int id);

    Task<VinylDto> CreateVinylAsync(VinylCreateDto vinylCreateDto);

    Task<VinylDto> UpdateVinylAsync(VinylUpdateDto vinylUpdateDto);

    Task<bool> DeleteVinylAsync(int id);
}
