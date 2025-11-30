using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeHubApi.Repositories;

public class RecipeRepository(HomeHubContext context, IMapper mapper) : IRecipeRepository
{
    public async Task<IEnumerable<RecipeDto>> GetAll()
    {
        return await context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .ProjectTo<RecipeDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    private async Task<Recipe?> GetById(int id)
    {
        return await context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RecipeDto> Add(RecipeDto dto)
    {
        var entity = mapper.Map<Recipe>(dto);
        
        context.Recipes.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<RecipeDto>(entity);
    }

    public async Task<RecipeDto?> Update(RecipeDto dto)
    {
        var recipe = await GetById(dto.Id);
        if (recipe == null) return null;

        // Map scalar properties (Name, Ratings, etc.) and preserve child collections
        mapper.Map(dto, recipe);

        // -----------------------------
        // Sync Ingredients
        // -----------------------------
        // Remove deleted ingredients
        foreach (var ingredient in recipe.Ingredients
                     .Where(i => dto.Ingredients.All(d => d.Id != i.Id))
                     .ToList())
        {
            recipe.Ingredients.Remove(ingredient);
        }

        // Add or update ingredients
        foreach (var ingredientDto in dto.Ingredients)
        {
            var existing = recipe.Ingredients
                .FirstOrDefault(i => i.Id == ingredientDto.Id);

            if (existing == null)
            {
                // New ingredient
                var newEntity = mapper.Map<Ingredient>(ingredientDto);
                newEntity.RecipeId = recipe.Id;
                recipe.Ingredients.Add(newEntity);
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
                // New instruction
                var newEntity = mapper.Map<Instruction>(instructionDto);
                newEntity.RecipeId = recipe.Id;
                recipe.Instructions.Add(newEntity);
            }
            else
            {
                // Update existing instruction
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