namespace HomeHubApi.DTOs;

public class IngredientDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    
    public string Name { get; set; } = null!;
    public float Quantity { get; set; }
    public int Unit { get; set; }
    public int SequenceNumber { get; set; }
}