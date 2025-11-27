namespace HomeHubApi.DTOs;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Type { get; set; }
    public int Serves { get; set; }
    public int Duration { get; set; }
    public int TasteRating { get; set; }
    public int EffortRating { get; set; }
    public int HealthyRating { get; set; }

    public List<IngredientDto> Ingredients { get; set; } = [];
    public List<InstructionDto> Instructions { get; set; } = [];
}
