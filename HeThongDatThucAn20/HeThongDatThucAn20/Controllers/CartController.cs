using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HeThongDatThucAn20.Helpers;
using HeThongDatThucAn20.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongDatThucAn20.Controllers
{
    public class CartController : Controller
    {
        private readonly HeThongDatDoAnContext db;
        public CartController(HeThongDatDoAnContext context) 
        {
            db = context;
        }

        public List<CartItem> Carts => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Carts);
        }

        public IActionResult AddToCart(int id, int sl = 1)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item == null)//chưa có
            {
                var hangHoa = db.Products.SingleOrDefault(p => p.ProductId == id);
                if (hangHoa == null)
                {
                    return NotFound();
                }
                item = new CartItem
                {
                    MaHH = id,
                    TenHH = hangHoa.ProductName,
                    DonGia = (float)hangHoa.UnitPrice,
                    SoLuong = sl,
                    Hinh = hangHoa.Picture ?? string.Empty,

                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += sl;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang) ;
            }
            return RedirectToAction("Index");
        }
    }
}

