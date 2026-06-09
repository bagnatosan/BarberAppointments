using Barber.Dto;
using Barber.Models;

namespace Barber.Services;

public interface IAppointmentService
{
    Task<List<string>> GetAvailableSlotsAsync(int hairdresserId, string date);
    Task<List<Hairdresser>> GetHairdressers();
    
    Task<bool>InsertAppointmentAsync(AppointmentDto appointment);

    Task<List<Haircut>> GetHaircutsAsync();
    Task<bool> CancelAppointmentAsync(DateTime date, int userId);
    Task<RecurrenceAvailabilityDto> CheckRecurrence(AppointmentDto appointmentDto);
}