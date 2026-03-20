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
                                            string email)
    {
        await _validatorCreate.ValidateAndThrowAsync(concertCreateDto);

        var concert = concertCreateDto.ToEntity();        

        concert.Active = true;

        concert.Email = email;

        var createdConcert = await _efRepository.PostAsync(concert);

        return createdConcert.ToConcertDto();
    }

    public async Task<ICollection<ConcertDto>> GetAsync(string email)
    {
        var concerts = await _repository.GetAsync(email);

        ArgumentNullException.ThrowIfNull(concerts);

        return concerts.Select(c => c.ToConcertDto()).ToList();
    }

    public async Task<ICollection<ConcertDto>> GetUpcomingAsync(string email)
    {
        var concerts = await _repository.GetUpcomingAsync(email);

        ArgumentNullException.ThrowIfNull(concerts);

        return concerts.Select(c => c.ToConcertDto()).ToList();
    }

    public async Task<ICollection<ConcertDto>> GetPastAsync(string email)
    {
        var concerts = await _repository.GetPastAsync(email);

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
                                           string email)
    {
        await _validatorUpdate.ValidateAndThrowAsync(concertUpdateDto);

        var concert = await _repository.GetByGuidAsync(guid);

        ArgumentNullException.ThrowIfNull(concert);

        concert.UpdateEntity(concertUpdateDto);

        concert.Active = true;

        concert.Email = email;

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
