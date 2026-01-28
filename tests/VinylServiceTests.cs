using AutoMapper;
using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Application.Services;
using Collection10Api.src.Application.Validators.Vinyl;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Profiles;
using Collection10Api.src.Infrastructure.Repositories.VinylRepo;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Collection10.Tests;

public class VinylServiceTests
{
    private readonly Mock<IVinylDapperRepository> _dapperRepositoy;
    private readonly Mock<IVinylEFRepository> _efRepository;    
    private readonly VinilService _service;
    private readonly IValidator<VinylCreateDto> _validator;

    public VinylServiceTests()
    {
        _dapperRepositoy = new Mock<IVinylDapperRepository>();

        _efRepository = new Mock<IVinylEFRepository>();

        var validator = new VinylCreateValidator();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CollectionProfile>();
        });

        var mapper = mapperConfig.CreateMapper();

        _service = new VinilService(_dapperRepositoy.Object, 
                                    _efRepository.Object, 
                                    mapper, 
                                    validator);
    }


    [Fact]
    public async Task CreateVinylAsync_ShouldAddVinyl()
    {
        var dto = new VinylCreateDto("Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     200);

        await _service.CreateVinylAsync(dto);

        _efRepository.Verify(
            r => r.CreateVinylAsync(It.Is<Vinyl>(v =>
                v.Artist == dto.Artist &&
                v.Album == dto.Album &&
                v.Year == dto.Year &&
                v.Photo == dto.Photo &&
                v.Price == dto.Price &&
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
      
        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateVinylAsync(dto));

        _efRepository.Verify(r => r.CreateVinylAsync(It.IsAny<Vinyl>()), Times.Never);
    }

    [Fact]
    public async Task GetAllVinylsAsync_ShouldReturnVinyls()
    {
        var vinyls = new List<VinylDto>
        {
            new VinylDto(1, "Pink Floyd", "The Wall", 1979, "https://example.com/thewall.jpg", 200),
            new VinylDto(2, "The Beatles", "Abbey Road", 1969, "https://example.com/abbeyroad.jpg", 150)
        };

        _dapperRepositoy.Setup(r => r.GetAllVinylsAsync())
                        .ReturnsAsync(vinyls.Select(v => new Vinyl
                        {
                            Id = v.Id,
                            Artist = v.Artist,
                            Album = v.Album,
                            Year = v.Year,
                            Photo = v.Photo,
                            Price = v.Price
                        }).ToList());

        var result = await _service.GetAllVinylsAsync();

        result.Should().BeEquivalentTo(vinyls);

        _dapperRepositoy.Verify(r => r.GetAllVinylsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetVinylByIdAsync_ShouldReturnVinyl()
    {
        var vinyl = new VinylDto(1,
                                 "Pink Floyd",
                                 "The Wall",
                                 1979,
                                 "https://example.com/thewall.jpg",
                                 200);

        _dapperRepositoy.Setup(r => r.GetVinylByIdAsync(vinyl.Id))
                        .ReturnsAsync(new Vinyl
                        {
                            Id = vinyl.Id,
                            Artist = vinyl.Artist,
                            Album = vinyl.Album,
                            Year = vinyl.Year,
                            Photo = vinyl.Photo,
                            Price = vinyl.Price
                        });

        var result = await _service.GetVinylByIdAsync(vinyl.Id);

        result.Should().BeEquivalentTo(vinyl);

        _dapperRepositoy.Verify(r => r.GetVinylByIdAsync(vinyl.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteVinylAsync_ShouldDeleteVinyl()
    {
        var vinilId = 1;

        _dapperRepositoy.Setup(r => r.GetVinylByIdAsync(vinilId))
                        .ReturnsAsync(new Vinyl
                        {
                            Id = vinilId,
                            Artist = "Pink Floyd",
                            Album = "The Wall",
                            Year = 1979,
                            Photo = "https://example.com/thewall.jpg",
                            Price = 200
                        });

        _efRepository.Setup(r => r.DeleteVinylAsync(It.IsAny<Vinyl>()))
                     .ReturnsAsync(true);

        var result = await _service.DeleteVinylAsync(vinilId);

        result.Should().BeTrue();

        _dapperRepositoy.Verify(r => r.GetVinylByIdAsync(vinilId), Times.Once);

        _efRepository.Verify(r => r.DeleteVinylAsync(It.IsAny<Vinyl>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVinylAsync_ShouldUpdateVinyl()
    {
        var dto = new VinylUpdateDto(1,
                                     "Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     250);

        await _service.UpdateVinylAsync(dto);

        _efRepository.Verify(
            r => r.UpdateVinylAsync(It.Is<Vinyl>(v =>
                v.Id == dto.Id &&
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
