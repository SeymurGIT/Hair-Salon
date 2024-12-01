using HairSalon.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

public class AppointmentCleanupService : BackgroundService
{
    private readonly ILogger<AppointmentCleanupService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public AppointmentCleanupService(ILogger<AppointmentCleanupService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Clean up appointments older than two days in NewAppointments table
                    var twoDaysAgo = DateTime.UtcNow.AddDays(-2);
                    var appointmentsToDelete = dbContext.NewAppointments.Where(a => a.ApplyDate < twoDaysAgo).ToList();

                    dbContext.NewAppointments.RemoveRange(appointmentsToDelete);
                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Rezerv təmizləmə əməliyyatı baş tutdu: {DateTime.UtcNow}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Təmizləmə zamanı xəta baş verdi.");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
