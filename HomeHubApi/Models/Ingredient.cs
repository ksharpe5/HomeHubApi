using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string Name { get; set; } = null!;

    public float Quantity { get; set; }

    public int Unit { get; set; }

    public int SequenceNumber { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
