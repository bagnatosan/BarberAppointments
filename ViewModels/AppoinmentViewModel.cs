using Barber.Models;

namespace Barber.ViewModels;

public class AppoinmentViewModel
{
    public List<Hairdresser> Hairdressers { get; set; } = new();
    
    public int SelectedHairdresserId { get; set; }
    
    public DateTime SelectedDate { get; set; }
    
    public string? SelectedTime { get; set; }
}