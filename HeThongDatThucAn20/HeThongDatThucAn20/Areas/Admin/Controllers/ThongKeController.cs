using HeThongDatThucAn20.Areas.Admin.Models;
using HeThongDatThucAn20.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace HeThongDatThucAn20.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ThongKeController : Controller
    {

        private readonly HeThongDatDoAnContext db;
            
        public ThongKeController()
        {
            db = new HeThongDatDoAnContext(); // Replace YourDbContext with your actual DbContext
        }

        public ActionResult Index()
        {
            // Danh sách tháng

            ViewBag.Month = new SelectList(Enumerable.Range(1, 12));
            ViewBag.Year = new SelectList(Enumerable.Range(DateTime.Now.Year - 9, 10).OrderByDescending(y => y), DateTime.Now.Year);

            return View();

        }

        public ActionResult DoanhThu(int month, int year)
        {

            ViewBag.m = month;
            ViewBag.y = year;

            var branches = db.Branches.OrderBy(b => b.BranchId).ToList(); // Lấy tất cả các chi nhánh từ cơ sở dữ liệu

            var branchRevenues = new List<Tuple<int, double>>(); // Danh sách chứa doanh thu của mỗi chi nhánh

            foreach (var branch in branches)
            {
                double branchRevenue = (double)db.OrderDetails
                                    .Where(od => od.Order.BranchId == branch.BranchId && od.Order.OrderDate.Month == month && od.Order.OrderDate.Year == year) // Lọc các đơn hàng của chi nhánh hiện tại
                                    .Join(
                                        db.Products,
                                        orderDetail => orderDetail.ProductId,
                                        product => product.ProductId,
                                        (orderDetail, product) => new { OrderDetail = orderDetail, Product = product }
                                    )
                                    .Sum(o => o.OrderDetail.Quantity * o.Product.UnitPrice); // Tính tổng doanh thu từ số lượng sản phẩm và giá của mỗi sản phẩm

                branchRevenues.Add(new Tuple<int, double>(branch.BranchId, branchRevenue)); // Thêm doanh thu của chi nhánh vào danh sách
            }

            foreach (var branchRevenue in branchRevenues)
            {
                var revenue = branchRevenue.Item2; ;
            }

            var viewModel = new List<ThongKeModels>(); // Danh sách chứa thông tin thống kê của từng chi nhánh\

            foreach (var branchRevenue in branchRevenues)
            {
                var branchName = db.Branches.First(b => b.BranchId == branchRevenue.Item1).BranchName; // Lấy tên của chi nhánh

                viewModel.Add(new ThongKeModels
                {
                    BranchId = branchRevenue.Item1,
                    BranchName = branchName,
                    DoanhThu = branchRevenue.Item2,
                });
            }
            return View(viewModel);
        }
        //public ActionResult Index()
        //{

        //}
        // GET: ThongKeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ThongKeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThongKeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThongKeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ThongKeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThongKeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ThongKeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
