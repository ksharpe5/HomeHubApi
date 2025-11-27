using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int Serves { get; set; }

    public int Duration { get; set; }

    public int TasteRating { get; set; }

    public int EffortRating { get; set; }

    public int HealthyRating { get; set; }

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
}
