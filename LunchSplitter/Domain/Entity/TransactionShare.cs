using System.ComponentModel.DataAnnotations.Schema;
using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace a;

[Keyless]
public class TransactionShare
{
    public int TransactionId { get; set; }
    
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    
    public decimal Amount { get; set; }
    
    [ForeignKey("TransactionId")]
    public Transaction Transaction { get; set; }
}