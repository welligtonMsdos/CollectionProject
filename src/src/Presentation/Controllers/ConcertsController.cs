using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ConcertsController : ApiControllerBase
{
    private readonly IConcertService _service;    

    public ConcertsController(IConcertService service)
    {
        _service = service;        
    }   

    [HttpPost]  
    public async Task<IActionResult> Post([FromBody] ConcertCreateDto concertCreateDto)
    {
        var result = await _service.PostAsync(concertCreateDto, UserId);

        return CreatedAtAction(nameof(GetByGuid), 
               new { guid = result.Guid }, 
               Result<ConcertDto>.Ok(result,
                                     "Concert successfully created!"));
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var concerts = await _service.GetAsync(UserId);

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
    
    [HttpGet("Upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        var concerts = await _service.GetUpcomingAsync(UserId);

        return Ok(Result<IEnumerable<ConcertDto>>.Ok(concerts));
    }
   
    [HttpGet("Past")]
    public async Task<IActionResult> GetPast()
    {
        var concerts = await _service.GetPastAsync(UserId);

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
        var updatedConcert = await _service.PutAsync(guid, 
                                                     concertUpdateDto, 
                                                     UserId);

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
