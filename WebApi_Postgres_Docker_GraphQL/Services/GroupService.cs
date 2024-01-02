using WebApi_Postgres_Docker_GraphQL.Domain;
using Microsoft.Extensions.Options;
using WebApi_Postgres_Docker_GraphQL.Settings;
using Npgsql;
using Dapper;

namespace WebApi_Postgres_Docker_GraphQL.Services;

public class GroupService
{
    private readonly UserService _userService;
    private readonly NpgsqlConnection _conn;

    public GroupService(
        UserService userService,
        IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        _userService = userService;
        _conn = new NpgsqlConnection
        (
            connectionString: userDatabaseSettings.Value.ConnectionString
        );

        _conn.Open();
    }

    public async Task<List<Group>> GetAsync()
    {
        var groups = await _conn.QueryAsync<Group>
        (
            $"SELECT * FROM \"group\""
        );

        foreach (var group in groups)
        {
            var userIds = (await _conn.QueryAsync<Guid>
            (
                $"SELECT * FROM \"user_group\" WHERE \"GroupId\" = @GroupId",
                new { GroupId = group.Id }
            )).ToList();

            group.Users = new List<User>();

            foreach(var userId in userIds)
            {
                var user = await _userService.GetAsync(userId);
                if (user != null)
                {
                    group.Users.Add(user);
                }
            }
        }

        return groups.ToList();
    }

    public async Task<Group?> GetAsync(Guid id)
    {
        var group = await _conn.QueryFirstOrDefaultAsync<Group>
        (
            $"SELECT * FROM \"group\" WHERE \"Id\" = @Id",
            new { Id = id }
        );

        if (group != null)
        {
            var userIds = (await _conn.QueryAsync<Guid>
            (
                $"SELECT * FROM \"user_group\" WHERE \"GroupId\" = @GroupId",
                new { GroupId = group.Id }
            )).ToList();

            group.Users = new List<User>();

            foreach(var userId in userIds)
            {
                var user = await _userService.GetAsync(userId);
                if (user != null)
                {
                    group.Users.Add(user);
                }
            }
        }

        return group;
    }

    public async Task CreateAsync(Group group)
    {
        if (group.Id == Guid.Empty)
        {
            group.Id = Guid.NewGuid();
        }

        var command = "INSERT INTO \"group\" (\"Id\", \"Name\") VALUES (@Id, @Name)";
        await _conn.ExecuteAsync(command, group);
    }

    public async Task UpdateAsync(Guid id, Group group)
    {
        var command = "UPDATE \"group\" SET \"Name\" = @Name WHERE \"Id\" = @Id";
        await _conn.ExecuteAsync(command, new { Id = id, Name = group.Name });
    }

    public async Task RemoveAsync(Guid id)
    {
        var command = "DELETE FROM \"group\" WHERE \"Id\" = @Id";
        await _conn.ExecuteAsync(command, new { Id = id });
    }

    public async Task CreateGroupUserAsync(Guid groupId, Guid userId)
    {
        var command = "INSERT INTO \"user_group\" (\"GroupId\", \"UserId\") VALUES (@GroupId, @UserId)";
        await _conn.ExecuteAsync(command, new { GroupId = groupId, UserId = userId });
    }
}