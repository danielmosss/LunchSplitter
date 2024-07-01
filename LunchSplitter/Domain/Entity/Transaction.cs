using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
    
    [Column("image")]
    public string Image { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}