using APBD_12.Data;
using APBD_12.DTOs;
using APBD_12.Models;
using Microsoft.AspNetCore.Mvc;
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

    public async  Task DeleteClient(int id)
    {
        var client = await _context.Clients.Include(c => c.ClientTrips).FirstOrDefaultAsync( c => c.IdClient == id);

        if (client == null)
        {
            throw new NotFoundException("Client not found");
        }

        if (!client.ClientTrips.IsNullOrEmpty())
        {
            throw new ConflictException("Client has trips, thus cannot be deleted");
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task AssignClientToTrip(AssignClientDto assignClientDto, int tripId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null)
        {
            throw new NotFoundException("Trip not found");
        }

        if (trip.DateFrom < DateTime.Now)
        {
            throw new ConflictException("The trip has already occured");
        }
        
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == assignClientDto.Pesel);

        if (client != null)
        {
            throw new ConflictException("The client has already occured");
        }

        client = new Client
        {
            FirstName = assignClientDto.FirstName,
            LastName = assignClientDto.LastName,
            Email = assignClientDto.Email,
            Telephone = assignClientDto.Telephone,
            Pesel = assignClientDto.Pesel

        };
        _context.Clients.Add(client);
        
        var join = new ClientTrip {
            RegisteredAt = DateTime.UtcNow,
            PaymentDate  = assignClientDto.PaymentDate, 
            IdClientNavigation = client,
            IdTripNavigation = trip
        };
        
        _context.ClientTrips.Add(join);
        await _context.SaveChangesAsync();
        
    }
    
}