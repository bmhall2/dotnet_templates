using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using User.Contract;

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