using Microsoft.AspNetCore.Mvc;

namespace HeThongDatThucAn20.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
