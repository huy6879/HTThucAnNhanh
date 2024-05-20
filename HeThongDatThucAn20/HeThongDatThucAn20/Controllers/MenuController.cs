using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeThongDatThucAn20.Controllers
{
    public class MenuController : Controller
    {
        private readonly HeThongDatDoAnContext db;
        public MenuController(HeThongDatDoAnContext context)
        {
            db = context;
        }
        public IActionResult Index(int? loai)
        {
            var sanphams = db.Products.AsQueryable();

            if(loai.HasValue)
            {
                sanphams = sanphams.Where(p => p.CateId == loai.Value);
            }

            var result = sanphams.Select(p => new SanPhamVM
            {
               MaHH = p.ProductId,
               TenHH = p.ProductName,
               DonGia = p.UnitPrice,
               Hinh = p.Picture ??  "",
               MoTa = p.Description,
               TenLoai = p.Cate.CategoryName
            });
            return View(result);
        }
    }
}
