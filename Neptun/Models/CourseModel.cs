using Neptun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum CourseType { Theory, Practice, Lab }
[Table("courses")]
public class CourseModel
{
    [Key]
    [Column("course_id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("course_code")]
    public string CourseCode { get; set; } = string.Empty;

    [Column("subject_guid")]
    public Guid SubjectId { get; set; }
    public SubjectModel? Subject { get; set; }

    [Column("semester")]
    public string Semester { get; set; } = string.Empty;

    [Column("max_students")]
    public int MaxStudents { get; set; }

    [Column("course_type")]
    public CourseType Type { get; set; }

    [Column("study_form")]
    public StudyMode Form { get; set; }

    [Column("hours")]
    public int Hours { get; set; }

    public List<UserModel> Teachers { get; set; } = new();
    public List<UserModel> Students { get; set; } = new();
}