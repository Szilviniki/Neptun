namespace Neptun.DTOs
{
    public class CourseChangeDto
    {
        public Guid StudentId { get; set; }
        public Guid FromCourseId { get; set; }
        public Guid ToCourseId { get; set; }
    }
}
