using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;
using Neptun.Services;
using Neptun.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Neptun.Services;

public class SubjectService(ApplicationDbContext context)
{
    public async Task<List<SubjectModel>> GetAllSubjectsAsync()
        => await context.Subjects.ToListAsync();

    public async Task<SubjectModel?> GetSubjectByIdAsync(Guid subjectId)
        => await context.Subjects.FindAsync(subjectId);

    public async Task<SubjectModel> CreateSubjectAsync(SubjectCreateDto dto)
    {
        var subject = new SubjectModel
        {
            Id = Guid.NewGuid(),
            Code = dto.Code,
            Name = dto.Name,
            Credits = dto.Credits,
            IsActive = true 
        };

        context.Subjects.Add(subject);
        await context.SaveChangesAsync();
        return subject;
    }

    public async Task<SubjectModel?> UpdateSubjectAsync(Guid subjectId, SubjectUpdateDto dto)
    {
        var subject = await context.Subjects.FindAsync(subjectId);
        if (subject == null) return null;

        subject.Name = dto.Name;
        subject.Credits = dto.Credits;

        await context.SaveChangesAsync();
        return subject;
    }

    public async Task<bool> SetSubjectStatusAsync(Guid subjectId, bool status)
    {
        var subject = await context.Subjects.FindAsync(subjectId);
        if (subject == null) return false;

        subject.IsActive = status;
        await context.SaveChangesAsync();
        return true;
    }
  
}