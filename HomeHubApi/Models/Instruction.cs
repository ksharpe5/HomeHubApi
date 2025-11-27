using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class Instruction
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
