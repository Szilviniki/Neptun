using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;

namespace Neptun.Services;

public class SubjectService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<SubjectModel>> GetAllSubjectsAsync()
        => await _context.Users.IgnoreQueryFilters().Cast<SubjectModel>().ToListAsync(); 

    public async Task<SubjectModel?> GetSubjectByIdAsync(Guid subjectId)
        => await _context.Subjects.FindAsync(subjectId);

    public async Task<SubjectModel> CreateSubjectAsync(SubjectModel subject)
    {
        subject.Id = Guid.NewGuid();
        subject.IsActive = true;     

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<SubjectModel?> UpdateSubjectAsync(Guid subjectId, SubjectModel updatedData)
    {
        var subject = await _context.Subjects.FindAsync(subjectId);
        if (subject == null) return null;

        subject.Code = updatedData.Code;
        subject.Name = updatedData.Name;
        subject.Credits = updatedData.Credits;

        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<bool> SetSubjectStatusAsync(Guid subjectId, bool status)
    {
        var subject = await _context.Subjects.FindAsync(subjectId);
        if (subject == null) return false;

        subject.IsActive = status;
        await _context.SaveChangesAsync();
        return true;
    }
}