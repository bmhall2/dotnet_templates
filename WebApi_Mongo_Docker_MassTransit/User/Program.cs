using System.Reflection;
using MassTransit;
using MongoDB.Driver;
using user.Settings;
using user.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection("UserDatabase"));

var config = builder.Configuration.GetSection("UserDatabase");
var mongoClient = new MongoClient(config.GetValue<string>("ConnectionString"));
builder.Services.AddSingleton<MongoClient>(mongoClient);

var mongoDatabase = mongoClient.GetDatabase(config.GetValue<string>("DatabaseName"));
var userCollection = mongoDatabase.GetCollection<User>("user");
builder.Services.AddSingleton<IMongoCollection<User>>(userCollection);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMassTransit(x =>{
    x.SetKebabCaseEndpointNameFormatter();

    // By default, sagas are in-memory, but should be changed to a durable
    // saga repository.
    x.SetInMemorySagaRepositoryProvider();

    var entryAssembly = Assembly.GetEntryAssembly();

    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
