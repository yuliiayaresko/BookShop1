using System;
using System.Collections.Generic;

namespace BookDomain.Model;

public partial class OrderDetail 
{

    public int OrderDetailId { get; set; }

    public string Status { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; } 
    public int OrderId { get; set; }
    public decimal? TotalPrice { get; set; }


    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
