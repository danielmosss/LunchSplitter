using a;
using LunchSplitter.Data;
using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LunchSplitter.Services;

public class TransactionService
{
    private IDbContextFactory<DatabaseContext> _dbContextFactory;

    public TransactionService(IDbContextFactory<DatabaseContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<int> AddTransaction(Transaction transaction, Dictionary<int, decimal> customShares, Group group)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            var transAction = transaction;
            context.Transactions.Add(transAction);
            context.SaveChanges();
            
            var totalShares = group.GroupUsers.Sum(x => x.Share);
            
            foreach (var share in customShares)
            {
                var username = context.Users.FirstOrDefault(u => u.Id == share.Key).Name;
                var transactionShare = new TransactionShare
                {
                    TransactionId = transaction.Id,
                    UserId = share.Key,
                    Amount = transaction.Amount * (share.Value / totalShares),
                    UserName = username
                };
                context.TransactionShares.Add(transactionShare);
            }
            await context.SaveChangesAsync();
            
            var sumOfShares = context.TransactionShares.Where(ts => ts.TransactionId == transaction.Id).Sum(ts => ts.Amount);
            if (sumOfShares != transaction.Amount)
            {
                var firstTransactionShare = context.TransactionShares.FirstOrDefault(ts => ts.TransactionId == transaction.Id);
                firstTransactionShare.Amount += (transaction.Amount - sumOfShares);
            }

            context.SaveChanges();
            return transaction.Id;
        }
    }

    public async Task<List<Transaction>> GetTransactions(int groupId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            var transactions = context.Transactions
                .Include(x => x.User)
                .Where(t => t.GroupId == groupId)
                .OrderByDescending(t => t.Date)
                .Take(25)
                .ToList();
            return transactions;
        }
    }
    
    public List<TransactionShare> GetTransactionShares(int transactionId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            var transactionShares = context.TransactionShares
                .Where(ts => ts.TransactionId == transactionId)
                .ToList();
            if (transactionShares.Count == 0)
            {
                return new List<TransactionShare>();
            }
            return transactionShares;
        }
    }
}