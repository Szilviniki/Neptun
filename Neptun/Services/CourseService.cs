using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;

namespace Neptun.Services;

public class CourseService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<CourseModel?> GetCourseByIdAsync(Guid courseId)
    {
        return await _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<CourseModel?> CreateCourseAsync(CourseModel course, List<Guid> teacherIds)
    {
        var subject = await _context.Subjects.FindAsync(course.SubjectId);
        if (subject == null || !subject.IsActive) return null;

        var teachers = await _context.Users
            .Where(u => teacherIds.Contains(u.Id) && u.Type == UserType.Teacher)
            .ToListAsync();

        course.Id = Guid.NewGuid();
        course.Teachers = teachers;

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<CourseModel?> UpdateCourseAsync(Guid courseId, CourseModel updatedData)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null) return null;

        course.CourseCode = updatedData.CourseCode;
        course.Semester = updatedData.Semester;
        course.MaxStudents = updatedData.MaxStudents;
        course.Type = updatedData.Type;
        course.Form = updatedData.Form;
        course.Hours = updatedData.Hours;

        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<string?> DeleteCourseAsync(Guid courseId)
    {

        var course = await _context.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return "Kurzus nem található.";

        if (course.Students.Any())
            return "A kurzus nem törölhető, mert vannak rajta hallgatók!";

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return null;
    }
}