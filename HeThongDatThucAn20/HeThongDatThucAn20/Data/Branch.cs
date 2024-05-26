using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace HeThongDatThucAn20.Data;

public partial class Branch
{
    [DisplayName("Mã chi nhánh")]
    public int BranchId { get; set; }
    [DisplayName("Tên chi nhánh")]
    public string BranchName { get; set; } = null!;
    [DisplayName("Thành phố")]
    public string BranchCity { get; set; } = null!;

    public string BranchDistrict { get; set; } = null!;
    [DisplayName("Địa chỉ")]
    public string BranchAddress { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
