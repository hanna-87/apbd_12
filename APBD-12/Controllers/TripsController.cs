using APBD_12.Services;

namespace APBD_12.Controllers;

public class TripsController
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    
        
}
