namespace Barber.Models;

public class Hairdresser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Appointment> Appointments { get; set; }
}