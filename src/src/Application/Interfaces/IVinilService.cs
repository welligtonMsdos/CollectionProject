using Collection10Api.src.Application.Dtos.Vinil;

namespace Collection10Api.src.Application.Interfaces;

public interface IVinilService: IService
{
    Task<ICollection<VinilDto>> GetAllVinilsAsync();
    Task<VinilDto> GetVinilByIdAsync(int id);

    Task<VinilDto> CreateVinilAsync(VinilCreateDto vinilCreateDto);

    Task<VinilDto> UpdateVinilAsync(VinilUpdateDto vinilUpdateDto);

    Task<bool> DeleteVinilAsync(int id);
}
