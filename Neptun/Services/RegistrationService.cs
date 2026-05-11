using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.DTOs;
using Neptun.Models;

public class RegistrationService(ApplicationDbContext context)
{
    public async Task<(bool Success, string Message)> RegisterToSubjectAsync(Guid subjectId, SubjectRegisterDto dto)
    {
        var student = await context.Users.FindAsync(dto.StudentId);
        if (student == null || student.Type != UserType.Student || !student.IsActive)
            return (false, "A hallgató nem található vagy inaktív.");

        var courses = await context.Courses
            .Include(c => c.Students)
            .Where(c => dto.CourseIds.Contains(c.Id) && c.SubjectId == subjectId)
            .ToListAsync();

        if (courses.Count != dto.CourseIds.Count)
            return (false, "Néhány megadott kurzus nem ehhez a tárgyhoz tartozik.");

        var semester = courses.First().Semester;
        if (courses.Any(c => c.Semester != semester))
            return (false, "Minden kurzusnak ugyanahhoz a félévhez kell tartoznia.");

        foreach (var course in courses)
        {
            if (course.Form != StudyMode.None && (int)course.Form != (int)student.Mode)
                return (false, $"A(z) {course.CourseCode} kurzus tagozata nem egyezik a hallgatóéval.");

            if (course.Students.Count >= course.MaxStudents)
                return (false, $"A(z) {course.CourseCode} kurzus betelt.");
        }

        var availableTypes = await context.Courses
            .Where(c => c.SubjectId == subjectId && c.Semester == semester &&
                       (c.Form == StudyMode.None || (int)c.Form == (int)student.Mode))
            .Select(c => c.Type)
            .Distinct()
            .ToListAsync();

        var selectedTypes = courses.Select(c => c.Type).Distinct().ToList();

        if (availableTypes.Count != selectedTypes.Count || courses.Count != selectedTypes.Count)
            return (false, "Minden kötelező kurzustípusból pontosan egyet fel kell venni.");

        foreach (var course in courses)
        {
            course.Students.Add(student);
        }

        await context.SaveChangesAsync();
        return (true, "Sikeres tárgyfelvétel.");
    }

    public async Task<bool> UnregisterFromSubjectAsync(Guid subjectId, SubjectUnregisterDto dto)
    {
        var studentCourses = await context.Courses
            .Include(c => c.Students)
            .Where(c => c.SubjectId == subjectId &&
                       c.Semester == dto.Semester &&
                       c.Students.Any(s => s.Id == dto.StudentId))
            .ToListAsync();

        if (studentCourses.Count == 0) return false;

        var student = await context.Users.FindAsync(dto.StudentId);
        if (student == null) return false;

        foreach (var course in studentCourses)
        {
            course.Students.Remove(student);
        }

        await context.SaveChangesAsync();
        return true;
    }
    public async Task<(bool Success, string Message)> ChangeCourseAsync(CourseChangeDto dto)
    {
        var fromCourse = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == dto.FromCourseId);
        var toCourse = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == dto.ToCourseId);
        var student = await context.Users.FindAsync(dto.StudentId);

        if (fromCourse == null || toCourse == null || student == null)
            return (false, "A megadott kurzusok vagy a hallgató nem található.");

        if (fromCourse.SubjectId != toCourse.SubjectId)
            return (false, "Csak ugyanazon tantárgy kurzusai között lehet átjelentkezni.");

        if (fromCourse.Type != toCourse.Type)
            return (false, "Csak azonos típusú (pl. gyakorlatról gyakorlatra) kurzusra lehet átjelentkezni.");

        if (!fromCourse.Students.Any(s => s.Id == dto.StudentId))
            return (false, "A hallgató nincs feliratkozva a forráskurzusra.");

        if (toCourse.Form != StudyMode.None && (int)toCourse.Form != (int)student.Mode)
            return (false, "Az új kurzus munkarendje nem felel meg a hallgató tagozatának.");

        if (toCourse.Students.Count >= toCourse.MaxStudents)
            return (false, "A célkurzus betelt.");

        fromCourse.Students.Remove(student);
        toCourse.Students.Add(student);

        await context.SaveChangesAsync();
        return (true, "Sikeres átjelentkezés.");
    }
}