using System.ComponentModel.DataAnnotations.Schema;
using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace a;

[Keyless]
public class GroupUserPermissions
{
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("permission_id")]
    public int PermissionId { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
    
    [ForeignKey("PermissionId")]
    public Permission Permission { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}