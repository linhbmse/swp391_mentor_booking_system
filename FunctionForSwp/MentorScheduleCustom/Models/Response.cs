using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.Models;

public partial class Response
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Request Request { get; set; } = null!;
}
