using System;
using System.Collections.Generic;

namespace Email.Models;

public partial class StudentGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public int TopicId { get; set; }

    public int WalletId { get; set; }

    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();

    public virtual Topic Topic { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
