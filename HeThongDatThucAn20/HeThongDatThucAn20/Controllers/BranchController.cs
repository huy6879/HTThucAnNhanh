using HeThongDatThucAn20.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongDatThucAn20.Controllers
{
    public class BranchController : Controller
    {
        private readonly HeThongDatDoAnContext db;
        public BranchController(HeThongDatDoAnContext context)
        {
            db = context;
        }
        // GET: BranchController
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult ListBranch()
        {
            IEnumerable<Branch> ds = db.Branches.Select(s => s);
            return View(ds);
        }
        public ActionResult Details(int id)
        {
            var details = db.Branches.FirstOrDefault(d => d.BranchId == id);
            ViewBag.branch = details;
            return View(details);
        }

    }
}