using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConcertsController : Controller
{
    private readonly IConcertService _service;
    private string? userEmail;

    public ConcertsController(IConcertService service)
    {
        _service = service;        
    }
   
    [HttpPost]  
    public async Task<IActionResult> Post([FromBody] ConcertCreateDto concertCreateDto)
    {
        userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var result = await _service.PostAsync(concertCreateDto, userEmail);

        return CreatedAtAction(nameof(GetByGuid), 
               new { guid = result.Guid }, 
               Result<ConcertDto>.Ok(result,
                                     "Concert successfully created!"));
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var concerts = await _service.GetAsync(userEmail);

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
    
    [HttpGet("Upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var concerts = await _service.GetUpcomingAsync(userEmail);

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
   
    [HttpGet("Past")]
    public async Task<IActionResult> GetPast()
    {
        userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var concerts = await _service.GetPastAsync(userEmail);

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
   
    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var concert = await _service.GetByGuidAsync(guid);

        if (concert is null)
            return NotFound(Result<object>.Failure("Concert not found."));

        return Ok(Result<ConcertDto>.Ok(concert));
    }
   
    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> Put(Guid guid, [FromBody] ConcertUpdateDto concertUpdateDto)
    {
        userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var updatedConcert = await _service.PutAsync(guid, concertUpdateDto, userEmail);

        if (updatedConcert is null)
            return NotFound(Result<object>.Failure("Concert not found for update."));

        return Ok(Result<ConcertDto>.Ok(updatedConcert, "Concert successfully updated!"));
    }
    
    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var deletedConcert = await _service.DeleteAsync(guid);

        if (!deletedConcert)
            return NotFound(Result<object>.Failure("Concert not found for deletion."));

        return Ok(Result<bool>.Ok(true, "Concert removed successfully!"));
    }
}
