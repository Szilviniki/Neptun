namespace Neptun.DTOs
{
    public class CourseUpdateDto
    {
        public string Semester { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public int Hours { get; set; }
    }
}
