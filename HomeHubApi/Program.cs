using AutoMapper;
using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Repositories;
using HomeHubApi.SettingsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("HomeHubDB");

builder.Services.Configure<TapoCredentials>(
    builder.Configuration.GetSection("TapoCredentials"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLAN", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(
                "http://localhost:4200",
                "http://192.168.2.*"
            );
    });
});

// Add services to the container.
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddOpenApi();
builder.Services.AddDbContext<HomeHubContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<ITapoDeviceRepository, TapoDeviceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseCors("AllowLAN");

#region Recipe Endpoints
app.MapGet("recipe", async (IRecipeRepository repo) =>
{
    var result  = await repo.GetAll();
    return Results.Ok(result);
});

app.MapPost("recipe", async (IRecipeRepository repo, RecipeDto recipe) =>
{
    var result  = await repo.Add(recipe);
    return Results.Ok(result);
});

app.MapPut("recipe", async (IRecipeRepository repo, RecipeDto recipe) =>
{
    var result = await repo.Update(recipe);
    return Results.Ok(result);
});

app.MapDelete("recipe", async (IRecipeRepository repo, int id) =>
{
    var result = await repo.Remove(id);
    return result ? Results.Ok() : Results.NotFound();
});
#endregion

#region Tapo Device Endpoints
app.MapGet("devices", async (ITapoDeviceRepository repo) =>
{
    var result = (await repo.GetAll()).ToList();
    
    return Results.Ok(result);
});

app.MapPost("devices/off", async (ITapoDeviceRepository repo, string ipAddress) =>
{
    await repo.TurnOffDevice(ipAddress);
    return Results.NoContent();
});

app.MapPost("devices/on", async (ITapoDeviceRepository repo, string ipAddress) =>
{
    await repo.TurnOnDevice(ipAddress);
    return Results.NoContent();
});

//
// app.MapPost("devices", async (ITapoDeviceRepository repo, TapoDeviceDto device) =>
// {
//     var result = await repo.Add(device);
//     return Results.Ok(result);
// });
//
// app.MapPut("devices", async (ITapoDeviceRepository repo, TapoDeviceDto device) =>
// {
//     var result = await repo.Update(device);
//     return result is null ? Results.NotFound() : Results.Ok(result);
// });
//
// app.MapDelete("devices", async (ITapoDeviceRepository repo, int id) =>
// {
//     var result = await repo.Delete(id);
//     return Results.Ok(result);
// });
#endregion

app.Run();