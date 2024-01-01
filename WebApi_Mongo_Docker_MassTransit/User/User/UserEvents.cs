using MediatR;

namespace user.User;

public class CreateUser : IRequest<string>
{
    public CreateUser(User user)
    {
        this.User = user;
    }

    public User User { get; set; }
}

public class RetrieveUsers : IRequest<List<User>>
{

}

public class RetrieveUser : IRequest<User>
{
    public RetrieveUser(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
}

public class UpdateUser : IRequest
{
    public UpdateUser(string userId, User user)
    {
        UserId = userId;
        User = user;
    }

    public string UserId { get; set; }
    public User User { get; set; }
}

public class DeleteUser : IRequest
{
    public DeleteUser(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
}