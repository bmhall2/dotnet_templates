namespace WebApi_Mongo_Docker_MediatR.Settings;

public class UserDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}