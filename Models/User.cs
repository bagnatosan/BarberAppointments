namespace Barber.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required List<Appointment> Appointments { get; set; }
}