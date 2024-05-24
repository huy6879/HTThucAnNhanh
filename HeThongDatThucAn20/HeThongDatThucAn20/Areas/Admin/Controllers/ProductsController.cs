using HeThongDatThucAn20.Areas.Admin.Models;
using HeThongDatThucAn20.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;


namespace HeThongDatThucAn20.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        string domain = "https://localhost:7187/";
        HttpClient client = new HttpClient();
        public async Task <IActionResult> ProductList()
        {
            client.BaseAddress = new Uri(domain);
            string datajson =  await client.GetStringAsync("/api/Product");
            List<ProductModels> products = JsonConvert.DeserializeObject<List<ProductModels>>(datajson);
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
        public IActionResult Create(Product p)
        {
            client.BaseAddress = new Uri(domain);
            var formdata = new MultipartFormDataContent();
            formdata.Add(new StringContent(p.ProductName), "productName");
            formdata.Add(new StringContent(p.UnitPrice.ToString()), "unitPrice");
            formdata.Add(new StringContent(p.Description), "descripTion");
            formdata.Add(new StringContent(p.Status.ToString()), "status");
            formdata.Add(new StringContent(p.Quantity.ToString()), "quantity");
            formdata.Add(new StringContent(p.UnitInStock.ToString()), "unitinStock");
            //client.PostAsync()
            return View();
        }

    }
}
