using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;

namespace Neptun.Services
{
    public class NotificationBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var targetTime = DateTime.Now.AddMinutes(30);
                    var lowerBound = targetTime.AddSeconds(-30);
                    var upperBound = targetTime.AddSeconds(30);

                    var upcomingSchedules = await context.Schedules
                        .Include(s => s.Course)
                            .ThenInclude(c => c!.Students)
                        .Include(s => s.Course)
                            .ThenInclude(c => c!.Teachers)
                        .Where(s => s.StartTime >= lowerBound && s.StartTime <= upperBound)
                        .ToListAsync(stoppingToken);

                    foreach (var schedule in upcomingSchedules)
                    {
                        if (schedule.Course == null) continue;


                        var usersToNotify = schedule.Course.Students
                            .Concat(schedule.Course.Teachers)
                            .DistinctBy(u => u.Id);

                        foreach (var user in usersToNotify)
                        {
                            context.NotificationLogs.Add(new NotificationLogModel
                            {
                                Id = Guid.NewGuid(),
                                UserId = user.Id,
                                CourseId = schedule.CourseId,
                                Message = $"Értesítés: A(z) {schedule.Course.CourseCode} kurzusod 30 perc múlva kezdődik a(z) {schedule.Room} teremben.",
                                GeneratedAt = DateTime.Now
                            });
                        }
                    }
                    await context.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
