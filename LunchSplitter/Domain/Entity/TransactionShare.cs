using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace a;

[Keyless]
public class TransactionShare
{
    public int TransactionId { get; set; }
    
    public int UserId { get; set; }
    
    public decimal Amount { get; set; }
    
    public User User { get; set; }
    public Transaction Transaction { get; set; }
}