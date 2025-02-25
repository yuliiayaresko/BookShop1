using System;
using System.Collections.Generic;

namespace BookDomain.Model;

public partial class Payment : Entity
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public DateOnly PaymentDate { get; set; }

    public string TransactionId { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
