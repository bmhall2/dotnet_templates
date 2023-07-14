using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace user.User;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }
}