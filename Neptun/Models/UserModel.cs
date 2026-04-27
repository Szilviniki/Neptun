using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neptun.Models;

public enum UserType { Student, Teacher, Admin }
public enum StudyMode { FullTime, PartTime, None }

[Table("users")]
public class UserModel
{
    [Key]
    [Column("user_id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty; 

    [Column("user_type")]
    public UserType Type { get; set; }

    [Column("study_mode")]
    public StudyMode Mode { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}