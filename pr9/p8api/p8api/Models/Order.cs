using System;
using System.Collections.Generic;

namespace p8api.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int UserId { get; set; }

    public int OrderStatusId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
