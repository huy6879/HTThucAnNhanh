using System;
using System.Collections.Generic;

namespace HeThongDatThucAn20.Data;

public partial class Category
{
    public int CateId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
