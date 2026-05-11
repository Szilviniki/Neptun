using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neptun.Models;

[Table("notification_log")]
public class NotificationLogModel
{
    [Key]
    [Column("notification_log_id")]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Required]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Required]
    [Column("generated_at")]
    public DateTime GeneratedAt { get; set; }
}