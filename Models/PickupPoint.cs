using System;
using System.Collections.Generic;

namespace Test3;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
