using System.ComponentModel.DataAnnotations;

namespace HeThongDatThucAn20.Areas.Admin.Models
{
    public class ThongKeModels
    {
        [Key]
        public int BranchId { get; set; }   
        public string BranchName { get; set; }  
        public string ?LoaiSP { get; set; }
        public int ?SoLuong { get; set; }
        public double ?DoanhThu { get; set; }    
    }
}
