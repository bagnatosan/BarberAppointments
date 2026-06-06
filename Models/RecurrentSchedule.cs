namespace Barber.Models;

public class RecurrentSchedule
{
    public Guid Id { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int HairdresserId { get; set; }
    public  Hairdresser? Hairdresser { get; set; }
    
    public List<Appointment> ? Appointments { get; set; }
    
    public int IntervalWeeks { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public bool IsActive { get; set; }
}