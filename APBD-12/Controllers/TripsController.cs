using APBD_12.DTOs;
using APBD_12.Services;
using Microsoft.AspNetCore.Mvc;
using TestFinal_APBD.Exceptions;

namespace APBD_12.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(
        [FromQuery] int page, [FromQuery] int pageSize)
    {
        try
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("/api/clients/{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _tripService.DeleteClient(idClient);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("{idTrip}/clients")]

    public async Task<IActionResult> AssignClientToTrip(int idTrip,[FromBody]AssignClientDto assignClientDto)
    {
        try
        {
            await _tripService.AssignClientToTrip(assignClientDto, idTrip);
            return NoContent();
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    
        
}
