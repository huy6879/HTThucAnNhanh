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
    public class BranchesController : Controller
    {
        string domain = "https://localhost:7187/";
        HttpClient client = new HttpClient();
        
        [HttpGet]
        public async Task<IActionResult> ListBranch(int page = 1)
        {
            client.BaseAddress = new Uri(domain);
            var datajson = await client.GetStringAsync("/api/branch");
            List<BranchModels> branch = JsonConvert.DeserializeObject<List<BranchModels>>(datajson);
            return View(branch);
        } 
        // Tao chi nhanh moi
        public async Task<IActionResult> Create()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/branch/");
            List<BranchModels> branch = JsonConvert.DeserializeObject<List<BranchModels>>(datajson);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchModels b)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            formdata.Add(new StringContent(b.BranchId.ToString()), "BranchId");
            formdata.Add(new StringContent(b.BranchName), "BranchName");
            formdata.Add(new StringContent(b.BranchCity), "BranchCity");
            formdata.Add(new StringContent(b.BranchDistrict.ToString()), "BranchDistrict");
            formdata.Add(new StringContent(b.BranchAddress), "BranchAddress");
            var result = await client.PostAsync("/api/branch/create-branch", formdata);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ListBranch");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi tạo chi nhánh.");
                return View(b); 
            }
        }

        //Update chi nhanh
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("/api/branch");
            List<BranchModels> branch = JsonConvert.DeserializeObject<List<BranchModels>>(datajson);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BranchModels b, int id)
        {
            client.BaseAddress = new Uri(domain);

            var formdata = new MultipartFormDataContent();

            formdata.Add(new StringContent(b.BranchId.ToString()), "BranchId");
            formdata.Add(new StringContent(b.BranchName), "BranchName");
            formdata.Add(new StringContent(b.BranchCity), "BranchCity");
            formdata.Add(new StringContent(b.BranchDistrict.ToString()), "BranchDistrict");
            formdata.Add(new StringContent(b.BranchAddress), "BranchAddress");


            var result = await client.PutAsync("/api/branch/update-branch/" + id, formdata);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ListBranch");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi chỉnh sửa sản phẩm.");
                return View(b);
            }
        }

        //Xoa chi nhanh
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            client.BaseAddress = new Uri(domain);

            string datajson = await client.GetStringAsync("/api/branch/get-by-id/" + id);

            BranchModels p = JsonConvert.DeserializeObject<BranchModels>(datajson);

            return View(p);

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Deleted(int id)
        {

            client.BaseAddress = new Uri(domain);

            var response = await client.DeleteAsync("/api/branch/delete-branch/" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListBranch");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Xảy ra lỗi khi xóa chi nhánh.");
               
                return RedirectToAction("Delete");
            }
        }

    }
}
