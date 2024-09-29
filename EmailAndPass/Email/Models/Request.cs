using System;
using System.Collections.Generic;

namespace Email.Models;

public partial class Request
{
    public int Id { get; set; }

    public int LeaderId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Status { get; set; } = null!;

    public virtual StudentDetail Leader { get; set; } = null!;

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
