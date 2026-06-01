using System;
using System.Collections.Generic;

namespace Test3;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? PickupPointId { get; set; }

    public int? CustomerId { get; set; }

    public int? Code { get; set; }

    public int? StatusId { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PickupPoint? PickupPoint { get; set; }

    public virtual OrderStatus? Status { get; set; }
}
