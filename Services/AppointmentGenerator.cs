using Barber.Data;

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
                var context = scoped.ServiceProvider.GetRequiredKeyedService<ApplicationDbContext>();
            }
            
            
            
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

}