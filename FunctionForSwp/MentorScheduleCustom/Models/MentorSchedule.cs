using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class MentorSchedule
{
    public int Id { get; set; }

    public int MentorDetailId { get; set; }

    public int SlotId { get; set; }

    public DateOnly Date { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual MentorDetail MentorDetail { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
