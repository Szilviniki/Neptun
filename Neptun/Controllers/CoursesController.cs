using Microsoft.AspNetCore.Mvc;
using Neptun.Models;
using Neptun.Services;

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(CourseService courseService) : ControllerBase
{
    private readonly CourseService _courseService = courseService;

    [HttpGet("{courseId}")]
    public async Task<IActionResult> Get(Guid courseId)
    {
        var course = await _courseService.GetCourseByIdAsync(courseId);
        return course == null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCreateRequest request)
    {
        var course = new CourseModel
        {
            CourseCode = request.CourseCode,
            SubjectId = request.SubjectId,
            Semester = request.Semester,
            MaxStudents = request.MaxStudents,
            Type = request.Type,
            Form = request.Form,
            Hours = request.Hours
        };

        var result = await _courseService.CreateCourseAsync(course, request.TeacherIds);

        if (result == null)
            return BadRequest("Érvénytelen tantárgy (vagy nem aktív)!");

        return Ok(result);
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        var error = await _courseService.DeleteCourseAsync(courseId);
        if (error != null) return BadRequest(error);
        return Ok(new { message = "Kurzus sikeresen törölve." });
    }
}

public class CourseCreateRequest
{
    public string CourseCode { get; set; } = string.Empty;
    public Guid SubjectId { get; set; }
    public string Semester { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public CourseType Type { get; set; }
    public StudyMode Form { get; set; }
    public int Hours { get; set; }
    public List<Guid> TeacherIds { get; set; } = new();
}