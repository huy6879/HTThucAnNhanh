using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HeThongDatThucAn20.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


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

        public IActionResult AddToCart(int id)
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
                    SoLuong = 1,
                    Hinh = hangHoa.Picture ?? string.Empty,

                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += 1;
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
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                if (item.SoLuong > 1)
                {
                    item.SoLuong -= 1;
                }
                else
                {
                    gioHang.Remove(item);
                }

                if (gioHang.Count == 0)
                {
                    HttpContext.Session.Remove(MySetting.CART_KEY);
                }
                else
                {
                    HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id)
        {
            var gioHang = Carts;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                item.SoLuong += 1;
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        //Điền số lượng sản phẩm trong mục Cart
        //[HttpPost]
        //public IActionResult UpdateQuantity(int id, int quantity)
        //{
        //    var gioHang = Carts;
        //    var item = gioHang.SingleOrDefault(p => p.MaHH == id);

        //    if (item != null && quantity > 0)
        //    {
        //        item.SoLuong = quantity;
        //    }
        //    else if (item != null && quantity <= 0)
        //    {
        //        gioHang.Remove(item);
        //    }

        //    if (gioHang.Count == 0)
        //    {
        //        HttpContext.Session.Remove(MySetting.CART_KEY);
        //    }
        //    else
        //    {
        //        HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
        //    }

        //    return RedirectToAction("Index");
        //}

        [Authorize]
        [HttpGet]
        public IActionResult Checkout(int id)
        {
            if (Carts.Count == 0)
            {
                return Redirect("/");
            }
            return View(Carts);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckOutVM model)
        {
            if (ModelState.IsValid)
            {
                var customerID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var khachHang = new Account();

                if (model.GiongKhachHang)
                {
                    khachHang = db.Accounts.SingleOrDefault(kh => kh.AccountId == int.Parse(customerID));
                }

                var hoaDon = new Order
                {
                    CustomerId = int.Parse(customerID),
                    CustomerName = model.HoTen ?? khachHang.Fullname,
                    PhoneNumber = model.DienThoai ?? khachHang.Phone,
                    ShipAddress = model.DiaChi ?? khachHang.Address,
                    OrderDate = DateTime.Now,
                    BranchId = 1,
                    PaymentId = 1,
                    StatusId = 0,
                    Note = model.GhiChu,
                };
                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoaDon);
                    db.SaveChanges();

                    var CTHD = new List<OrderDetail>();
                    foreach (var item in Carts)
                    {
                        CTHD.Add(new OrderDetail
                        {
                            OrderId = hoaDon.OrderId,
                            ProductId = item.MaHH,
                            Quantity = item.SoLuong,
                            UnitPrice = item.DonGia,
                            Discount = 0,
                        });
                    }

                    db.AddRange(CTHD);
                    db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    {
                        db.Database.RollbackTransaction();
                        //ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                    }
                }
            }
            return View(Carts);
        }

    }
}

