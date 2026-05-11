using Neptun.Models;

namespace Neptun.DTOs
{
    public class CourseCreateDto
    {
    public string CourseCode { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty; 
        public string Semester { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public CourseType Type { get; set; } 
        public StudyMode Form { get; set; }
        public int Hours { get; set; }
        public bool IsWeeklyHours { get; set; } 
        public List<Guid> TeacherIds { get; set; } = new(); 
    
    }
}
