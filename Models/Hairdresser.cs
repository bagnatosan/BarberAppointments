using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class Hairdresser
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required string LastName { get; set; }
    
    [Required]
    public required List<Appointment> Appointments { get; set; }
}