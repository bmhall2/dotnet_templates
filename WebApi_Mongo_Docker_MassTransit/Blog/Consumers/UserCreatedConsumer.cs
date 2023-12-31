using MassTransit;
using user.contract;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly ILogger<UserCreatedConsumer> logger;

    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<UserCreated> context)
    {
        logger.LogInformation($"Recieved created message for: {context.Message.User.Name}");

        return Task.CompletedTask;
    }
}