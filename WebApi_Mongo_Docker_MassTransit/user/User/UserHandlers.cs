using MassTransit;
using MediatR;
using MongoDB.Driver;
using user.contract;

namespace user.User;

public class CreateUserHandler : IRequestHandler<CreateUser, string>
{
    private readonly IMongoCollection<User> userCollection;
    private readonly IBus bus;

    public CreateUserHandler(
        IMongoCollection<User> userCollection,
        IBus bus
    )
    {
        this.userCollection = userCollection;
        this.bus = bus;
    }

    public async Task<string> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        if (request == null || request.User == null || request.User.Name == null)
            throw new Exception("Invalid user object");

        var user = request.User;

        await userCollection.InsertOneAsync(user);

        if (user.Id == null)
            throw new Exception("Malformed user post insert");

        await bus.Publish(new contract.UserCreated(new contract.User(user.Id, user.Name)));

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