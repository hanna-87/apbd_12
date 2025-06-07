using System.ComponentModel.DataAnnotations;

namespace APBD_12.DTOs;

public class AssignClientDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; } = null!;

    [Required] [MaxLength(120)] public string LastName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string Telephone { get; set; } = null!;
    
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; } = null!;
    
    [Required]
    public int IdTrip { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string TripName { get; set; } = null!;
    

    public DateTime? PaymentDate { get; set; }
    
    

}