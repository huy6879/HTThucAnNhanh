using System;
using System.Collections.Generic;

namespace HeThongDatThucAn20.Data;

public partial class Account
{
    public int AccountId { get; set; }

    public int? BranchId { get; set; }

    public int RoleId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RandomKey { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
