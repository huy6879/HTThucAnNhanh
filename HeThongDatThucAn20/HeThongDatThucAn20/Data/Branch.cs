﻿using System;
using System.Collections.Generic;

namespace HeThongDatThucAn20.Data;

public partial class Branch
{
    public int BranchId { get; set; }

    public int? EmployeeId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCity { get; set; } = null!;

    public string BranchDistrict { get; set; } = null!;

    public string BranchAddress { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Account? Employee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
