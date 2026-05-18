namespace Barber.Services;

public interface IAppoinmentService
{
    Task<List<string>> GetAvailableSlots(int hairdresserId, string date);
}