using LunchSplitter.Data;
using LunchSplitter.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
namespace LunchSplitter.Controllers;

public class GroupController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public GroupController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    // get groups
    public List<Group> GetGroups()
    {
        return _databaseContext.Groups.ToList();
    }
}