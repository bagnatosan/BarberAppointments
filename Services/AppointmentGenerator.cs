using Barber.Data;
using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Services;

public class AppointmentGenerator : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AppointmentGenerator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scoped =  _serviceScopeFactory.CreateScope() )
            {
                var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var activeRecurrentDates = await context.RecurrentSchedules
                    .Where(r => r.IsActive)
                    .ToListAsync();

                foreach (var i in activeRecurrentDates)
                {
                    var currentDate = DateTime.Now.Date;
                    var endDate = currentDate.AddDays(30);
                    
                    var lastAppointment = await context.Appointments
                        .AsNoTracking()
                        .Where(a => a.RecurrentSchedulesId == i.Id)
                        .OrderByDescending(a => a.Date)
                        .FirstOrDefaultAsync();

                    var appointments = await context.Appointments
                        .Where(a => a.Date.Date > DateTime.Now.Date)
                        .ToListAsync();

                    if (lastAppointment != null)
                    {
                        bool wasCreated = false;
                        
                        while (currentDate != endDate && !wasCreated)    //reseteo de horas
                        {
                            var timeInterval = lastAppointment.Date - currentDate.Date;
                            var differenceBetweenDays = timeInterval.Days;
                            wasCreated = false;

                            
                            var differenceBetweenWeeks = differenceBetweenDays / 7;
                            if (differenceBetweenWeeks % i.IntervalWeeks == 0)
                            {
                                foreach (var a in appointments)
                                {
                                    if (currentDate.Add(i.StartTime) == a.Date) //todo esto esta mal
                                    {
                                        if (a.IsCanceled)
                                        {
                                            a.IsCanceled = false;
                                            a.UserId = i.UserId;
                                            //agregar haircutid
                                            a.HairdresserId = i.HairdresserId;
                                            a.RecurrentSchedulesId = i.Id;
                                        }
                                        else 
                                        {
                                            var appointment = new Appointment()
                                            {
                                                Date = currentDate.Add(i.StartTime),
                                                HaircutId = lastAppointment.HaircutId,
                                                HairdresserId = i.HairdresserId,
                                                IsCanceled = false,
                                                RecurrentSchedulesId = i.Id,
                                                UserId = i.UserId
                                            };
                                
                                            context.Add(appointment);
                                        }
                                        
                                                            
                                    }
                                }
                                
                                
                                wasCreated = true;
                            }
                            currentDate = currentDate.AddDays(1);
                        }
                    }

                }
                await context.SaveChangesAsync();


            }
            
            
            
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

}