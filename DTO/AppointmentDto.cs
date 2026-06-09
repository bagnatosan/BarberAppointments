using Barber.Models;

namespace Barber.Dto;

public class AppointmentDto
{
    public List<Hairdresser> Hairdressers { get; set; } = new();

    public int SelectedHairdresserId { get; set; } = 0;
    
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public string SelectedTime { get; set; } = "00:00";
    
    public int SelectedHaircutId { get; set; } = 0;

    public List<Haircut> Haircuts { get; set; } = new();
    
    public bool Weekly { get; set; } = false;
    public bool BiWeekly { get; set; } = false;
    
    public Guid RecurrentSchedulesId { get; set; } = Guid.Empty;
    public RecurrentSchedule? RecurrentSchedule { get; set; }
}