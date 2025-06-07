using APBD_12.Data;
using APBD_12.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestFinal_APBD.Exceptions;

namespace APBD_12.Services;

public class TripService : ITripService
{
    private readonly MasterContext _context;

    public TripService(MasterContext context)
    {
        _context = context;
    }


    public  async Task<GetTripsDto> GetTripsAsync(int page, int pageSize)
    {
        if (page < 1)
        {
            throw new BadRequestException("Invalid page number");
        }

        if (pageSize < 1)
        {
            throw new BadRequestException("Invalid page size");
        }
        
       var tripsToSkip = (page - 1) * pageSize;
       var trips = await _context.Trips.OrderByDescending(t => t.DateFrom).Skip(tripsToSkip).Take(pageSize).Select(t =>
           new TripInfoDto
           {
               Name = t.Name,
               Description = t.Description,
               DateFrom = t.DateFrom,
               DateTo = t.DateTo,
               MaxPeople = t.MaxPeople,
               Countries = t.IdCountries.Select(c => new CountryInfoDto
                   {
                       Name = c.Name
                   }
               ).ToList(),
               Clients = t.ClientTrips.Select(ct => new ClientInfoDto
               {
                   FirstName = ct.IdClientNavigation.FirstName,
                   LastName = ct.IdClientNavigation.LastName,
               }).ToList()
           }).ToListAsync();

       if (trips.IsNullOrEmpty())
       {
           throw new NotFoundException("No trips found");
       }
       
       var totalCount = await _context.Trips.CountAsync();
       var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

       return new GetTripsDto
       {
           AllPages = totalPages,
           PageNum = page,
           PageSize = pageSize,
           Trips = trips
       };

    }
}