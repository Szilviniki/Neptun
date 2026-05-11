using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptun.Data;

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController(ApplicationDbContext context) : ControllerBase
{
    ///  <summary>
    /// Minden generált értesítés listázása szűrési lehetőséggel.
    /// </summary>
    /// <param name="userId">Szűrés adott felhasználóra (hallgató/oktató)</param>
    /// <param name="courseId">Szűrés adott kurzusra</param>
    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] Guid? userId, [FromQuery] Guid? courseId)
    {
        var query = context.NotificationLogs.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(n => n.UserId == userId.Value);
        }

        if (courseId.HasValue)
        {
            query = query.Where(n => n.CourseId == courseId.Value);
        }

        var results = await query
            .OrderByDescending(n => n.GeneratedAt)
            .ToListAsync();

        return Ok(results);
    }
}