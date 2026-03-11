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

    public async Task<ConcertDto> PostAsync(ConcertCreateDto dto, 
                                            string email)
    {
        await _validatorCreate.ValidateAndThrowAsync(dto);

        var concert = _mapper.Map<Concert>(dto);

        concert.Guid = Guid.NewGuid();

        concert.Active = true;

        concert.Email = email;

        var createdConcert = await _efRepository.PostAsync(concert);

        return _mapper.Map<ConcertDto>(createdConcert);
    }

    public async Task<ICollection<ConcertDto>> GetAsync(string email)
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetAsync(email));
    }

    public async Task<ICollection<ConcertDto>> GetUpcomingAsync(string email)
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetUpcomingAsync(email));
    }

    public async Task<ICollection<ConcertDto>> GetPastAsync(string email)
    {
        return _mapper.Map<ICollection<ConcertDto>>(await _repository.GetPastAsync(email));
    }

    public async Task<ConcertDto> GetByGuidAsync(Guid guid)
    {
        return _mapper.Map<ConcertDto>(await _repository.GetByGuidAsync(guid));
    }

    public async Task<ConcertDto> PutAsync(ConcertUpdateDto dto, string email)
    {
        await _validatorUpdate.ValidateAndThrowAsync(dto);

        var concert = _mapper.Map<Concert>(dto);

        concert.Active = true;      
        
        concert.Email = email;

        var updatedConcert = await _efRepository.PutAsync(concert);

        return _mapper.Map<ConcertDto>(updatedConcert);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var concert = await _repository.GetByGuidAsync(guid);

        if (concert == null) return false;

        return await _efRepository.DeleteAsync(concert);
    }    
}
