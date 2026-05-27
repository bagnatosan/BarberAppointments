using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class Hairdresser
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public List<Appointment>? Appointments{ get; set; }
}