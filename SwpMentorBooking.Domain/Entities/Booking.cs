using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SwpMentorBooking.Domain.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int LeaderId { get; set; }

    public int MentorScheduleId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Note { get; set; }

    public string Status { get; set; } = null!;
    
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    
    public virtual StudentDetail Leader { get; set; } = null!;
    
    public virtual MentorSchedule MentorSchedule { get; set; } = null!;
}
