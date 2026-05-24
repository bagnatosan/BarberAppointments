using Barber.Models;

namespace Barber.Dto;

public class AppointmentDto
{
    public List<Hairdresser> Hairdressers { get; set; } = new();

    public int SelectedHairdresserId { get; set; } = 0;
    
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public string? SelectedTime { get; set; } = "00:00";
}