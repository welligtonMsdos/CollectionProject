using AutoMapper;
using Collection10Api.src.Application.Dtos.Vinil;
using Collection10Api.src.Application.Services;
using Collection10Api.src.Application.Validators.Vinil;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Profiles;
using Collection10Api.src.Infrastructure.Repositories.Vinils;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Collection10.Tests;

public class VinylServiceTests
{
    private readonly Mock<IVinilDapperRepository> _dapperRepositoy;
    private readonly Mock<IVinilEFRepository> _efRepository;    
    private readonly VinilService _service;
    private readonly IValidator<VinilCreateDto> _validator;

    public VinylServiceTests()
    {
        _dapperRepositoy = new Mock<IVinilDapperRepository>();

        _efRepository = new Mock<IVinilEFRepository>();

        var validator = new VinilCreateValidator();

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
        var dto = new VinilCreateDto("Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     200);

        await _service.CreateVinilAsync(dto);

        _efRepository.Verify(
            r => r.CreateVinilAsync(It.Is<Vinyl>(v =>
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
    public async Task CreateVinylAsync_ShouldFail_WhenDataIsInvalid(string artist, string album, int year, string photo, decimal price)
    {     
        var dto = new VinilCreateDto(artist, album, year, photo, price);
      
        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateVinilAsync(dto));

        _efRepository.Verify(r => r.CreateVinilAsync(It.IsAny<Vinyl>()), Times.Never);
    }

    [Fact]
    public async Task GetAllVinylsAsync_ShouldReturnVinyls()
    {
        var vinils = new List<VinilDto>
        {
            new VinilDto(1, "Pink Floyd", "The Wall", 1979, "https://example.com/thewall.jpg", 200),
            new VinilDto(2, "The Beatles", "Abbey Road", 1969, "https://example.com/abbeyroad.jpg", 150)
        };

        _dapperRepositoy.Setup(r => r.GetAllVinilsAsync())
                        .ReturnsAsync(vinils.Select(v => new Vinyl
                        {
                            Id = v.Id,
                            Artist = v.Artist,
                            Album = v.Album,
                            Year = v.Year,
                            Photo = v.Photo,
                            Price = v.Price
                        }).ToList());

        var result = await _service.GetAllVinilsAsync();

        result.Should().BeEquivalentTo(vinils);

        _dapperRepositoy.Verify(r => r.GetAllVinilsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetVinylByIdAsync_ShouldReturnVinyl()
    {
        var vinil = new VinilDto(1,
                                 "Pink Floyd",
                                 "The Wall",
                                 1979,
                                 "https://example.com/thewall.jpg",
                                 200);

        _dapperRepositoy.Setup(r => r.GetVinilByIdAsync(vinil.Id))
                        .ReturnsAsync(new Vinyl
                        {
                            Id = vinil.Id,
                            Artist = vinil.Artist,
                            Album = vinil.Album,
                            Year = vinil.Year,
                            Photo = vinil.Photo,
                            Price = vinil.Price
                        });

        var result = await _service.GetVinilByIdAsync(vinil.Id);

        result.Should().BeEquivalentTo(vinil);

        _dapperRepositoy.Verify(r => r.GetVinilByIdAsync(vinil.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteVinylAsync_ShouldDeleteVinyl()
    {
        var vinilId = 1;

        _dapperRepositoy.Setup(r => r.GetVinilByIdAsync(vinilId))
                        .ReturnsAsync(new Vinyl
                        {
                            Id = vinilId,
                            Artist = "Pink Floyd",
                            Album = "The Wall",
                            Year = 1979,
                            Photo = "https://example.com/thewall.jpg",
                            Price = 200
                        });

        _efRepository.Setup(r => r.DeleteVinilAsync(It.IsAny<Vinyl>()))
                     .ReturnsAsync(true);

        var result = await _service.DeleteVinilAsync(vinilId);

        result.Should().BeTrue();

        _dapperRepositoy.Verify(r => r.GetVinilByIdAsync(vinilId), Times.Once);

        _efRepository.Verify(r => r.DeleteVinilAsync(It.IsAny<Vinyl>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVinylAsync_ShouldUpdateVinyl()
    {
        var dto = new VinilUpdateDto(1,
                                     "Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     250);

        await _service.UpdateVinilAsync(dto);

        _efRepository.Verify(
            r => r.UpdateVinilAsync(It.Is<Vinyl>(v =>
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
