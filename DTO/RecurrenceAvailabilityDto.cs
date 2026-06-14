using Barber.Models;

namespace Barber.Dto;

public class RecurrenceAvailabilityDto
{
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    public int UserId { get; set; } = 0;

    public bool HasActiveRecurrence { get; set; } = false;
    
    public bool HasAppointmentRegular  { get; set; } = false;
    
    public bool WeeklyAvailable { get; set; } = false;
    public bool BiweeklyAvailable { get; set; } = false;
}