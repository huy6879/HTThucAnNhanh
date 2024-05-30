using HeThongDatThucAn20.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace HeThongDatThucAn20.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string domain = "https://localhost:7187/";
        HttpClient client = new HttpClient();
        [HttpGet]
        public async Task<IActionResult> CateList(int page = 1)
        {
            client.BaseAddress = new Uri(domain);

            var datajson = await client.GetStringAsync("/api/Category");
            List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
            return View(cate);
        }

        public async Task<IActionResult> Create()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/Category");
            List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
            ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModels c)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            // Thêm các trường dữ liệu khác vào form data
            formdata.Add(new StringContent(c.CateId.ToString()), "CateId");
            formdata.Add(new StringContent(c.CategoryName.ToString()), "CategoryName");
            formdata.Add(new StringContent(c.Description), "Description");
            formdata.Add(new StringContent(c.Status.ToString()), "Status");

            // Gửi yêu cầu POST đến API với dữ liệu sản phẩm
            var result = await client.PostAsync("/api/Category/create-category", formdata);

            // Kiểm tra kết quả và xử lý
            if (result.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList
                return RedirectToAction("CateList");
            }
            else
            {
                // Xử lý lỗi nếu cần thiết
                // Ví dụ: hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo sản phẩm.");
                return View(c); // Hoặc redirect đến một trang khác tùy theo logic của ứng dụng
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            client.BaseAddress = new Uri(domain);

            string datajson = await client.GetStringAsync("/api/Category/get-by-id/" + id);

            CategoryModels p = JsonConvert.DeserializeObject<CategoryModels>(datajson);

            return View(p);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Deleted(int id)
        {

            // Thiết lập địa chỉ cơ sở cho HttpClient
            client.BaseAddress = new Uri(domain);

            // Gửi yêu cầu DELETE đến API để xóa sản phẩm với productId đã cho
            var response = await client.DeleteAsync("/api/Category/delete-category/" + id);

            // Kiểm tra kết quả của yêu cầu
            if (response.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList hoặc thực hiện hành động khác tùy thuộc vào logic ứng dụng của bạn
                return RedirectToAction("cateList");
            }
            else
            {
                // Xử lý lỗi nếu cần thiết
                // Ví dụ: hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa sản phẩm.");
                // Hoặc redirect đến một trang khác tùy theo logic ứng dụng của bạn
                return RedirectToAction("Delete"); // Ví dụ: chuyển hướng đến trang ProductList để hiển thị danh sách sản phẩm
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/Category");
            List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
            ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryModels c, int id)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            // Thêm các trường dữ liệu khác vào form data
            formdata.Add(new StringContent(c.CateId.ToString()), "CateId");
            formdata.Add(new StringContent(c.CategoryName.ToString()), "CategoryName");
            formdata.Add(new StringContent(c.Description), "Description");
            formdata.Add(new StringContent(c.Status.ToString()), "Status");


            // Gửi yêu cầu POST đến API với dữ liệu sản phẩm
            var result = await client.PutAsync("/api/Category/update-product/" + id, formdata);

            // Kiểm tra kết quả và xử lý
            if (result.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList
                return RedirectToAction("CateList");
            }
            else
            {
                // Xử lý lỗi nếu cần thiết
                // Ví dụ: hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo sản phẩm.");
                return View(c); // Hoặc redirect đến một trang khác tùy theo logic của ứng dụng
            }
        }

    }
}
