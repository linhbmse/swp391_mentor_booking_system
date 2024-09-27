using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class StudentDetail
{
    public int UserId { get; set; }

    public string StudentCode { get; set; } = null!;

    public int? GroupId { get; set; }

    public bool IsLeader { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual StudentGroup? Group { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User User { get; set; } = null!;
}
