using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("HomeHubDB");

// Add services to the container.
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddOpenApi();
builder.Services.AddDbContext<HomeHubContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

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

app.Run();