using AutoMapper;
using Collection10Api.src.Application.Dtos.Vinil;
using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Application.Validators.Vinil;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.Vinils;
using FluentValidation;

namespace Collection10Api.src.Application.Services;

public class VinilService : IVinilService
{
    private readonly IVinilDapperRepository _repository;
    private readonly IVinilEFRepository _efRepository;
    private readonly IMapper _mapper;
    private readonly VinilCreateValidator _validator;

    public VinilService(IVinilDapperRepository repository,
                        IVinilEFRepository efRepository,
                        IMapper mapper,
                        VinilCreateValidator validator)
    {
        _repository = repository;
        _efRepository = efRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<VinilDto> CreateVinilAsync(VinilCreateDto vinilCreateDto)
    {
        await _validator.ValidateAndThrowAsync(vinilCreateDto);

        var vinilEntity = _mapper.Map<Vinyl>(vinilCreateDto);        

        vinilEntity.Active = true;

        var createdVinil = await _efRepository.CreateVinilAsync(vinilEntity);

        return _mapper.Map<VinilDto>(createdVinil);
    }   

    public async Task<ICollection<VinilDto>> GetAllVinilsAsync()
    {
        return _mapper.Map<ICollection<VinilDto>>(await _repository.GetAllVinilsAsync());
    }

    public async Task<VinilDto> GetVinilByIdAsync(int id)
    {
        return _mapper.Map<VinilDto>(await _repository.GetVinilByIdAsync(id));
    }

    public async Task<VinilDto> UpdateVinilAsync(VinilUpdateDto vinilUpdateDto)
    {
        var vinilEntity = _mapper.Map<Vinyl>(vinilUpdateDto);

        vinilEntity.Active = true;

        var updateVinil = await _efRepository.UpdateVinilAsync(vinilEntity);

        return _mapper.Map<VinilDto>(updateVinil);
    }

    public async Task<bool> DeleteVinilAsync(int id)
    {
        var vinilEntity = await _repository.GetVinilByIdAsync(id);

        return await _efRepository.DeleteVinilAsync(vinilEntity);
    }
}
