using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MemoryGameApi.Data;
using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                typeInfo =>
                {
                    if (typeInfo.Type == typeof(Player))
                    {
                        var passwordProp = typeInfo.Properties.FirstOrDefault(
                            p => string.Equals(p.Name, "password", StringComparison.OrdinalIgnoreCase));
                        if (passwordProp != null)
                        {
                            passwordProp.ShouldSerialize = (_, _) => false;
                        }
                    }
                }
            }
        };
    });

builder.Services.AddDbContext<MemoryGameDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MemoryGamePortal")));

builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MemoryGameDbContext>();
    db.Database.Migrate();

    var admin = db.Players.FirstOrDefault(p => p.PlayerId == 1);
    if (admin != null)
    {
        admin.Username = "عبير مقرم";
        admin.Email = "abeer@gmail.com";
        admin.Password = "1234";
        admin.Role = "Admin";
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
