using MediatR;

public class UpdateUserRequest : IRequest
{
    public required string Id { get; set; }
    public required UserViewModel User { get; set; }
}