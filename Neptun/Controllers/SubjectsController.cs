using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptun.DTOs;
using Neptun.Services;
using Neptun.Data; 

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SubjectsController(
    SubjectService subjectService,
    RegistrationService registrationService,
    ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await subjectService.GetAllSubjectsAsync());

    [HttpPost]
    public async Task<IActionResult> Create(SubjectCreateDto dto)
        => Ok(await subjectService.CreateSubjectAsync(dto));

    [HttpPut("{subjectId}")]
    public async Task<IActionResult> Update(Guid subjectId, SubjectUpdateDto dto)
    {
        var result = await subjectService.UpdateSubjectAsync(subjectId, dto);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("{subjectId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid subjectId)
        => await subjectService.SetSubjectStatusAsync(subjectId, false) ? Ok() : NotFound();

    [HttpPost("{subjectId}/reactivate")]
    public async Task<IActionResult> Reactivate(Guid subjectId)
        => await subjectService.SetSubjectStatusAsync(subjectId, true) ? Ok() : NotFound();

    [HttpPost("{subjectId}/register")]
    public async Task<IActionResult> Register(Guid subjectId, SubjectRegisterDto dto)
    {
        var result = await registrationService.RegisterToSubjectAsync(subjectId, dto);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }
    [HttpPost("{subjectId}/unregister")]
    public async Task<IActionResult> Unregister(Guid subjectId, SubjectUnregisterDto dto)
    {
        var success = await registrationService.UnregisterFromSubjectAsync(subjectId, dto);

        if (!success)
        {
            return BadRequest("A hallgató nincs feliratkozva erre a tárgyra az adott félévben.");
        }

        return Ok(new { message = "Sikeres lejelentkezés a tárgyról." });
    }

    [HttpGet("{subjectId}/students")]
    public async Task<IActionResult> GetSubjectStudents(Guid subjectId, [FromQuery] string semester)
    {
        var students = await context.Courses
            .Where(c => c.SubjectId == subjectId && c.Semester == semester)
            .SelectMany(c => c.Students)
            .Distinct()
            .ToListAsync();

        return Ok(students);
    }

    
}