using System;
using System.Collections.Generic;

namespace Email.Models;

public partial class AdminDetail
{
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
