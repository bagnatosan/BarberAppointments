

namespace Barber.Models;

public class Appointment
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int HairdresserId { get; set; }
    public Hairdresser? Hairdresser { get; set; }
    
}