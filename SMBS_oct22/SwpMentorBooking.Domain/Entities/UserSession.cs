using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class UserSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime ExpireTime { get; set; }

    public virtual User User { get; set; } = null!;
}
