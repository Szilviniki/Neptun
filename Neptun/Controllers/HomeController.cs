using Microsoft.AspNetCore.Mvc;
using Neptun.Models;
using Neptun.Services;

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController(SubjectService subjectService) : ControllerBase
{
    private readonly SubjectService _subjectService = subjectService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _subjectService.GetAllSubjectsAsync());

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetById(Guid subjectId)
    {
        var subject = await _subjectService.GetSubjectByIdAsync(subjectId);
        return subject == null ? NotFound() : Ok(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SubjectModel subject)
    {
        var result = await _subjectService.CreateSubjectAsync(subject);
        return Ok(result);
    }

    [HttpPut("{subjectId}")]
    public async Task<IActionResult> Update(Guid subjectId, SubjectModel subject)
    {
        var result = await _subjectService.UpdateSubjectAsync(subjectId, subject);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("{subjectId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid subjectId)
    {
        var success = await _subjectService.SetSubjectStatusAsync(subjectId, false);
        return success ? Ok(new { message = "Subject inactivated" }) : NotFound();
    }

    [HttpPost("{subjectId}/reactivate")]
    public async Task<IActionResult> Reactivate(Guid subjectId)
    {
        var success = await _subjectService.SetSubjectStatusAsync(subjectId, true);
        return success ? Ok(new { message = "Subject reactivated" }) : NotFound();
    }
}