using System.Data.Common;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChatAppBackend.Data;
using ChatAppBackend.Hubs;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using var factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger<Program>();

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")).UseSnakeCaseNamingConvention());

var connectionString = builder.Configuration.GetConnectionString("Mariadb");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseSnakeCaseNamingConvention().LogTo(Console.WriteLine, LogLevel.Debug));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<RegisteredUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
    // options.SignIn.RequireConfirmedAccount = true;
    // options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    // options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.Configure<JsonOptions>(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    option.JsonSerializerOptions.NumberHandling =
        JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString;
});

builder.Services.AddSignalR(options => { options.StreamBufferCapacity = 0; })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.NumberHandling =
            JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString;
    });

builder.Services.AddControllers(); // swagger
builder.Services.AddEndpointsApiExplorer(); // swagger
builder.Services.AddSwaggerGen(); // swagger

// builder.Services.AddIdGen(0); // snowflake id

// builder.Services.AddSignalR().AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();


if (app.Environment.IsDevelopment()) // swagger
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<ChatHub>("/Chat");
app.MapGet("Test", () => "Sent from C#!");

app.MapGet("/Api/IsLoggedIn", () => { return Results.Ok(); }).RequireAuthorization();

// app.UseRouting();


app.MapGroup("/Api/Auth").MapIdentityApi<RegisteredUser>();

app.UseAuthorization();

logger.LogInformation("Started ChatApp backend server!");
app.Run();