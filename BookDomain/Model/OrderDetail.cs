using System;
using System.Collections.Generic;

namespace BookDomain.Model;

public partial class OrderDetail : Entity
{
   

    public int StatusId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public decimal? Price { get; set; }


    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
