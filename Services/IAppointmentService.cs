using Barber.Models;

namespace Barber.Services;

public interface IAppointmentService
{
    Task<List<string>> GetAvailableSlotsAsync(int hairdresserId, string date);
    Task<List<Hairdresser>> GetHairdressers();
}