using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HeThongDatThucAn20.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Tên đăng nhập")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
    }
}
