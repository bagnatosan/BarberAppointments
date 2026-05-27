using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(15)]
    public required string FirstName { get; set; }
    
    [Required]
    [MaxLength(15)]
    public required string LastName { get; set; }
    
    [Required]
    public required string Phone { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(30)]
    
    public required string Email { get; set; }
    
    [MaxLength(16)]
    public string? Password { get; set; }


    public string Role { get; set; } = "Customer";
    public List<Appointment> Appointments { get; set; } = new();
}