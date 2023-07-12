using MongoDB.Driver;
using WebApi_Mongo_Docker_MediatR.Settings;
using WebApi_Mongo_Docker_MediatR.User;

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
