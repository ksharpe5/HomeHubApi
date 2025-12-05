using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("HomeHubDB");

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
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

app.MapPost("recipe/copy", async (IRecipeRepository repo, int id) =>
{
    var result = await repo.Copy(id);
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapPut("recipe", async (IRecipeRepository repo, RecipeDto recipe) =>
{
    var result = await repo.Update(recipe);
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapDelete("recipe", async (IRecipeRepository repo, int id) =>
{
    var result = await repo.Remove(id);
    return result ? Results.Ok() : Results.NotFound();
});
#endregion

#region Product Endpoints
app.MapGet("product", async (IProductRepository repo) =>
{
    var result = await repo.GetAll();
    return Results.Ok(result);
});

app.MapGet("product/barcode", async (IProductRepository repo, string barcode) =>
{
    var result = await repo.GetByBarcode(barcode);
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapPost("product", async (IProductRepository repo, ProductDto product) =>
{
    var result = await repo.Add(product);
    return Results.Ok(result);
});

app.MapPut("product", async (IProductRepository repo, ProductDto product) =>
{
    var result = await repo.Update(product);
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapDelete("product", async (IProductRepository repo, int id) =>
{
    var result = await repo.Remove(id);
    return result ? Results.Ok() : Results.NotFound();
});
#endregion

app.Run();