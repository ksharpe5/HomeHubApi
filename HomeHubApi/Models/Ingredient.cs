using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string Name { get; set; } = null!;

    public int Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
