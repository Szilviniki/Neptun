using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;
using Neptun.DTOs;
using Neptun.Migrations;

namespace Neptun.Services;

public class CourseService(ApplicationDbContext context)
{

    public async Task<CourseModel?> GetCourseByIdAsync(Guid courseId)
    {
        return await context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Teachers)
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<List<CourseModel>> GetAllCoursesAsync()
    {
        return await context.Courses
            .Include(c => c.Subject)
            .ToListAsync();
    }

    public async Task<CourseModel?> CreateCourseAsync(CourseCreateDto dto)
    {
        var subject = await context.Subjects
            .FirstOrDefaultAsync(s => s.Code == dto.SubjectCode);

        if (subject == null || !subject.IsActive)
            return null;

        var teachers = await context.Users
            .Where(u => dto.TeacherIds.Contains(u.Id) && u.Type == UserType.Teacher)
            .ToListAsync();

        if (teachers.Count != dto.TeacherIds.Count)
            return null;

        var course = new CourseModel
        {
            Id = Guid.NewGuid(),
            CourseCode = dto.CourseCode,
            SubjectId = subject.Id,
            Semester = dto.Semester,
            MaxStudents = dto.MaxStudents,
            Type = dto.Type,
            Form = dto.Form,
            Hours = dto.Hours,
            Teachers = teachers
        };

        context.Courses.Add(course);
        await context.SaveChangesAsync();
        return course;
    }

    public async Task<CourseModel?> UpdateCourseAsync(Guid courseId, CourseUpdateDto dto)
    {
        var course = await context.Courses.FindAsync(courseId);
        if (course == null) return null;

        course.Semester = dto.Semester;
        course.MaxStudents = dto.MaxStudents;
        course.Hours = dto.Hours;

        await context.SaveChangesAsync();
        return course;
    }

    public async Task<bool> DeleteCourseAsync(Guid courseId)
    {
        var course = await context.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return false;

        if (course.Students.Count > 0)
            return false;

        context.Courses.Remove(course);
        await context.SaveChangesAsync();
        return true;
    }
}