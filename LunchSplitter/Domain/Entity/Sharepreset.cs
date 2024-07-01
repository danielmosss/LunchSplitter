using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LunchSplitter.Domain.Entity;
[Keyless]
public class Sharepreset
{
    [Column("group_sharepreset_id")]
    public int GroupSharepresetId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("share")]
    public int share { get; set; }
    
    [ForeignKey("GroupSharepresetId")]
    public GroupSharepreset GroupSharepreset { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}