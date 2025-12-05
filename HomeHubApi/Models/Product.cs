using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Barcode { get; set; }

    public string Name { get; set; } = null!;

    public int Unit { get; set; }

    public decimal? DefaultQuantity { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
