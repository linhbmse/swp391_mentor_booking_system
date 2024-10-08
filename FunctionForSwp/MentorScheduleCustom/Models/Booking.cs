using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int LeaderId { get; set; }

    public int MentorScheduleId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual StudentDetail Leader { get; set; } = null!;

    public virtual MentorSchedule MentorSchedule { get; set; } = null!;
}
