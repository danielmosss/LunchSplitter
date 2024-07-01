using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class GroupSharepreset
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int id { get; set; }
    
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
}