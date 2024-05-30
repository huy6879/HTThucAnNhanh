using HeThongDatThucAn20.Data;

namespace HeThongDatThucAn20.ViewModels
{
    public class AccountCreateVM
    {
        public int AccountId { get; set; }

        public int? BranchId { get; set; } = null;

        public int RoleId { get; set; }

        public string Fullname { get; set; } = null!;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        //public string? RandomKey { get; set; }
    }
}
