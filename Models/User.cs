using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Phone { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }


    public string Role { get; set; } = "Customer";
    public List<Appointment> Appointments { get; set; }
}