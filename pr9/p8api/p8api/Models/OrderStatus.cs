using System;
using System.Collections.Generic;

namespace p8api.Models;

public partial class OrderStatus
{
    public int IdOrderStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
