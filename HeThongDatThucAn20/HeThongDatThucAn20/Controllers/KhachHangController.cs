using AutoMapper;
using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.Helpers;
using HeThongDatThucAn20.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;



namespace HeThongDatThucAn20.Controllers
{
    public class KhachHangController : Controller
    {
        public readonly HeThongDatDoAnContext db;
        private readonly IMapper _mapper;

        public KhachHangController(HeThongDatDoAnContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                if (model.Password == model.Repass)
                {
                    try
                    {
                        var khachhang = _mapper.Map<Account>(model);
                        khachhang.RoleId = 3;
                        khachhang.RandomKey = ChuanHoa.GenerateRandomKey();
                        khachhang.Password = model.Password.ToMd5Hash(khachhang.RandomKey);

                        db.Add(khachhang);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        var mess = $"{ex.Message} shh";
                    }
                }
            }    
            return View();
        }

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl; 
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if(ModelState.IsValid)
            {
                var khachhang = db.Accounts.SingleOrDefault(kh =>
                kh.Email == model.UserName);
                if(khachhang == null)
                {
                    ModelState.AddModelError("Lỗi", "Không có khách hàng này");
                }    
                else
                {  
                    if(khachhang.Password != model.PassWord.ToMd5Hash(khachhang.RandomKey))
                    {
                        ModelState.AddModelError("Lỗi", "Sai thông tin đăng nhập");
                    }    
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.Fullname),
                            new Claim(ClaimTypes.StreetAddress,khachhang.Address),
                            new Claim(ClaimTypes.MobilePhone,khachhang.Phone),
                            new Claim(ClaimTypes.Email,khachhang.Email),
                            new Claim(ClaimTypes.Role,"Customer"),
                        };
                        var claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        if(Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return Redirect("/");
                        }    
                    }    
                }    
            }    
            return View();
        }

        //public IActionResult
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
