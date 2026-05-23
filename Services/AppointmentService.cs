using Barber.Data;
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

    [HttpGet]
    public async Task<List<string>> GetAvailableSlots(int hairdresserId, string date)
    {
        byte timeOfCutting = 40;
        byte hourStart = 0;
        byte hourEnd = 0;
        
        DateTime pointer;
        DateTime lastHour;
        var selectedDate = DateTime.Parse(date);
        var dayOfWeek = selectedDate.DayOfWeek;
        
        var availableSlots = new List<string>();
        
        var occupiedAppointments = await _context.Appointments
            .Where(a => a.Hairdresser.Id == hairdresserId && a.Date == selectedDate.Date)
            .Select(a => a.Date)            //Da solo los dias que no estan ocupados
            .ToListAsync();


        if (dayOfWeek == DayOfWeek.Saturday)
        {
            hourStart = 10;
            hourEnd = 14;
        }
        else if (dayOfWeek == DayOfWeek.Sunday)
        {
            return availableSlots;
        }
        else
        {
            hourStart = 10;
            hourEnd = 18;
        }

        pointer = selectedDate.Date.AddHours(hourStart);
        lastHour = selectedDate.Date.AddHours(hourEnd);

        

        while (pointer.AddMinutes(timeOfCutting) <= lastHour)
        {

            foreach (var appointment in occupiedAppointments)
            {
                if (appointment.Date != pointer)        //No esta ocupado el turno
                    availableSlots.Add(pointer.ToString("HH:mm"));
            }
            
            pointer = pointer.AddMinutes(timeOfCutting);
        }

        return availableSlots;

    }
   
}