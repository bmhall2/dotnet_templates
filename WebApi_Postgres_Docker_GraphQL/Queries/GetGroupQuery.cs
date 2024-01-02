using WebApi_Postgres_Docker_GraphQL.Domain;
using WebApi_Postgres_Docker_GraphQL.Services;

namespace WebApi_Postgres_Docker_GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class GroupQueries : Query
{
    private readonly GroupService _groupService;

    public GroupQueries(GroupService groupService)
    {
        _groupService = groupService;
    }

    public List<Group> GetGroups()
    {
        return _groupService.GetAsync().Result;
    }

    public Group? GetGroup(Guid id)
    {
        return _groupService.GetAsync(id).Result;
    }
}