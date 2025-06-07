using APBD_12.DTOs;
using APBD_12.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_12.Services;

public interface ITripService
{
    Task<GetTripsDto> GetTripsAsync(int page, int pageSize);
    Task DeleteClient(int id);

    Task AssignClientToTrip(AssignClientDto assignClientDto, int tripId);
}