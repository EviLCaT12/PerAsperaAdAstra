using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DataBase;
using DataBase.Dbo;

namespace Hermes.Services;

public class OutboxService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxService> _logger;

    public OutboxService(IServiceProvider serviceProvider, ILogger<OutboxService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckOutbox(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task CheckOutbox(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var newOutboxes = await context.Outboxes
                                           .Where(o => o.Status == 0)
                                           .ToListAsync(stoppingToken);

            foreach (var outbox in newOutboxes)
            {
                if (stoppingToken.IsCancellationRequested)
                    return;
                try
                {
                    ProcessOutbox(outbox);
                    outbox.Status = 1;
                    context.Outboxes.Delete(outbox);
                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox record with Id: {Id}", outbox.Id);
                }
            }
        }
    }

    private void ProcessOutbox(OutboxDbo outbox)
    {
        
        Console.WriteLine($"Processing outbox record: {outbox.Payload}");
    }
}
