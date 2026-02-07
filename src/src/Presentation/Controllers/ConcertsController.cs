using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConcertsController : Controller
{
    private readonly IConcertService _service;

    public ConcertsController(IConcertService service)
    {
        _service = service;
    }
   
    [HttpPost]  
    public async Task<IActionResult> Post([FromBody] ConcertCreateDto concertCreateDto)
    {       
        var result = await _service.CreateAsync(concertCreateDto);

        return CreatedAtAction(nameof(GetByGuid), 
               new { guid = result.Guid }, 
               Result<ConcertDto>.Ok(result,
                                     "Concert successfully created!"));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var concerts = await _service.GetAllAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
    
    [HttpGet("Upcomming")]
    public async Task<IActionResult> GetAllUpcomingAsync()
    {
        var concerts = await _service.GetAllConcertsUpcomingAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
   
    [HttpGet("Past")]
    public async Task<IActionResult> GetAllPastAsync()
    {
        var concerts = await _service.GetAllConcertsPastAsync();

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
   
    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var concert = await _service.GetByGuidAsync(guid);

        if (concert == null)
            return NotFound(Result<object>.Failure("Concert not found."));

        return Ok(Result<ConcertDto>.Ok(concert));
    }
   
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ConcertUpdateDto concertUpdateDto)
    {
        var updatedConcert = await _service.UpdateAsync(concertUpdateDto);

        if (updatedConcert == null)
            return NotFound(Result<object>.Failure("Concert not found for update."));

        return Ok(Result<ConcertDto>.Ok(updatedConcert, "Concert successfully updated!"));
    }
    
    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> DeleteConcert(Guid guid)
    {
        var deletedConcert = await _service.DeleteAsync(guid);

        if (!deletedConcert)
            return NotFound(Result<object>.Failure("Concert not found for deletion."));

        return Ok(Result<bool>.Ok(true, "Concert removed successfully!"));
    }
}
