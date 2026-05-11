namespace Neptun.DTOs
{
    public class SubjectRegisterDto
    {
        public Guid StudentId { get; set; }
        public List<Guid> CourseIds { get; set; } = new();
    }
}
