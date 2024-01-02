using WebApi_Postgres_Docker_GraphQL.Domain;
using Microsoft.Extensions.Options;
using WebApi_Postgres_Docker_GraphQL.Settings;
using Npgsql;
using Dapper;

namespace WebApi_Postgres_Docker_GraphQL.Services;

public class UserService
{
    private readonly NpgsqlConnection _conn;
    public UserService(
        IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        _conn = new NpgsqlConnection
        (
            connectionString: userDatabaseSettings.Value.ConnectionString
        );

        _conn.Open();
    }

    public async Task<List<User>> GetAsync()
    {
        var users = await _conn.QueryAsync<User>
        (
            $"SELECT * FROM \"user\""
        );

        return users.ToList();
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = await _conn.QueryFirstOrDefaultAsync<User>
        (
            $"SELECT * FROM \"user\" WHERE \"Id\" = @Id",
            new { Id = id }
        );

        return user;
    }

    public async Task CreateAsync(User user)
    {
        if (user.Id == null)
        {
            user.Id = Guid.NewGuid();
        }

        var command = "INSERT INTO \"user\" (\"Id\", \"Name\") VALUES (@Id, @Name)";
        await _conn.ExecuteAsync(command, user);
    }

    public async Task UpdateAsync(Guid id, User user)
    {
        var command = "UPDATE \"user\" SET \"Name\" = @Name WHERE \"Id\" = @Id";
        await _conn.ExecuteAsync(command, new { Id = id, Name = user.Name });
    }

    public async Task RemoveAsync(Guid id)
    {
        var command = "DELETE FROM \"user\" WHERE \"Id\" = @Id";
        await _conn.ExecuteAsync(command, new { Id = id });
    }
}