using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class GroupUser
{
    [Key]
    [Column("group_user_id")]
    public int GroupUserId { get; set; }
    
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("is_admin")]
    public bool IsAdmin { get; set; }
    
    [Column("share")]
    public int Share { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}