namespace HomeHubApi.DTOs;

public class InstructionDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Text { get; set; } = null!;
    public int SequenceNumber { get; set; }
}