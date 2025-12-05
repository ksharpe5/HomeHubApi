namespace HomeHubApi.DTOs;

public class RecipeIngredientDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public ProductDto Product { get; set; } = null!;
    public decimal QuantityRequired { get; set; }
    public int Unit { get; set; }
    public int SequenceNumber { get; set; }
}