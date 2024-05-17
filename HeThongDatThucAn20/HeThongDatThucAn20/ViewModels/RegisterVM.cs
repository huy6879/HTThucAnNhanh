using System.ComponentModel.DataAnnotations;

namespace HeThongDatThucAn20.ViewModels
{
    public class RegisterVM
    {
        public int RoleId { get; set; }

        public int? BranchId { get; set; }

        [Display(Name="Họ tên")]
        [Required(ErrorMessage="*")]
        public string Fullname { get; set; } = null!;


        [Display(Name="Tài khoản")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string Email { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Repass { get; set; } = null!;


        public string? Address { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage="Chưa đúng định dạng di động")]
        public string Phone { get; set; } = null!;
        public string? Randomkey { get; set; }


    }
}
