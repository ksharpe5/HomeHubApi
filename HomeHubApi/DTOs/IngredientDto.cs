namespace HomeHubApi.DTOs;

public class IngredientDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public string Unit { get; set; } = null!;
}