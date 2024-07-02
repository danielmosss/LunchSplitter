using a;
using LunchSplitter.Data;
using LunchSplitter.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LunchSplitter.Services;

public class GroupService
{
    private IDbContextFactory<DatabaseContext> _dbContextFactory;

    public GroupService(IDbContextFactory<DatabaseContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public List<Group> GetGroups()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var groups = context.Groups.Include(g => g.GroupUsers).ThenInclude(gu => gu.User).ToList();
        return groups;
    }

    public Group GetGroupById(int groupId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            var group = context.Groups.Include(g => g.GroupUsers).ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == groupId);
            return group;
        }
    }

    public async void DeleteGroup(int groupId)
    {
        Console.WriteLine("Deleting group with id: " + groupId);
        using (var context = _dbContextFactory.CreateDbContext())
        {
            Group group = context.Groups.Find(groupId);
            context.Groups.Remove(group);
            await context.SaveChangesAsync();
        }
    }

    public async void AddGroup(Group group, int userId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            AddUserToGroup(group, userId);
        }
    }

    public async void AddUserToGroup(Group group, int userId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            User user = context.Users.Find(userId);
            Group dbGroup = context.Groups.Find(group.Id);
            if (dbGroup.GroupUsers == null)
            {
                dbGroup.GroupUsers = new List<GroupUser>();
            }

            GroupUser groupUser = new GroupUser
            {
                Group = dbGroup,
                User = user
            };
            dbGroup.GroupUsers.Add(groupUser);
            await context.SaveChangesAsync();
        }
    }

    public void AddTransactionToGroup(int groupId, Transaction transaction, GroupSharepreset groupSharepreset)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            Group group = context.Groups.Find(groupId);
            if (group.Transactions == null)
            {
                group.Transactions = new List<Transaction>();
            }

            group.Transactions.Add(transaction);
            context.SaveChanges();

            ShareTransaction(groupSharepreset, transaction);
        }
    }

    private void ShareTransaction(GroupSharepreset groupSharepreset, Transaction transaction)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            GroupSharepreset dbGroupSharepreset = context.GroupSharepresets.Find(groupSharepreset.id);
            List<Sharepreset> sharepreset =
                context.Sharepresets.Where(sp => sp.GroupSharepresetId == dbGroupSharepreset.id).ToList();
            Transaction dbTransaction = context.Transactions.Find(transaction.Id);
            TransactionShare transactionShare =
                context.TransactionShares.FirstOrDefault(ts => ts.TransactionId == dbTransaction.Id);
            // check if there are no transaction shares for this transaction
            if (transactionShare == null)
            {
                var totalshare = sharepreset.Sum(sp => sp.share);
                foreach (GroupUser groupUser in dbGroupSharepreset.Group.GroupUsers)
                {
                    var userShare = sharepreset.Where(x => x.UserId == groupUser.User.Id).FirstOrDefault();
                    var Amount = dbTransaction.Amount * userShare.share / totalshare;
                    TransactionShare newTransactionShare = new TransactionShare
                    {
                        Transaction = dbTransaction,
                        UserId = groupUser.User.Id,
                        UserName = groupUser.User.Name,
                        Amount = Amount
                    };
                    context.TransactionShares.Add(newTransactionShare);
                }

                context.SaveChanges();
            }
        }
    }
    
    public List<GroupSharepreset> GetGroupSharePresets(int groupid)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            List<GroupSharepreset> groupSharepreset = context.GroupSharepresets.Where(x => x.GroupId == groupid).ToList();
            if (groupSharepreset == null)
            {
                return new List<GroupSharepreset>();
            }
            return groupSharepreset;
        }
    }

    public List<Sharepreset> GetSharePreset(int sharePresetId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            List<Sharepreset> sharepresets = context.Sharepresets.Where(sp => sp.GroupSharepresetId == sharePresetId).ToList();
            if (sharepresets == null)
            {
                return new List<Sharepreset>();
            }
            return sharepresets;
        }
    }

    public void CreatePreset(GroupSharepreset newPreset)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.GroupSharepresets.Add(newPreset);
            context.SaveChanges();
        }
    }
}