using Barber.Data;
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

                var currentDate = DateTime.Now;
                var endDate = currentDate.AddDays(30);

                var activeRecurrentDates = await context.RecurrentSchedules
                    .Where(r => r.IsActive)
                    .ToListAsync();

                foreach (var i in activeRecurrentDates)
                {
                        
                }


            }
            
            
            
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

}