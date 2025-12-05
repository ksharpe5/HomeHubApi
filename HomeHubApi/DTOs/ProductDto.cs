namespace HomeHubApi.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal? DefaultQuantity { get; set; }
    public int Unit { get; set; } // enum
    public string? Barcode { get; set; }
}