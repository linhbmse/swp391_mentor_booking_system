using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class Specialization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MentorDetail> MentorDetails { get; set; } = new List<MentorDetail>();
}
