using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcertController : Controller
{
    private readonly IConcertService _service;

    public ConcertController(IConcertService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("[Action]")]  
    public async Task<IActionResult> CreateConcert([FromBody] ConcertCreateDto concertCreateDto)
    {       
        var result = await _service.CreateAsync(concertCreateDto);

        return CreatedAtAction(nameof(GetConcertByGuid), 
               new { guid = result.Guid }, 
               Result<ConcertDto>.Ok(result,
                                     "Concert successfully created!"));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllConcerts()
    {
        var concerts = await _service.GetAllAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllConcertsUpcomingAsync()
    {
        var concerts = await _service.GetAllConcertsUpcomingAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllConcertsPastAsync()
    {
        var concerts = await _service.GetAllConcertsPastAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }

    [Authorize]
    [HttpGet("[Action]/{guid}")]
    public async Task<IActionResult> GetConcertByGuid(Guid guid)
    {
        var concert = await _service.GetByGuidAsync(guid);

        if (concert == null)
            return NotFound(Result<object>.Failure("Concert not found."));

        return Ok(Result<ConcertDto>.Ok(concert));
    }

    [Authorize]
    [HttpPut("[Action]")]
    public async Task<IActionResult> UpdateConcert([FromBody] ConcertUpdateDto concertUpdateDto)
    {
        var updatedConcert = await _service.UpdateAsync(concertUpdateDto);

        if (updatedConcert == null)
            return NotFound(Result<object>.Failure("Concert not found for update."));

        return Ok(Result<ConcertDto>.Ok(updatedConcert, "Concert successfully updated!"));
    }

    [Authorize]
    [HttpDelete("[Action]/{guid}")]
    public async Task<IActionResult> DeleteConcert(Guid guid)
    {
        var deletedConcert = await _service.DeleteAsync(guid);

        if (!deletedConcert)
            return NotFound(Result<object>.Failure("Concert not found for deletion."));

        return Ok(Result<bool>.Ok(true, "Concert removed successfully!"));
    }
}
