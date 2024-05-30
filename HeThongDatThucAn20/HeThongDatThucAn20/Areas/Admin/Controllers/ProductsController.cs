using HeThongDatThucAn20.Areas.Admin.Models;
using HeThongDatThucAn20.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;


namespace HeThongDatThucAn20.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        string domain = "https://localhost:7187/";
        HttpClient client = new HttpClient();

        [HttpGet]
        public async Task <IActionResult> ProductList()
        {
            client.BaseAddress = new Uri(domain);
            string datajson =  await client.GetStringAsync("/api/Product/");
            ProductModels[] productsArray = JsonConvert.DeserializeObject<ProductModels[]>(datajson);

            // Chuyển đổi mảng thành danh sách để truyền cho view
            List<ProductModels> products = productsArray.ToList();

            return View(products);
        }
        public async Task <IActionResult> Create()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/Category");
            List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
            ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    
        public async  Task<IActionResult> Create(Product p)
        {
            client.BaseAddress = new Uri(domain);
            var formdata = new MultipartFormDataContent();

            // Thêm các trường dữ liệu khác vào form data
            formdata.Add(new StringContent(p.ProductId.ToString()), "ProductId");
            formdata.Add(new StringContent(p.CateId.ToString()), "CateId");
            formdata.Add(new StringContent(p.ProductName), "ProductName");
            formdata.Add(new StringContent(p.UnitPrice.ToString()), "UnitPrice");
            formdata.Add(new StringContent(p.Description), "Description");
            formdata.Add(new StringContent(p.Status.ToString()), "Status");
            formdata.Add(new StringContent(p.Quantity.ToString()), "Quantity");
            formdata.Add(new StringContent(p.UnitInStock.ToString()), "UnitInStock");

            // Gửi yêu cầu POST đến API với dữ liệu sản phẩm
            var result = await client.PostAsync("/api/Product/create-product", formdata);

            // Kiểm tra kết quả và xử lý
            if (result.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList
                return RedirectToAction("ProductList");
            }
            else
            {
                // Xử lý lỗi nếu cần thiết
                // Ví dụ: hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo sản phẩm.");
                return View(p); // Hoặc redirect đến một trang khác tùy theo logic của ứng dụng
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            client.BaseAddress = new Uri(domain);

            string datajson1 = await client.GetStringAsync("/api/Category");
            List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson1);
            ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");

            var datajson = await client.GetStringAsync("/api/Product/search-product-byId/" + id);

            ProductModels p = JsonConvert.DeserializeObject<ProductModels>(datajson);
            //List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
            //ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Deleted(int id)
        {

            // Thiết lập địa chỉ cơ sở cho HttpClient
            client.BaseAddress = new Uri(domain);

            // Gửi yêu cầu DELETE đến API để xóa sản phẩm với productId đã cho
            var response = await client.DeleteAsync("/api/Product/delete-product/" + id);

            // Kiểm tra kết quả của yêu cầu
            if (response.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList hoặc thực hiện hành động khác tùy thuộc vào logic ứng dụng của bạn
                return RedirectToAction("ProductList");
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
        //public async Task<IActionResult> Edit()
        //{
        //    client.BaseAddress = new Uri(domain);
        //    string datajson = await client.GetStringAsync("/api/Category");
        //    List<CategoryModels> cate = JsonConvert.DeserializeObject<List<CategoryModels>>(datajson);
        //    ViewBag.CateId = new SelectList(cate, "CateId", "CategoryName");
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SearchSP(string keyword)
        {

            if (keyword == null)
                return RedirectToAction("ProductList");
            else
            {
                // Thiết lập địa chỉ cơ sở cho HttpClient
                client.BaseAddress = new Uri(domain);

                // Gửi yêu cầu DELETE đến API để xóa sản phẩm với productId đã cho
                var datajson = await client.GetStringAsync("/api/Product/search-product/" + keyword);

                List<ProductModels> products = JsonConvert.DeserializeObject<List<ProductModels>>(datajson);

                return View(products);
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
        public async Task<IActionResult> Edit(ProductModels c, int id)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            // Thêm các trường dữ liệu khác vào form data
            formdata.Add(new StringContent(c.ProductId.ToString()), "ProductId");
            formdata.Add(new StringContent(c.CateId.ToString()), "CateId");
            formdata.Add(new StringContent(c.ProductName), "ProductName");
            formdata.Add(new StringContent(c.UnitPrice.ToString()), "UnitPrice");
            formdata.Add(new StringContent(c.Description), "Description");
            formdata.Add(new StringContent(c.Status.ToString()), "Status");
            formdata.Add(new StringContent(c.Quantity.ToString()), "Quantity");
            formdata.Add(new StringContent(c.UnitInStock.ToString()), "UnitInStock");




            // Gửi yêu cầu POST đến API với dữ liệu sản phẩm
            var result = await client.PutAsync("/api/Product/update-product/" + id, formdata);

            // Kiểm tra kết quả và xử lý
            if (result.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng đến trang ProductList
                return RedirectToAction("ProductList");
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
