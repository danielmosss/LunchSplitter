using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "must be greater than zero")]
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "length can't be more than 100.")]
    [Column("description")]
    public string Description { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}