using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HeThongDatThucAn20.Areas.Admin.Models
{
    public class ProductModels
    {
        [Key]
        public int ProductId { get; set; }
        public int CateId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
        public int? UnitInStock { get; set; }
        public string? Description { get; set; }
    }
}
