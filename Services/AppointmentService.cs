using System.Security.Claims;
using Barber.Data;
using Barber.Dto;
using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Services;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Hairdresser>> GetHairdressers()
    {
        var hairdressers = await _context.Hairdressers
            .Include(u => u.User)
            .ToListAsync();

        return hairdressers;
    }


    public async Task<List<string>> GetAvailableSlotsAsync(int hairdresserId, string date)
    {
        var selectedDate = DateTime.Parse(date);
        
        var occupiedAppointments = await GetOccuppiedAppointmentsAsync(hairdresserId, selectedDate);

        var (hourStart, hourEnd) = GetBussinessHours(selectedDate.DayOfWeek);

        if (hourStart == 0 && hourEnd == 0) return new List<string>();          //Si es domingo, devuelve lista vacia
        
        return GenerateTimeSlots(selectedDate, hourStart, hourEnd, occupiedAppointments);
    }
    
    public async Task<bool> InsertAppointmentAsync(AppointmentDto appointmentdto)
    {
        DateTime mergedTime = MergeTime(appointmentdto);

        var userId = GetUserId();
        
        if (userId == 0) return false;

        if(await AppointmentExistsAsync(appointmentdto.SelectedHairdresserId , mergedTime)) return false;


        var appointment = new Appointment()
        {
            Date = mergedTime,
            UserId = userId,
            HairdresserId = appointmentdto.SelectedHairdresserId,
            IsCanceled =  false
        };

        try
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
        
        
    }
    
    //Private

    
    private async Task<List<DateTime>> GetOccuppiedAppointmentsAsync(int hairdresserId, DateTime date)
    {
        var occupiedAppointments = await _context.Appointments
            .Where(a => a.Hairdresser.Id == hairdresserId && a.Date.Date == date.Date)
            .Select(a => a.Date)            //Da solo los dias que estan ocupados
            .ToListAsync();
        
        return occupiedAppointments;
    }
    
    private (byte start, byte end) GetBussinessHours(DayOfWeek dayOfWeek)
    {
        if (dayOfWeek == DayOfWeek.Saturday)
            return (10, 14);
        else if (dayOfWeek == DayOfWeek.Sunday)
            return (0, 0);              //Cerrado
        else
            return (10, 18);            //Lunes a viernes
    }

    private List<string> GenerateTimeSlots(DateTime selectedDate, byte hourStart, byte hourEnd,
        List<DateTime> occupiedAppointments)
    {
        byte timeOfCutting = 40;
        
        var availableSlots = new List<string>();

        var pointer = selectedDate.Date.AddHours(hourStart);
        var lastHour = selectedDate.Date.AddHours(hourEnd);

        while (pointer.AddMinutes(timeOfCutting) <= lastHour)
        {
            if(!occupiedAppointments.Contains(pointer))
                availableSlots.Add(pointer.ToString("HH:mm"));
            
            pointer = pointer.AddMinutes(timeOfCutting);
        }
        
        return availableSlots;       
    }

    private DateTime MergeTime(AppointmentDto appointment)
    {
        var selectedDate = appointment.SelectedDate.Date;
        var selectedTime = TimeSpan.Parse(appointment.SelectedTime);
        selectedDate = selectedDate.Add(selectedTime);
        
        return selectedDate;
    }

    private async Task<bool> AppointmentExistsAsync(int hairdresserId, DateTime date)
    {
        return await _context.Appointments
            .AnyAsync(a => a.HairdresserId == hairdresserId && a.Date == date);
    }

    private int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return 0;
        }
        
        var userId = int.Parse(userIdClaim);
        
        return userId;
    }
    
}