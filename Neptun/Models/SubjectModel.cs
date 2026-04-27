using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("subjects")]
public class SubjectModel
{
    [Key]
    [Column("subject_id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("subject_code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Column("subject_name")]
    public string Name { get; set; } = string.Empty;

    [Column("credit_value")]
    public int Credits { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}