using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeThongDatThucAn20.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly HeThongDatDoAnContext db;

        public MenuLoaiViewComponent(HeThongDatDoAnContext context) => db = context;
        
        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(c => new MenuLoaiVM
            {
                MaLoai = c.CateId, 
                TenLoai = c.CategoryName, 
                SoLuong = c.Products.Count
            });
            return View(data);
        }
    }
}
