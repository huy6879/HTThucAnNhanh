using System.ComponentModel.DataAnnotations;

namespace HeThongDatThucAn20.Areas.Admin.Models
{
    public class BranchModels
    {
        [Key]
        public int BranchId { get; set; }

        public string BranchName { get; set; } = null!;

        public string BranchCity { get; set; } = null!;

        public string BranchDistrict { get; set; } = null!;

        public string BranchAddress { get; set; } = null!;
    }
}
