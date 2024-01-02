using WebApi_Postgres_Docker_GraphQL.Domain;
using WebApi_Postgres_Docker_GraphQL.Services;

namespace WebApi_Postgres_Docker_GraphQL.Queries;

public class UserQueries
{
    private readonly UserService _userService;

    public UserQueries(UserService userService)
    {
        _userService = userService;
    }

    public User? GetUser(Guid id)
    {
        return _userService.GetAsync(id).Result;
    }
}