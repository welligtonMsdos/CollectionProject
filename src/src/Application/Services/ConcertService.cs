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

    public async Task<ConcertDto> CreateConcertAsync(ConcertCreateDto concertCreateDto)
    {
        await _validatorCreate.ValidateAndThrowAsync(concertCreateDto);

        var concert = _mapper.Map<Concert>(concertCreateDto);

        concert.Guid = Guid.NewGuid();

        concert.Active = true;

        var createdConcert = await _efRepository.CreateConcertAsync(concert);

        return _mapper.Map<ConcertDto>(createdConcert);
    }

    public async Task<ICollection<ConcertDto>> GetAllConcertsAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllConcertsAsync());
    }

    public async Task<ICollection<ConcertDto>> GetAllConcertsUpcomingAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllConcertsUpcomingAsync());
    }

    public async Task<ICollection<ConcertDto>> GetAllConcertsPastAsync()
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAllConcertsPastAsync());
    }

    public async Task<ConcertDto> GetConcertByGuidAsync(Guid guid)
    {
        return _mapper.Map<ConcertDto>(await _repository.GetConcertByGuidAsync(guid));
    }

    public async Task<ConcertDto> UpdateConcertAsync(ConcertUpdateDto concertUpdateDto)
    {
        await _validatorUpdate.ValidateAndThrowAsync(concertUpdateDto);

        var concert = _mapper.Map<Concert>(concertUpdateDto);

        concert.Active = true;

        var updatedConcert = await _efRepository.UpdateConcertAsync(concert);

        return _mapper.Map<ConcertDto>(updatedConcert);
    }

    public async Task<bool> DeleteConcertAsync(Guid guid)
    {
        var concert = await _repository.GetConcertByGuidAsync(guid);

        return await _efRepository.DeleteConcertAsync(concert);
    }    
}
