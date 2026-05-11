using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptun.DTOs;
using Neptun.Models;
using Neptun.Services;
using Neptun.Data; 

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(
    CourseService courseService,
    RegistrationService registrationService,
    ScheduleService scheduleService,
    ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        var result = await courseService.CreateCourseAsync(dto);

        if (result == null)
        {
            return BadRequest("A kurzus nem hozható létre. Ellenőrizd: a tantárgy aktív-e, és az oktatók léteznek-e.");
        }

        return CreatedAtAction(nameof(GetById), new { courseId = result.Id }, result);
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<CourseModel>> GetById(Guid courseId)
    {
        var course = await courseService.GetCourseByIdAsync(courseId);
        return course == null ? NotFound() : Ok(course);
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> Update(Guid courseId, CourseUpdateDto dto)
    {
        var result = await courseService.UpdateCourseAsync(courseId, dto);

        if (result == null)
        {
            return NotFound("A kurzus nem található.");
        }

        return Ok(result);
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        var success = await courseService.DeleteCourseAsync(courseId);

        if (!success)
        {
            return BadRequest("A kurzus nem törölhető. Vagy nem létezik, vagy már vannak rajta feliratkozott hallgatók.");
        }

        return NoContent();
    }

    [HttpGet("{courseId}/students")]
    public async Task<IActionResult> GetCourseStudents(Guid courseId)
    {
        var course = await context.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return NotFound("A kurzus nem található.");

        return Ok(course.Students);
    }

    [HttpPost("change")]
    public async Task<IActionResult> ChangeCourse(CourseChangeDto dto)
    {
        var result = await registrationService.ChangeCourseAsync(dto);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(new { message = result.Message });
    }
    /// <summary>
    /// Új órarendi időpontok rögzítése egy kurzushoz.
    /// </summary>
    /// <param name="courseId">A kurzus azonosítója</param>
    /// <param name="dto">Az időpontok listája. IsWeekly=true esetén 14 hét generálódik.</param>
    /// <remarks>
    /// Példa kérés:
    /// POST /api/courses/{guid}/schedule
    /// {
    ///   "items": [
    ///     {
    ///       "startTime": "2026-09-15T08:00:00",
    ///       "endTime": "2026-09-15T09:30:00",
    ///       "room": "I. Előadó",
    ///       "isWeekly": true
    ///     }
    ///   ]
    /// }
    /// </remarks>
    [HttpPost("{courseId}/schedule")]
    public async Task<IActionResult> AddSchedule(Guid courseId, CreateScheduleDto dto)
    {
        await scheduleService.AddScheduleAsync(courseId, dto);
        return Ok(new { message = "Órarendi időpontok sikeresen rögzítve." });
    }

    /// <summary>
    /// Órarendi időpontok módosítása. 
    /// Figyelem: Ez a művelet törli a korábbi időpontokat és újakat hoz létre!
    /// </summary>
    [HttpPost("{courseId}/schedule/modify")]
    public async Task<IActionResult> ModifySchedule(Guid courseId, CreateScheduleDto dto)
    {
        
        var oldSchedules = context.Schedules.Where(s => s.CourseId == courseId);
        context.Schedules.RemoveRange(oldSchedules);

    
        await scheduleService.AddScheduleAsync(courseId, dto);

        return Ok(new { message = "Órarend sikeresen módosítva." });
    }
}