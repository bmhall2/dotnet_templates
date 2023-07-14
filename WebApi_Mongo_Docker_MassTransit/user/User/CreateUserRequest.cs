using MediatR;

public class CreateUserRequest : IRequest<string>
{
    public required UserViewModel User { get; set; }
}