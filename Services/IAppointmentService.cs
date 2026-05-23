namespace Barber.Services;

public interface IAppointmentService
{
    Task<List<string>> GetAvailableSlots(int hairdresserId, string date);
}