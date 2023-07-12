using MediatR;
using MongoDB.Driver;

namespace WebApi_Mongo_Docker_MediatR.User;

public class CreateUserHandler : IRequestHandler<CreateUser, string>
{
    private readonly IMongoCollection<User> userCollection;

    public CreateUserHandler(
        IMongoCollection<User> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    public async Task<string> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        if (request == null || request.User == null)
            throw new Exception("Invalid user object");

        var user = request.User;
        await userCollection.InsertOneAsync(user);

        return user.Id ?? "error";
    }
}

public class RetrieveUsersHandler : IRequestHandler<RetrieveUsers, List<User>>
{
    private readonly IMongoCollection<User> userCollection;

    public RetrieveUsersHandler(
        IMongoCollection<User> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    public async Task<List<User>> Handle(RetrieveUsers request, CancellationToken cancellationToken)
    {
        return await userCollection.Find(_ => true).ToListAsync();
    }
}

public class RetrieveUserHandler : IRequestHandler<RetrieveUser, User>
{
    private readonly IMongoCollection<User> userCollection;

    public RetrieveUserHandler(
        IMongoCollection<User> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    public async Task<User> Handle(RetrieveUser request, CancellationToken cancellationToken)
    {
        return await userCollection.Find(x => x.Id == request.UserId).FirstOrDefaultAsync();
    }
}

public class UpdateUserHandler : IRequestHandler<UpdateUser>
{
    private readonly IMongoCollection<User> userCollection;

    public UpdateUserHandler(
        IMongoCollection<User> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    async Task IRequestHandler<UpdateUser>.Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        if (request == null || request.User == null)
            throw new Exception("Invalid user object");

        var user = request.User;
        await userCollection.ReplaceOneAsync(x => x.Id == request.UserId, user);
    }
}

public class DeleteUserHandler : IRequestHandler<DeleteUser>
{
    private readonly IMongoCollection<User> userCollection;

    public DeleteUserHandler(
        IMongoCollection<User> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    public async Task Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        await userCollection.DeleteOneAsync(x => x.Id == request.UserId);
    }
}