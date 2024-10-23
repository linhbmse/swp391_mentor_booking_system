using System;
using System.Collections.Generic;

namespace SwpMentorBooking.Domain.Entities;

public partial class Wallet
{
    public int Id { get; set; }

    public int Balance { get; set; } = 200;

    public virtual StudentGroup? StudentGroup { get; set; }

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
