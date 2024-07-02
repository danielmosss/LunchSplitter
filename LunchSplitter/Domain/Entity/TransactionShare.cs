using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace a;

public class TransactionShare
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("transaction_id")]
    public int TransactionId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("user_name")]
    public string UserName { get; set; }
    
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [ForeignKey("TransactionId")]
    public Transaction Transaction { get; set; }
}