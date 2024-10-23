using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class AdminDetail
{
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
