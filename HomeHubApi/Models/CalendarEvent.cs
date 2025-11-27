using System;
using System.Collections.Generic;

namespace HomeHubApi.Models;

public partial class CalendarEvent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }

    public int? TravelTime { get; set; }

    public string? Location { get; set; }

    public string? Attendees { get; set; }
}
