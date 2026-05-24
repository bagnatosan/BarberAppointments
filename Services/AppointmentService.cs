using Barber.Data;
using Barber.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Services;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _context;

    public AppointmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hairdresser>> GetHairdressers()
    {
        var hairdressers = await _context.Hairdressers.ToListAsync();

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

    private async Task<List<DateTime>> GetOccuppiedAppointmentsAsync(int hairdresserId, DateTime date)
    {
        var occupiedAppointments = await _context.Appointments
            .Where(a => a.Hairdresser.Id == hairdresserId && a.Date == date.Date)
            .Select(a => a.Date)            //Da solo los dias que no estan ocupados
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
    
}