using WebApi_Postgres_Docker_GraphQL.Queries;
using WebApi_Postgres_Docker_GraphQL.Services;
using WebApi_Postgres_Docker_GraphQL.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection("UserDatabase"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
        .AddTypeExtension<UserQueries>()
        .AddTypeExtension<GroupQueries>();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<GroupService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGraphQL();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
