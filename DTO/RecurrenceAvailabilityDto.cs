namespace Barber.Dto;

public class RecurrenceAvailabilityDto
{
    public List<DateTime> Dates { get; set; } = new List<DateTime>();

    public int UserId { get; set; } = 0;

    public bool HasActiveRecurrence { get; set; } = false;
    
    public bool HasAppointmentRegular  { get; set; } = false;
    
    public bool WeeklyAvailable { get; set; } = false;
    public bool BiweeklyAvailable { get; set; } = false;
}