using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Extensions;
using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Application.Validators.Concert;
using Collection10Api.src.Infrastructure.Repositories.ConcertRepo;
using FluentValidation;

namespace Collection10Api.src.Application.Services;

public class ConcertService : IConcertService
{
    private readonly IConcertDapperRepository _repository;
    private readonly IConcertEFRepository _efRepository;   
    private readonly ConcertCreateValidator _validatorCreate;
    private readonly ConcertUpdateValidator _validatorUpdate;

    public ConcertService(IConcertDapperRepository repository, 
                          IConcertEFRepository efRepository,                          
                          ConcertCreateValidator validatorCreate,
                          ConcertUpdateValidator validatorUpdate)
    {
        _repository = repository;
        _efRepository = efRepository;      
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
    }

    public async Task<ConcertDto> PostAsync(ConcertCreateDto concertCreateDto, 
                                            string userId)
    {
        await _validatorCreate.ValidateAndThrowAsync(concertCreateDto);

        var concert = concertCreateDto.ToEntity();        

        concert.Active = true;

        concert.UserId = userId;

        var createdConcert = await _efRepository.PostAsync(concert);

        return createdConcert.ToConcertDto();
    }

    public async Task<ICollection<ConcertDto>> GetAsync(string userId)
    {
        var concerts = await _repository.GetAsync(userId);

        ArgumentNullException.ThrowIfNull(concerts);

        return concerts.Select(c => c.ToConcertDto()).ToList();
    }

    public async Task<ICollection<ConcertDto>> GetUpcomingAsync(string userId)
    {
        var concerts = await _repository.GetUpcomingAsync(userId);

        ArgumentNullException.ThrowIfNull(concerts);

        return concerts.Select(c => c.ToConcertDto()).ToList();
    }

    public async Task<ICollection<ConcertDto>> GetPastAsync(string userId)
    {
        var concerts = await _repository.GetPastAsync(userId);

        ArgumentNullException.ThrowIfNull(concerts);

        return concerts.Select(c => c.ToConcertDto()).ToList();
    }

    public async Task<ConcertDto> GetByGuidAsync(Guid guid)
    {
        var concert = await _repository.GetByGuidAsync(guid);

        ArgumentNullException.ThrowIfNull(concert);

        return concert.ToConcertDto();
    }

    public async Task<ConcertDto> PutAsync(Guid guid, 
                                           ConcertUpdateDto concertUpdateDto,
                                           string userId)
    {
        await _validatorUpdate.ValidateAndThrowAsync(concertUpdateDto);

        var concert = await _repository.GetByGuidAsync(guid);

        ArgumentNullException.ThrowIfNull(concert);

        concert.UpdateEntity(concertUpdateDto);

        concert.Active = true;

        concert.UserId = userId;

        await _efRepository.PutAsync(concert);

        return concert.ToConcertDto();
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var concert = await _repository.GetByGuidAsync(guid);

        if (concert == null) return false;

        return await _efRepository.DeleteAsync(concert);
    }    
}
