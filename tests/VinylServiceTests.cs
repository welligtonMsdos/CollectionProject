using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Application.Services;
using Collection10Api.src.Application.Validators.Vinil;
using Collection10Api.src.Application.Validators.Vinyl;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Repositories.VinylRepo;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Collection10.Tests;

public class VinylServiceTests
{
    private readonly Mock<IVinylDapperRepository> _dapperRepositoy;
    private readonly Mock<IVinylEFRepository> _efRepository;    
    private readonly VinylService _service;   

    public VinylServiceTests()
    {
        _dapperRepositoy = new Mock<IVinylDapperRepository>();

        _efRepository = new Mock<IVinylEFRepository>();

        var validatorCreate = new VinylCreateValidator();

        var validatorUpdate = new VinylUpdateValidator();

        _service = new VinylService(_dapperRepositoy.Object, 
                                    _efRepository.Object,
                                    validatorCreate,
                                    validatorUpdate);
    }


    [Fact]
    public async Task CreateVinylAsync_ShouldAddVinyl()
    {
        var dto = new VinylCreateDto("Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     200);

        var expectedEntity = new Vinyl
        {
            Artist = dto.Artist,
            Album = dto.Album,
            Year = dto.Year,
            Photo = dto.Photo,
            Price = dto.Price
        };

        _efRepository
            .Setup(r => r.PostAsync(It.IsAny<Vinyl>()))
            .ReturnsAsync(expectedEntity);

        var result = await _service.PostAsync(dto);

        Assert.NotNull(result);

        _efRepository.Verify(
            r => r.PostAsync(It.Is<Vinyl>(v =>
                v.Artist == dto.Artist &&
                v.Album == dto.Album &&
                v.Active == true
            )),
            Times.Once
        );
    }

    [Theory]
    [InlineData("", "The Wall", 1979, "https://linkforpictureofalbum.com", 100)]           // Artist is required
    [InlineData("Pink Floyd", "", 1979, "https://linkforpictureofalbum.com", 100)]         // Album is required
    [InlineData("Pi", "The Wall", 1979, "https://linkforpictureofalbum.com", 100)]         // Artist must be at least 3 characters
    [InlineData("Pink Floyd", "The Wall", 1979, "https:", 100)]                            // Photo URL must be at least 10 characters
    [InlineData("Pink Floyd", "The Wall", 1979, "https://linkforpictureofalbum.com", 0)]   // Price must be greater than 0
    public async Task CreateVinylAsync_ShouldFail_WhenDataIsInvalid(string artist, 
                                                                    string album, 
                                                                    int year, 
                                                                    string photo, 
                                                                    decimal price)
    {     
        var dto = new VinylCreateDto(artist, 
                                     album, 
                                     year, 
                                     photo, 
                                     price);
      
        await Assert.ThrowsAsync<ValidationException>(() => _service.PostAsync(dto));

        _efRepository.Verify(r => r.PostAsync(It.IsAny<Vinyl>()), Times.Never);
    }

    [Fact]
    public async Task GetAllVinylsAsync_ShouldReturnVinyls()
    {
        var vinyls = new List<VinylDto>
        {
            new VinylDto(Guid.NewGuid(), "Pink Floyd", "The Wall", 1979, "https://example.com/thewall.jpg", 200),
            new VinylDto(Guid.NewGuid(), "The Beatles", "Abbey Road", 1969, "https://example.com/abbeyroad.jpg", 150)
        };

        _dapperRepositoy.Setup(r => r.GetAsync(""))
                        .ReturnsAsync(vinyls.Select(v => new Vinyl
                        {
                            Guid = v.Guid,
                            Artist = v.Artist,
                            Album = v.Album,
                            Year = v.Year,
                            Photo = v.Photo,
                            Price = v.Price
                        }).ToList());

        var result = await _service.GetAsync("");

        result.Should().BeEquivalentTo(vinyls);

        _dapperRepositoy.Verify(r => r.GetAsync(""), Times.Once);
    }

    [Fact]
    public async Task GetVinylByIdAsync_ShouldReturnVinyl()
    {
        var vinyl = new VinylDto(Guid.NewGuid(),
                                 "Pink Floyd",
                                 "The Wall",
                                 1979,
                                 "https://example.com/thewall.jpg",
                                 200);

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(vinyl.Guid))
                        .ReturnsAsync(new Vinyl
                        {
                            Guid = vinyl.Guid,
                            Artist = vinyl.Artist,
                            Album = vinyl.Album,
                            Year = vinyl.Year,
                            Photo = vinyl.Photo,
                            Price = vinyl.Price
                        });

        var result = await _service.GetByGuidAsync(vinyl.Guid);

        result.Should().BeEquivalentTo(vinyl);

        _dapperRepositoy.Verify(r => r.GetByGuidAsync(vinyl.Guid), Times.Once);
    }

    [Fact]
    public async Task DeleteVinylAsync_ShouldDeleteVinyl()
    {
        var vinilId = Guid.NewGuid();

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(vinilId))
                        .ReturnsAsync(new Vinyl
                        {
                            Guid = vinilId,
                            Artist = "Pink Floyd",
                            Album = "The Wall",
                            Year = 1979,
                            Photo = "https://example.com/thewall.jpg",
                            Price = 200
                        });

        _efRepository.Setup(r => r.DeleteAsync(It.IsAny<Vinyl>()))
                     .ReturnsAsync(true);

        var result = await _service.DeleteAsync(vinilId);

        result.Should().BeTrue();

        _dapperRepositoy.Verify(r => r.GetByGuidAsync(vinilId), Times.Once);

        _efRepository.Verify(r => r.DeleteAsync(It.IsAny<Vinyl>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVinylAsync_ShouldUpdateVinyl()
    {
        var dto = new VinylUpdateDto("Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     250);

        Guid guid = Guid.NewGuid();

        var existingVinyl = new Vinyl { 
            Guid = guid,
            Artist = "Pink Floyd",
            Album = "The Wall",
            Year = 1979,
            Photo = "https://example.com/thewall.jpg",
            Price = 250
        };

        _dapperRepositoy.Setup(r => r.GetByGuidAsync(guid))
               .ReturnsAsync(existingVinyl);

        await _service.PutAsync(guid, dto);

        _efRepository.Verify(
        r => r.PutAsync(It.Is<Vinyl>(v =>
            v.Guid == guid &&
            v.Artist == dto.Artist &&
            v.Album == dto.Album &&
            v.Year == dto.Year &&
            v.Photo == dto.Photo &&
            v.Price == dto.Price
        )),
        Times.Once
    );
    }
}
