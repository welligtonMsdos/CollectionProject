using AutoMapper;
using Collection10Api.src.Application.Dtos.Show;
using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.Shows;

namespace Collection10Api.src.Application.Services;

public class ShowService : IShowService
{
    private readonly IShowDapperRepository _repository;
    private readonly IShowEFRepository _efRepository;
    private readonly IMapper _mapper;

    public ShowService(IShowDapperRepository repository, 
                       IShowEFRepository efRepository, 
                       IMapper mapper)
    {
        _repository = repository;
        _efRepository = efRepository;
        _mapper = mapper;
    }

    public async Task<ShowDto> CreateShowAsync(ShowCreateDto showCreateDto)
    {
        var showEntity = _mapper.Map<Show>(showCreateDto);

        showEntity.Guid = Guid.NewGuid();

        showEntity.Active = true;

        var createdShow = await _efRepository.CreateShowAsync(showEntity);

        return _mapper.Map<ShowDto>(createdShow);
    }

    public async Task<ICollection<ShowDto>> GetAllShowsAsync()
    {
        return _mapper.Map<ICollection<ShowDto>>(await _repository.GetAllShowsAsync());
    }

    public async Task<ICollection<ShowDto>> GetAllShowsUpcomingAsync()
    {
        return _mapper.Map<ICollection<ShowDto>>(await _repository.GetAllShowsUpcomingAsync());
    }

    public async Task<ICollection<ShowDto>> GetAllShowsPastAsync()
    {
        return _mapper.Map<ICollection<ShowDto>>(await _repository.GetAllShowsPastAsync());
    }

    public async Task<ShowDto> GetShowByGuidAsync(Guid guid)
    {
        return _mapper.Map<ShowDto>(await _repository.GetShowByGuidAsync(guid));
    }

    public async Task<ShowDto> UpdateShowAsync(ShowUpdateDto showUpdateDto)
    {
        var showEntity = _mapper.Map<Show>(showUpdateDto);

        showEntity.Active = true;

        var updateShow = await _efRepository.UpdateShowAsync(showEntity);

        return _mapper.Map<ShowDto>(updateShow);
    }

    public async Task<bool> DeleteShowAsync(Guid guid)
    {
        var showEntity = await _repository.GetShowByGuidAsync(guid);

        return await _efRepository.DeleteShowAsync(showEntity);
    }    
}
