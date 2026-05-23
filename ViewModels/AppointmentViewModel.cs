using Barber.Models;

namespace Barber.ViewModels;

public class AppointmentViewModel
{
    public List<Hairdresser> Hairdressers { get; set; } = new();

    public int SelectedHairdresserId { get; set; } = 0;
    
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public string? SelectedTime { get; set; } = "00:00";
}