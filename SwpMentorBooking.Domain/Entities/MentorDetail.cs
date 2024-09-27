using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class MentorDetail
{
    public int UserId { get; set; }

    public string? MainProgrammingLanguage { get; set; }

    public string? AltProgrammingLanguage { get; set; }

    public string? Framework { get; set; }

    public string? Education { get; set; }

    public string? AdditionalContactInfo { get; set; }

    public int? BookingScore { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MentorSchedule> MentorSchedules { get; set; } = new List<MentorSchedule>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

    public virtual ICollection<Specialization> Specs { get; set; } = new List<Specialization>();
}
