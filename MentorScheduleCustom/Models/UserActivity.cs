using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class UserActivity
{
    public int Id { get; set; }

    public int ActivityTypeId { get; set; }

    public string? Description { get; set; }

    public int OperatorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserActivityType ActivityType { get; set; } = null!;

    public virtual User Operator { get; set; } = null!;
}
