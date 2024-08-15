using robot_controller_api.Persistence;
using FastMember;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RobotContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RobotDatabase")));

// Add services to the container.
builder.Services.AddControllers();
//4.1p
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
//builder.Services.AddScoped<IMapDataAccess, MapADO>();
//4.2c
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
//builder.Services.AddScoped<IMapDataAccess, MapRepository>();
//4.3D
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
