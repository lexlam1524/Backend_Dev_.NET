using robot_controller_api.Persistence;
using FastMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using robot_controller_api.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using robot_controller_api.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RobotContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RobotDatabase")));

// Add services to the container.
builder.Services.AddControllers();
//4.1p
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
builder.Services.AddScoped<IMapDataAccess, MapADO>();
builder.Services.AddScoped<IUserDataAccess, UserADO>();
builder.Services.AddScoped<PasswordHasher<UserModel>>();

//4.2c
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
//builder.Services.AddScoped<IMapDataAccess, MapRepository>();
//4.3D
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
//builder.Services.AddScoped<IMapDataAccess, MapEF>();
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler> ("BasicAuthentication", default);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
