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
            var group = context.Groups.Include(g => g.GroupUsers).ThenInclude(gu => gu.User).FirstOrDefault(g => g.Id == groupId);
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
}