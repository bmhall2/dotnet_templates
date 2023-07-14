using MassTransit;
using MediatR;
using MongoDB.Driver;
using User.Contract;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, string>
{
    private readonly IMongoCollection<UserRecord> userCollection;
    private readonly IBus bus;

    public CreateUserHandler(
        IMongoCollection<UserRecord> userCollection,
        IBus bus
    )
    {
        this.userCollection = userCollection;
        this.bus = bus;
    }

    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.User == null || request.User.Name == null)
            throw new Exception("Invalid user object");

        var user = new UserRecord
        {
            Name = request.User.Name
        };

        await userCollection.InsertOneAsync(user);

        if (user.Id == null)
            throw new Exception("Malformed user post insert");

        await bus.Publish(new UserCreated(new User.Contract.User(user.Id, user.Name)));

        return user.Id ?? "error";
    }
}

public class RetrieveUsersHandler : IRequestHandler<RetrieveUsersRequest, List<UserRecord>>
{
    private readonly IMongoCollection<UserRecord> userCollection;

    public RetrieveUsersHandler(
        IMongoCollection<UserRecord> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    public async Task<List<UserRecord>> Handle(RetrieveUsersRequest request, CancellationToken cancellationToken)
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

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest>
{
    private readonly IMongoCollection<UserRecord> userCollection;

    public UpdateUserHandler(
        IMongoCollection<UserRecord> userCollection
    )
    {
        this.userCollection = userCollection;
    }

    async Task IRequestHandler<UpdateUserRequest>.Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.User == null)
            throw new Exception("Invalid user object");

        var user = new UserRecord
        {
            Name = request.User.Name
        };

        await userCollection.ReplaceOneAsync(x => x.Id == request.Id, user);
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