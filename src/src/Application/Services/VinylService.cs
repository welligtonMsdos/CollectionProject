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

    public async Task<VinylDto> CreateAsync(VinylCreateDto dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var vinylEntity = _mapper.Map<Vinyl>(dto);        

        vinylEntity.Active = true;

        var createdVinyl = await _efRepository.CreateAsync(vinylEntity);

        return _mapper.Map<VinylDto>(createdVinyl);
    }   

    public async Task<ICollection<VinylDto>> GetAllAsync()
    {
        return _mapper.Map<ICollection<VinylDto>>(await _repository.GetAllAsync());
    }

    public async Task<VinylDto> GetByGuidAsync(Guid guid)
    {
        return _mapper.Map<VinylDto>(await _repository.GetByGuidAsync(guid));
    }

    public async Task<VinylDto> UpdateAsync(VinylUpdateDto dto)
    {
        var vinylEntity = _mapper.Map<Vinyl>(dto);

        vinylEntity.Active = true;

        var updateVinyl = await _efRepository.UpdateAsync(vinylEntity);

        return _mapper.Map<VinylDto>(updateVinyl);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var vinylEntity = await _repository.GetByGuidAsync(guid);

        if (vinylEntity == null) return false;        

        return await _efRepository.DeleteAsync(vinylEntity);
    }
}
