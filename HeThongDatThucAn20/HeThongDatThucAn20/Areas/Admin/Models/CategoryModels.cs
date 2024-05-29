using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HeThongDatThucAn20.Areas.Admin.Models
{
    public class CategoryModels
    {
        public int CateId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public int? Status { get; set; }

    }
}
