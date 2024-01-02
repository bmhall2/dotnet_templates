namespace WebApi_Postgres_Docker_GraphQL.Settings;

public class UserDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}