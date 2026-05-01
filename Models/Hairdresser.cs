namespace Barber.Models;

public class Hairdresser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<Appointment> Appointments { get; set; }
}