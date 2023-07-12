using WebApi_Mongo_Docker.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi_Mongo_Docker.Settings;

namespace WebApi_Mongo_Docker.Services;

public class UserService
{
    private readonly IMongoCollection<User> _userCollection;

    public UserService(
        IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            userDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>("user");
    }

    public async Task<List<User>> GetAsync() =>
        await _userCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _userCollection.InsertOneAsync(user);

    public async Task UpdateAsync(string id, User user) =>
        await _userCollection.ReplaceOneAsync(x => x.Id == id, user);

    public async Task RemoveAsync(string id) =>
        await _userCollection.DeleteOneAsync(x => x.Id == id);
}