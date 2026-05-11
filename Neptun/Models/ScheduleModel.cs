using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neptun.Models;

[Table("schedule")]
public class ScheduleModel
{
    [Key]
    [Column("schedule_id")]
    public Guid Id { get; set; }

    [Required]
    [Column("course_id")]
    public Guid CourseId { get; set; }

    public CourseModel? Course { get; set; }

    [Required]
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Column("end_time")]
    public DateTime EndTime { get; set; }

    [Required]
    [Column("room")]
    public string Room { get; set; } = string.Empty;
}