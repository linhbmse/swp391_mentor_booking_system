using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MentorDetail> MentorDetails { get; set; } = new List<MentorDetail>();
}
