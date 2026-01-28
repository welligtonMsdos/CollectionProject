using AutoMapper;
using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Application.Validators.Vinyl;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.VinylRepo;
using FluentValidation;

namespace Collection10Api.src.Application.Services;

public class VinylService : IVinylService
{
    private readonly IVinylDapperRepository _repository;
    private readonly IVinylEFRepository _efRepository;
    private readonly IMapper _mapper;
    private readonly VinylCreateValidator _validator;

    public VinylService(IVinylDapperRepository repository,
                        IVinylEFRepository efRepository,
                        IMapper mapper,
                        VinylCreateValidator validator)
    {
        _repository = repository;
        _efRepository = efRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<VinylDto> CreateVinylAsync(VinylCreateDto vinylCreateDto)
    {
        await _validator.ValidateAndThrowAsync(vinylCreateDto);

        var vinylEntity = _mapper.Map<Vinyl>(vinylCreateDto);        

        vinylEntity.Active = true;

        var createdVinyl = await _efRepository.CreateVinylAsync(vinylEntity);

        return _mapper.Map<VinylDto>(createdVinyl);
    }   

    public async Task<ICollection<VinylDto>> GetAllVinylsAsync()
    {
        return _mapper.Map<ICollection<VinylDto>>(await _repository.GetAllVinylsAsync());
    }

    public async Task<VinylDto> GetVinylByIdAsync(int id)
    {
        return _mapper.Map<VinylDto>(await _repository.GetVinylByIdAsync(id));
    }

    public async Task<VinylDto> UpdateVinylAsync(VinylUpdateDto vinylUpdateDto)
    {
        var vinylEntity = _mapper.Map<Vinyl>(vinylUpdateDto);

        vinylEntity.Active = true;

        var updateVinyl = await _efRepository.UpdateVinylAsync(vinylEntity);

        return _mapper.Map<VinylDto>(updateVinyl);
    }

    public async Task<bool> DeleteVinylAsync(int id)
    {
        var vinylEntity = await _repository.GetVinylByIdAsync(id);

        return await _efRepository.DeleteVinylAsync(vinylEntity);
    }
}
