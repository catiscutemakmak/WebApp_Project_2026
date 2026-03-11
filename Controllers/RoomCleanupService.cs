using hateekub.Models;
using hateekub.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using hateekub.Data;

public class RoomCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RoomCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {   
        Console.WriteLine("Room cleanup check...");
        while (!stoppingToken.IsCancellationRequested)
        {   
            Console.WriteLine("Room cleanup running...");
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.UtcNow;
            

            var rooms = await db.Rooms
                .Where(r => r.Status == RoomStatus.Waiting && r.PlayDateTime <= now)
                .ToListAsync();

            foreach (var room in rooms)
            {
                room.Status = RoomStatus.Starting;
            }

            await db.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}