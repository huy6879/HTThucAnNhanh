using Microsoft.AspNetCore.Mvc;

namespace HeThongDatThucAn20.Controllers
{
    public class CartController : Controller
    {
        //private readonly HeThongDatThucAn20
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            return View();
        }
    }
}
