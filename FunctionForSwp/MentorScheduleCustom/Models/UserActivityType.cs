using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class UserActivityType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
}
