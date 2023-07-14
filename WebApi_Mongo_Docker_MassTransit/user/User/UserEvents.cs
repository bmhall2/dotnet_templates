using MediatR;

public class RetrieveUser : IRequest<UserRecord>
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