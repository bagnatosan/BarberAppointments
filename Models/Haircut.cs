using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class Haircut
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = "";
    public int Price { get; set; }
    
    public List<Appointment>? Appointments { get; set; }
}