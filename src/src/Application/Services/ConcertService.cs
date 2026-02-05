using AutoMapper;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Application.Validators.Concert;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.ConcertRepo;
using FluentValidation;

namespace Collection10Api.src.Application.Services;

public class ConcertService : IConcertService
{
    private readonly IConcertDapperRepository _repository;
    private readonly IConcertEFRepository _efRepository;
    private readonly IMapper _mapper;
    private readonly ConcertCreateValidator _validatorCreate;
    private readonly ConcertUpdateValidator _validatorUpdate;

    public ConcertService(IConcertDapperRepository repository, 
                          IConcertEFRepository efRepository, 
                          IMapper mapper,
                          ConcertCreateValidator validatorCreate,
                          ConcertUpdateValidator validatorUpdate)
    {
        _repository = repository;
        _efRepository = efRepository;
        _mapper = mapper;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
    }

    public async Task<ConcertDto> CreateAsync(ConcertCreateDto dto)
    {
        await _validatorCreate.ValidateAndThrowAsync(dto);

        var concert = _mapper.Map<Concert>(dto);

        concert.Guid = Guid.NewGuid();

        concert.Active = true;

        var createdConcert = await _efRepository.CreateAsync(concert);

        return _mapper.Map<ConcertDto>(createdConcert);
    }

    public async Task<ICollection<ConcertDto>> GetAllAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllAsync());
    }

    public async Task<ICollection<ConcertDto>> GetAllConcertsUpcomingAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllConcertsUpcomingAsync());
    }

    public async Task<ICollection<ConcertDto>> GetAllConcertsPastAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllConcertsPastAsync());
    }

    public async Task<ConcertDto> GetByGuidAsync(Guid guid)
    {
        return _mapper.Map<ConcertDto>(await _repository.GetByGuidAsync(guid));
    }

    public async Task<ConcertDto> UpdateAsync(ConcertUpdateDto dto)
    {
        await _validatorUpdate.ValidateAndThrowAsync(dto);

        var concert = _mapper.Map<Concert>(dto);

        concert.Active = true;

        var updatedConcert = await _efRepository.UpdateAsync(concert);

        return _mapper.Map<ConcertDto>(updatedConcert);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var concert = await _repository.GetByGuidAsync(guid);

        return await _efRepository.DeleteAsync(concert);
    }    
}
