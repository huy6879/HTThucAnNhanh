using System;
using System.Collections.Generic;

namespace HeThongDatThucAn20.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int PaymentId { get; set; }

    public int BranchId { get; set; }

    public string ShipAddress { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public double? ShippingFee { get; set; }

    public int StatusId { get; set; }

    public string? Note { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Account Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment Payment { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;
}
