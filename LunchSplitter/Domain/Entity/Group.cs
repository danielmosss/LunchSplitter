using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    public List<Transaction> Transactions { get; set; }
    
    public List<GroupUser> GroupUsers { get; set; }
}