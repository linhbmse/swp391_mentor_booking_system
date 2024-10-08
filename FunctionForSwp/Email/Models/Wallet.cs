using System;
using System.Collections.Generic;

namespace Email.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public int Balance { get; set; }

    public virtual StudentGroup? StudentGroup { get; set; }

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
