using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class Slot
{
    public int Id { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual ICollection<MentorSchedule> MentorSchedules { get; set; } = new List<MentorSchedule>();
}
