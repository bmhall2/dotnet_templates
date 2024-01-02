using WebApi_Postgres_Docker_GraphQL.Domain;
using WebApi_Postgres_Docker_GraphQL.Services;

namespace WebApi_Postgres_Docker_GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class UserQueries : Query
{
    private readonly UserService _userService;

    public UserQueries(UserService userService)
    {
        _userService = userService;
    }

    public List<User> GetUsers()
    {
        return _userService.GetAsync().Result;
    }

    public User? GetUser(Guid id)
    {
        return _userService.GetAsync(id).Result;
    }
}