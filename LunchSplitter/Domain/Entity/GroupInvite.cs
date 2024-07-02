using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class GroupInvite
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "must be greater than zero")]
    [Column("usage")]
    public int usage { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
}