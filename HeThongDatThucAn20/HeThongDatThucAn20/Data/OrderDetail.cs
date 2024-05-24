﻿using System;
using System.Collections.Generic;

namespace HeThongDatThucAn20.Data;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public float? Discount { get; set; }

    public double UnitPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
