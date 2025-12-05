using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeHubApi.Repositories;

public class RecipeRepository(HomeHubContext context, IMapper mapper) : IRecipeRepository
{
    // Fetch all recipes with ingredients (linked to products) and instructions
    public async Task<IEnumerable<RecipeDto>> GetAll()
    {
        return await context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Product)
            .Include(r => r.Instructions)
            .ProjectTo<RecipeDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    private async Task<Recipe?> GetById(int id)
    {
        return await context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Product)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RecipeDto> Add(RecipeDto dto)
    {
        var entity = mapper.Map<Recipe>(dto);

        // Map RecipeIngredients
        foreach (var riDto in dto.Ingredients)
        {
            var riEntity = mapper.Map<RecipeIngredient>(riDto);
            riEntity.RecipeId = entity.Id; // ensure FK
            entity.RecipeIngredients.Add(riEntity);
        }

        context.Recipes.Add(entity);
        await context.SaveChangesAsync();

        var saved = await context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Product)
            .Include(r => r.Instructions)
            .FirstAsync(r => r.Id == entity.Id);

        return mapper.Map<RecipeDto>(saved);
    }

    public async Task<RecipeDto?> Update(RecipeDto dto)
    {
        var recipe = await GetById(dto.Id);
        if (recipe == null) return null;

        // Map scalar properties
        mapper.Map(dto, recipe);

        // -----------------------------
        // Sync RecipeIngredients
        // -----------------------------
        // Remove deleted ingredients
        foreach (var ingredient in recipe.RecipeIngredients
                     .Where(i => dto.Ingredients.All(d => d.Id != i.Id))
                     .ToList())
        {
            recipe.RecipeIngredients.Remove(ingredient);
        }

        // Add or update ingredients
        foreach (var ingredientDto in dto.Ingredients)
        {
            var existing = recipe.RecipeIngredients
                .FirstOrDefault(i => i.Id == ingredientDto.Id);

            if (existing == null)
            {
                // New ingredient
                var newEntity = mapper.Map<RecipeIngredient>(ingredientDto);
                newEntity.RecipeId = recipe.Id;
                recipe.RecipeIngredients.Add(newEntity);
            }
            else
            {
                // Update existing ingredient
                mapper.Map(ingredientDto, existing);
            }
        }

        // -----------------------------
        // Sync Instructions
        // -----------------------------
        foreach (var instruction in recipe.Instructions
                     .Where(i => dto.Instructions.All(d => d.Id != i.Id))
                     .ToList())
        {
            recipe.Instructions.Remove(instruction);
        }

        foreach (var instructionDto in dto.Instructions)
        {
            var existing = recipe.Instructions
                .FirstOrDefault(i => i.Id == instructionDto.Id);

            if (existing == null)
            {
                var newEntity = mapper.Map<Instruction>(instructionDto);
                newEntity.RecipeId = recipe.Id;
                recipe.Instructions.Add(newEntity);
            }
            else
            {
                mapper.Map(instructionDto, existing);
            }
        }

        await context.SaveChangesAsync();
        return mapper.Map<RecipeDto>(recipe);
    }

    public async Task<bool> Remove(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        context.Recipes.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}

public interface IRecipeRepository
{
    Task<IEnumerable<RecipeDto>> GetAll();
    Task<RecipeDto> Add(RecipeDto recipe);
    Task<RecipeDto?> Update(RecipeDto dto);
    Task<bool> Remove(int id);
}