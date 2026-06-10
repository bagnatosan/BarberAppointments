using System.Runtime.InteropServices.JavaScript;
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

    public async Task<List<Haircut>> GetHaircutsAsync()
    {
        var haircuts = await _context.Haircuts.AsNoTracking().ToListAsync();
        return haircuts;
    }

    public async Task<bool> CancelAppointmentAsync(DateTime date, int userId)
    {
        var appointment = await _context.Appointments
            .SingleOrDefaultAsync(a => a.Date == date && a.UserId == userId && a.IsCanceled == false);

        if (appointment == null) return false;

        appointment.IsCanceled = true;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<string>> GetAvailableSlotsAsync(int hairdresserId, string date)
    {
        var selectedDate = DateTime.Parse(date);
        
        var occupiedAppointments = await GetOccuppiedAppointmentsAsync(hairdresserId, selectedDate);

        var (hourStart, hourEnd) = GetBussinessHours(selectedDate.DayOfWeek);

        if (hourStart == 0 && hourEnd == 0) return new List<string>();          //Si es domingo, devuelve lista vacia
        
        return GenerateTimeSlots(selectedDate, hourStart, hourEnd, occupiedAppointments);
    }
    
    public async Task<RecurrenceAvailabilityDto> CheckRecurrence(AppointmentDto appointmentDto)
    {
        var userId = GetUserId(); 
        var selectedTimeWithHour = MergeTime(appointmentDto);
        var fechaT7 = selectedTimeWithHour.AddDays(7);
        var fechaT14 = selectedTimeWithHour.AddDays(14);

        var existRecurrenceWeekly = await _context.Appointments
            .AnyAsync(a => a.Date == fechaT7
                           && a.IsCanceled == false
                           && a.HairdresserId == appointmentDto.SelectedHairdresserId);
        
        var existRecurrenceBiWeekly = await _context.Appointments
            .AnyAsync(a => a.Date == fechaT14
                           && a.IsCanceled == false
                           && a.HairdresserId == appointmentDto.SelectedHairdresserId);
        
        return new RecurrenceAvailabilityDto
        {
            WeeklyAvailable = !existRecurrenceWeekly,
            BiweeklyAvailable = !existRecurrenceBiWeekly
        };
    }
    
    public async Task<bool> InsertAppointmentAsync(AppointmentDto appointmentdto)
    {
        DateTime mergedTime = MergeTime(appointmentdto);

        var userId = GetUserId();
        
        if (userId == 0) return false;

        Guid? IdRecurrent = null; 

        if (appointmentdto.Weekly || appointmentdto.BiWeekly)   //Creacion de nuevo turno fijo
        {
            var recurrent = new RecurrentSchedule()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = mergedTime.DayOfWeek,
                HairdresserId = appointmentdto.SelectedHairdresserId,
                UserId = userId,
                IsActive = true,
                StartTime = mergedTime.TimeOfDay,
                IntervalWeeks = appointmentdto.Weekly ? 1 : 2 //Operador ternario
            };

            IdRecurrent = recurrent.Id;
            
            _context.Add(recurrent);

        }
        
        var appointment = CreateOrRecycleAppointmentAsync(mergedTime, userId, appointmentdto.SelectedHairdresserId
            , appointmentdto.SelectedHaircutId, IdRecurrent);
        if (!await appointment)  return false;      //No se pudo registrar
        
        DateTime datePlusDays = DateTime.Now;

        if (appointmentdto.BiWeekly)
        {
            datePlusDays = mergedTime.AddDays(14);
            
            var appointmentRecurrent = CreateOrRecycleAppointmentAsync(datePlusDays, userId,
                appointmentdto.SelectedHairdresserId, appointmentdto.SelectedHaircutId, IdRecurrent);
            if (!await appointmentRecurrent) return false;
        }
        
        if (appointmentdto.Weekly)
        {
            datePlusDays = mergedTime.AddDays(7);
            
            var appointmentRecurrent = CreateOrRecycleAppointmentAsync(datePlusDays, userId,
                appointmentdto.SelectedHairdresserId, appointmentdto.SelectedHaircutId, IdRecurrent);
            if (!await appointmentRecurrent) return false;

            datePlusDays = datePlusDays.AddDays(7);
            
            var appointmentRecurrentJustInCase = CreateOrRecycleAppointmentAsync(datePlusDays, userId,
                appointmentdto.SelectedHairdresserId, appointmentdto.SelectedHaircutId, IdRecurrent);
            if(!await appointmentRecurrentJustInCase) return false;
        }

        
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    


    //Private
    
    private async Task<bool> CreateOrRecycleAppointmentAsync(DateTime date , int userId , int hairdresserId , 
        int haircutId , Guid? recurrentScheduleId )
    {
        var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(a =>
            a.HairdresserId == hairdresserId && a.Date == date);
        

        if (existingAppointment == null)
        {
            var appointment = new Appointment() 
            {
                Date = date,
                UserId = userId,
                HairdresserId = hairdresserId,
                HaircutId = haircutId,
                IsCanceled = false,
                RecurrentSchedulesId =  recurrentScheduleId
            };
            _context.Appointments.Add(appointment);

        }

        else
        {
            if (!existingAppointment.IsCanceled) return false;

            existingAppointment.IsCanceled = false;
            existingAppointment.UserId = userId;
            existingAppointment.HaircutId = haircutId;
            existingAppointment.RecurrentSchedulesId = recurrentScheduleId;
        }


        return true;

    }
    
    private async Task<List<DateTime>> GetOccuppiedAppointmentsAsync(int hairdresserId, DateTime date)
    {
        var occupiedAppointments = await _context.Appointments
            .Where(a => a.Hairdresser.Id == hairdresserId && a.Date.Date == date.Date && a.IsCanceled == false)
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