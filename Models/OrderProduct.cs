using System;
using System.Collections.Generic;

namespace Test3;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? ProductCount { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
