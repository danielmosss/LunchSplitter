using a;
using Azure.Core;
using LunchSplitter.Components.Pages;
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
    
    public List<Group> GetGroupsByUserId(int userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var groups = context.Groups
            .Include(g => g.GroupUsers)
            .ThenInclude(gu => gu.User)
            .Where(g => g.GroupUsers.Any(gu => gu.UserId == userId))
            .ToList();
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

            AddUserToGroup(group, userId, true);
        }
    }
    
    public async Task<Boolean> AddUserToGroup(Group group, int userId, bool isAdmin = false)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            User user = context.Users.Find(userId);
            Group dbGroup = context.Groups.Find(group.Id);
            if (dbGroup.GroupUsers == null)
            {
                dbGroup.GroupUsers = new List<GroupUser>();
            }
            
            if (UserAlreadyInGroup(dbGroup.Id, userId))
            {
                Console.WriteLine("User already in group");
                return false;
            }

            GroupUser groupUser = new GroupUser
            {
                Group = dbGroup,
                User = user,
                Share = 1
            };
            if (isAdmin)
            {
                groupUser.IsAdmin = true;
            }
            dbGroup.GroupUsers.Add(groupUser);
            await context.SaveChangesAsync();
            return true;
        }
    }
    
    private bool UserAlreadyInGroup(int groupId, int userId)
    {
        Group group = GetGroupById(groupId);
        return group.GroupUsers.Any(gu => gu.UserId == userId);
    }
    
    public class UserTotalAmount
    {
        public GroupUser GroupUser { get; set; }
        public int TransactionsPaid { get; set; }
        public decimal TotalPaid { get; set; }
        public int TransactionsSpent { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public List<UserTotalAmount> GetUserTotalAmounts(int groupId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            List<GroupUser> groupUsers = context.GroupUsers
                .Include(gu => gu.User)
                .Where(gu => gu.GroupId == groupId)
                .ToList();
            List<TransactionShare> transactionShares = context.TransactionShares
                .Where(ts => ts.Transaction.GroupId == groupId)
                .ToList();
            List<Transaction> transactions = context.Transactions
                .Where(t => t.GroupId == groupId)
                .ToList();

            var userTotalAmounts = new List<UserTotalAmount>();
            foreach (var groupUser in groupUsers)
            {
                var totalPaid = transactions.Where(t => t.UserId == groupUser.UserId);
                var totalSpent = transactionShares.Where(ts => ts.UserId == groupUser.UserId);
                userTotalAmounts.Add(new UserTotalAmount
                {
                    GroupUser = groupUser,
                    TransactionsPaid = totalPaid.Count(),
                    TotalPaid = totalPaid.Sum(t => t.Amount),
                    TransactionsSpent = totalSpent.Count(),
                    TotalSpent = totalSpent.Sum(ts => ts.Amount)
                });
            }

            return userTotalAmounts;
        }
    }

    public void CreateInvite(GroupInvite invite)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.GroupInvites.Add(invite);
            context.SaveChanges();
        }
    }
    
    public List<GroupInvite> GetInvites(int groupId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return context.GroupInvites
                .Where(gi => gi.GroupId == groupId)
                .ToList();
        }
    }

    public GroupInvite GetInvite(Guid inviteId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return context.GroupInvites.Find(inviteId);
        }
    }
    
    public async void UseInvite(Guid inviteId, int userId)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            GroupInvite invite = context.GroupInvites.Find(inviteId);
            if (invite == null)
            {
                return;
            }
            
            if (invite.usage < 2)
            {
                context.GroupInvites.Remove(invite);
            }
            
            var group = context.Groups.Find(invite.GroupId);
            var result = await AddUserToGroup(group, userId);
            if (result)
            {
                invite.usage--;
                context.SaveChanges();
            }
        }
    }
}