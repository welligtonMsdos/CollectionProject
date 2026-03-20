using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Services;
using Collection10Api.src.Application.Validators.Concert;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.ConcertRepo;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Globalization;

namespace Collection10Tests;

public class ConcertServiceTests
{
    private readonly Mock<IConcertDapperRepository> _dapperRepositoy;
    private readonly Mock<IConcertEFRepository> _efRepository;
    private readonly ConcertService _service;   
    private readonly string _email = "test@gmail.com";
    private readonly DateOnly _date = DateOnly.Parse(DateTime.Now.AddDays(30).ToShortDateString());
    private readonly DateOnly _pastDate = DateOnly.Parse(DateTime.Now.AddDays(-30).ToShortDateString());
    private readonly string _dateString = DateTime.Now.AddDays(30).ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));
    private readonly string _pastDateString = DateTime.Now.AddDays(-30).ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));

    public ConcertServiceTests()
    {
        _dapperRepositoy = new Mock<IConcertDapperRepository>();

        _efRepository = new Mock<IConcertEFRepository>();

        var validatorCreate = new ConcertCreateValidator();

        var validatorUpdate = new ConcertUpdateValidator();        

        _service = new ConcertService(_dapperRepositoy.Object,
                                      _efRepository.Object,                                      
                                      validatorCreate,
                                      validatorUpdate);
    }

    [Fact]
    public async Task PostConcertAsync_ShouldAddConcert()
    {
        var dto = new ConcertCreateDto("Pink Floyd", 
                                       "The Wall Tour",
                                       _date,
                                       "https://example.com/thewalltour.jpg");       

        var expectedEntity = new Concert
        {           
            Artist = dto.Artist,
            Venue = dto.Venue,
            ShowDate = dto.ShowDate,
            Photo = dto.Photo,
            Active = true,
            Email = _email
        };

        _efRepository
             .Setup(r => r.PostAsync(It.IsAny<Concert>()))
             .ReturnsAsync(expectedEntity);

        var result = await _service.PostAsync(dto, _email);

        Assert.NotNull(result);

        _efRepository.Verify(
            r => r.PostAsync(It.Is<Concert>(v =>               
                v.Artist == dto.Artist &&
                v.Venue == dto.Venue &&
                v.ShowDate == dto.ShowDate &&
                v.Photo == dto.Photo &&
                v.Active == true &&
                v.Email == _email
            )),
            Times.Once
        );
    }

    [Theory]
    [InlineData("", "The Wall Tour", "2026-03-10", "https://linkforpictureofconcert.com")]   // Artist is required
    [InlineData("Pink Floyd", "", "2026-03-10", "https://linkforpictureofconcert.com")]      // Venue is required
    [InlineData("Pi", "The Wall Tour", "2026-03-10", "https://linkforpictureofconcert.com")] // Artist must be at least 3 characters
    [InlineData("Pink Floyd", "The Wall Tour", "2026-03-10", "https:")]                      // Photo URL must be at least 10 characters
    public async Task PostConcertAsync_ShouldFail_WhenDataIsInvalid(string artist,
                                                                    string venue,
                                                                    string showDate,
                                                                    string photo)
    {
        var dto = new ConcertCreateDto(artist,
                                       venue,
                                       DateOnly.Parse(showDate),
                                       photo);       

        await Assert.ThrowsAsync<ValidationException>(() => _service.PostAsync(dto, _email));

        _efRepository.Verify(r => r.PostAsync(It.IsAny<Concert>()), Times.Never);
    }

    [Fact]
    public async Task GetConcertsAsync_ShouldReturnConcerts()
    {
        var concerts = new List<ConcertDto>
        {
            new ConcertDto(Guid.NewGuid(), 
                           "Pink Floyd", 
                           "The Wall Tour",
                           _date,
                           _dateString,                            
                           "https://example.com/thewalltour.jpg"),
            new ConcertDto(Guid.NewGuid(), 
                           "The Beatles", 
                           "Abbey Road Live",
                           _date,
                           _dateString,
                           "https://example.com/abbeyroadlive.jpg")
        };       

        _dapperRepositoy.Setup(r => r.GetAsync(_email))
                        .ReturnsAsync(concerts.Select(c => new Concert
                        {
                            Guid = c.Guid,
                            Artist = c.Artist,
                            Venue = c.Venue,
                            ShowDate = c.ShowDate,
                            Photo = c.Photo,
                            Active = true,
                            Email = _email
                        }).ToList());

        var result = await _service.GetAsync(_email);

        result.Should().BeEquivalentTo(concerts);
        
        _dapperRepositoy.Verify(r => r.GetAsync(_email), Times.Once);
    }

    [Fact]
    public async Task GetConcertByGuidAsync_shouldReturnConcert()
    {
        var concert = new ConcertDto(Guid.NewGuid(),
                                     "Pink Floyd",
                                     "The Wall Tour",
                                     _date,
                                     _dateString,
                                     "https://example.com/thewalltour.jpg");       

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(concert.Guid))
                        .ReturnsAsync(new Concert
                        {
                            Guid = concert.Guid,
                            Artist = concert.Artist,
                            Venue = concert.Venue,
                            ShowDate = concert.ShowDate,
                            Photo = concert.Photo,
                            Active = true,
                            Email = _email
                        });

        var result = await _service.GetByGuidAsync(concert.Guid);

        result.Should().BeEquivalentTo(concert);

        _dapperRepositoy.Verify(r => r.GetByGuidAsync(concert.Guid), Times.Once);
    }

    [Fact]
    public async Task GetPastAsync_shouldReturnPastConcerts()
    {
        var concerts = new List<ConcertDto>
        {
            new ConcertDto(Guid.NewGuid(),
                           "Pink Floyd",
                           "The Wall Tour",
                           _pastDate,
                           _pastDateString,
                           "https://example.com/thewalltour.jpg")
        };
      
        _dapperRepositoy.Setup(r => r.GetPastAsync(_email))
                        .ReturnsAsync(concerts.Select(c => new Concert
                        {
                            Guid = c.Guid,
                            Artist = c.Artist,
                            Venue = c.Venue,
                            ShowDate = c.ShowDate,
                            Photo = c.Photo,
                            Active = true,
                            Email = _email
                        }).ToList());

        var result = await _service.GetPastAsync(_email);

        result.Should().BeEquivalentTo(concerts);

        _dapperRepositoy.Verify(r => r.GetPastAsync(_email), Times.Once);
    }

    [Fact]
    public async Task GetUpcomingAsync_shouldReturnUpcomingConcerts()
    {
        var concerts = new List<ConcertDto>
        {
            new ConcertDto(Guid.NewGuid(),
                           "Pink Floyd",
                           "The Wall Tour",
                           _date,
                           _dateString,
                           "https://example.com/thewalltour.jpg")
        };
        var email = "";
        _dapperRepositoy.Setup(r => r.GetUpcomingAsync(email))
                        .ReturnsAsync(concerts.Select(c => new Concert
                        {
                            Guid = c.Guid,
                            Artist = c.Artist,
                            Venue = c.Venue,
                            ShowDate = c.ShowDate,
                            Photo = c.Photo,
                            Active = true,
                            Email = email
                        }).ToList());

        var result = await _service.GetUpcomingAsync(email);

        result.Should().BeEquivalentTo(concerts);

        _dapperRepositoy.Verify(r => r.GetUpcomingAsync(email), Times.Once);
    }

    [Fact]
    public async Task DeleteConcertAsync_shouldDeleteConcert()
    {
        var concertId = Guid.NewGuid();

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(concertId))
                        .ReturnsAsync(new Concert
                        {
                            Guid = concertId,
                            Artist = "Pink Floyd",
                            Venue = "The Wall Tour",
                            ShowDate = _date,
                            Photo = "https://example.com/thewalltour.jpg",
                            Active = true,
                            Email = _email
                        });

        _efRepository.Setup(r => r.DeleteAsync(It.IsAny<Concert>()))
                     .ReturnsAsync(true);

        var result = await _service.DeleteAsync(concertId);

        result.Should().BeTrue();

        _dapperRepositoy.Verify(r => r.GetByGuidAsync(concertId), Times.Once);

        _efRepository.Verify(r => r.DeleteAsync(It.IsAny<Concert>()), Times.Once);
    }

    [Fact]
    public async Task PutConcertAsync_ShouldPutConcert()
    {
        var dto = new ConcertUpdateDto("Pink Floyd",
                                       "The Wall Tour",
                                       _date,
                                       "https://example.com/thewalltour.jpg");

        Guid guid = Guid.NewGuid();

        var existingVinyl = new Concert
        {
            Guid = guid,
            Artist = "Pink Floyd",
            Venue = "The Wall Tour",
            ShowDate = _date,
            Photo = "https://example.com/thewalltour.jpg",
            Active = true,
            Email = _email
        };

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(guid))
               .ReturnsAsync(existingVinyl);

        await _service.PutAsync(guid, dto, _email);

        _efRepository.Verify(
        r => r.PutAsync(It.Is<Concert>(v =>
            v.Guid == guid &&
            v.Artist == dto.Artist &&
            v.Venue == dto.Venue &&
            v.ShowDate == dto.ShowDate &&
            v.Photo == dto.Photo &&
            v.Active == true &&
            v.Email == _email
        )),
        Times.Once
    );
    }   
}
