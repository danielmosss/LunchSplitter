using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LunchSplitter.Domain.Entity;

public class GroupUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("group_user_id")]
    public int GroupUserId { get; set; }
    
    [Required]
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; }
    
    [Required]
    [Column("share")]
    public int Share { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    
    
}