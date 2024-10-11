using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class WalletTransaction
{
    public int Id { get; set; }

    public int WalletId { get; set; }

    public int Amount { get; set; }

    public string Type { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
