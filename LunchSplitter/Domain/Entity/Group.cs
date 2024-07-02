using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchSplitter.Domain.Entity;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "length can't be more than 100.")]
    [Column("name")]
    public string Name { get; set; }
    
    public List<Transaction> Transactions { get; set; }
    
    public List<GroupUser> GroupUsers { get; set; }
}