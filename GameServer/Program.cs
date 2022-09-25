using GameServer.GamePlay;
using GameServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<GameHub>("/gameHub");

app.Run();


void ConfigureServices(IServiceCollection services)
{
    // Get Configuration values for min and max
    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    int min = MyConfig.GetValue<int>("AppSettings:MinValue");
    int max = MyConfig.GetValue<int>("AppSettings:MaxValue");

    services.AddControllers();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.IgnoreObsoleteActions();
        c.IgnoreObsoleteProperties();
        c.CustomSchemaIds(type => type.FullName);
    });

    services.AddSignalR();

    //Registry of the IGame as a singleton for injection
    services.Add(new ServiceDescriptor(typeof(IGame), Game.GetGame(min, max)));
}