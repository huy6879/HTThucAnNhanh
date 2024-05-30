using HeThongDatThucAn20.Data;

namespace HeThongDatThucAn20.ViewModels
{
    public class AccountVM
    {
        public int AccountId { get; set; }

        public int? BranchId { get; set; } = null;

        public int RoleId { get; set; }

        public string Fullname { get; set; } = null!;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public virtual Branch? Branch { get; set; }

        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual RoleVM Role { get; set; } = null!;
    }
}
