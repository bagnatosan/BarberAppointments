using System.ComponentModel.DataAnnotations;

namespace Barber.Models;

public class Appointment
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    [MaxLength(8)]
    public string? Time { get; set; }
    
    public int UserId { get; set; }
    public required User User { get; set; }
    
    public int HairdresserId { get; set; }
    public required Hairdresser Hairdresser { get; set; }
    
    public bool IsDone { get; set; }
}