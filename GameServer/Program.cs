using GameServer;
using GameServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
int min = MyConfig.GetValue<int>("AppSettings:MinValue");
int max = MyConfig.GetValue<int>("AppSettings:MaxValue");

GameHub.Gaming = Game.GetGame(min, max);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<GameHub>("/gameHub");

app.Run();
