using System;
using System.Collections.Generic;

namespace BookDomain.Model;

public partial class WarehouseBook 
{
    public int WarehouseCode { get; set; }

    public int BookId { get; set; }

    public int Count { get; set; }

    public virtual Book Book { get; set; } = null!;
}
