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
        return context.Groups.ToList();
    }

    public void AddGroup(Group group)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Groups.Add(group);
            context.SaveChanges();
        }
    }
}