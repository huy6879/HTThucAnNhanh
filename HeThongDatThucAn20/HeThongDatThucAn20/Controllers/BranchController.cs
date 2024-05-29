using HeThongDatThucAn20.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [HttpGet]
        public ActionResult SearchByDistrict()
        {
            // Initially display all branches or an empty list
            var branches = db.Branches.ToList();
            return View(branches);
        }

        [HttpPost]
        public ActionResult SearchByDistrict(string districtName)
        {
            if (districtName == null)
            {
                return RedirectToAction("ListBranch");
            }
            else
            {
                var filteredBranches = db.Branches
                .Where(b => b.BranchDistrict.Contains(districtName))
                .ToList();

                ViewBag.DistrictName = districtName;
                return View(filteredBranches);
            }
            
        }
    }
}