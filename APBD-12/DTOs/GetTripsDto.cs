using System.ComponentModel.DataAnnotations;
using APBD_12.Models;

namespace APBD_12.DTOs;

public class GetTripsDto
{
    [Required]
    public int PageNum { get; set; }
    [Required]
    public int PageSize { get; set; }
    [Required]
    public int AllPages { get; set; }
    [Required]
    public List<TripInfoDto> Trips { get; set; } = null!;
}

public class TripInfoDto
{
    [Required]
    [MaxLength(120)]
    public String Name { get; set; } = null!;
    [Required]
    [MaxLength(220)]
    public String Description { get; set; } = null!;
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    [Required]
    public int MaxPeople { get; set; }
    [Required]
    public List<CountryInfoDto> Countries { get; set; } = null!;
    [Required]
    public List<ClientInfoDto> Clients { get; set; } = null!;
}


public class CountryInfoDto
{
    [Required]
    [MaxLength(120)]
    public String Name { get; set; } = null!;
}

public class ClientInfoDto
{
    [Required]
    [MaxLength(120)]
    public String FirstName { get; set; } = null!;
    [Required]
    [MaxLength(120)]
    public String LastName { get; set; } = null!;
}