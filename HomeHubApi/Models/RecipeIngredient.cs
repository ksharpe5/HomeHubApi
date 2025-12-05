using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int ProductId { get; set; }

    public decimal QuantityRequired { get; set; }
    
    public int Unit { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
