using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.DTOs;
using Neptun.Models;
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.DependencyInjection;

namespace Neptun.Services
{
    public class ScheduleService(ApplicationDbContext context)
    {
        public async Task AddScheduleAsync(Guid courseId, CreateScheduleDto dto)
        {
            var schedules = new List<ScheduleModel>();

            foreach (var item in dto.Items)
            {
                int occurrences = item.IsWeekly ? 14 : 1;

                for (int i = 0; i < occurrences; i++)
                {
                    schedules.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        CourseId = courseId,
                        StartTime = item.StartTime.AddDays(i * 7),
                        EndTime = item.EndTime.AddDays(i * 7),
                        Room = item.Room
                    });
                }
            }

            context.Schedules.AddRange(schedules);
            await context.SaveChangesAsync();
        }
    }
}