using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int GivenBy { get; set; }

    public int GivenTo { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime Date { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual User GivenByNavigation { get; set; } = null!;

    public virtual User GivenToNavigation { get; set; } = null!;
}
