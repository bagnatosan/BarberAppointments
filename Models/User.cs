namespace Barber.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public List<Appointment> Appointments { get; set; }
}