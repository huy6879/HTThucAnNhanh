using Microsoft.AspNetCore.Mvc;
using HeThongDatThucAn20.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace HeThongDatThucAn20.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        string domain = "https://localhost:7187/";
        HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> ListOrder(int page = 1)
        {
            client.BaseAddress = new Uri(domain);
            var datajson = await client.GetStringAsync("/api/Order");
            List<OrderModels> branch = JsonConvert.DeserializeObject<List<OrderModels>>(datajson);
            return View(branch);
        }
        // Tao chi nhanh moi
        public async Task<IActionResult> Create()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/Order/");
            List<OrderModels> branch = JsonConvert.DeserializeObject<List<OrderModels>>(datajson);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderModels o)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            formdata.Add(new StringContent(o.OrderId.ToString()), "OrderId");
            formdata.Add(new StringContent(o.CustomerId.ToString()), "CustomerId");
            formdata.Add(new StringContent(o.CustomerName), "CustomerName");
            formdata.Add(new StringContent(o.PhoneNumber), "PhoneNumber");
            formdata.Add(new StringContent(o.PaymentId.ToString()), "PaymentId");
            formdata.Add(new StringContent(o.BranchId.ToString()), "BranchId");
            formdata.Add(new StringContent(o.ShipAddress), "ShipAddress");
            formdata.Add(new StringContent(o.OrderDate.ToString()), "OrderDate");
            formdata.Add(new StringContent(o.ShipDate.ToString()), "ShipDate");
            formdata.Add(new StringContent(o.StatusId.ToString()), "StatusId");

            var result = await client.PostAsync("/api/Order/create-order", formdata);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ListOrder");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi tạo đơn hàng mới.");
                return View(o);
            }
        }

        //Update chi nhanh
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/Order");
            List<OrderModels> branch = JsonConvert.DeserializeObject<List<OrderModels>>(datajson);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderModels o, int id)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            formdata.Add(new StringContent(o.OrderId.ToString()), "OrderId");
            formdata.Add(new StringContent(o.CustomerId.ToString()), "CustomerId");
            formdata.Add(new StringContent(o.CustomerName), "CustomerName");
            formdata.Add(new StringContent(o.PhoneNumber), "PhoneNumber");
            formdata.Add(new StringContent(o.PaymentId.ToString()), "PaymentId");
            formdata.Add(new StringContent(o.BranchId.ToString()), "BranchId");
            formdata.Add(new StringContent(o.ShipAddress), "ShipAddress");
            formdata.Add(new StringContent(o.OrderDate.ToString()), "OrderDate");
            formdata.Add(new StringContent(o.ShipDate.ToString()), "ShipDate");
            formdata.Add(new StringContent(o.StatusId.ToString()), "StatusId");
            var result = await client.PutAsync("/api/Order/update-order/" + id, formdata);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ListOrder");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi chỉnh sửa đơn hàng.");
                return View(o);
            }
        }

        //Xoa chi nhanh
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            client.BaseAddress = new Uri(domain);

            string datajson = await client.GetStringAsync("/api/Order/get-by-id/" + id);

            OrderModels p = JsonConvert.DeserializeObject<OrderModels>(datajson);

            return View(p);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Deleted(int id)
        {

            client.BaseAddress = new Uri(domain);

            var response = await client.DeleteAsync("/api/Order/delete-order/" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListOrder");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi xóa đơn hàng.");

                return RedirectToAction("Delete");
            }
        }
    }
}
