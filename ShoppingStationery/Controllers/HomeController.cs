using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingStationery.Models;
using System.Diagnostics;

namespace ShoppingStationery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public StationeryShoppingContext _db;

        public HomeController(ILogger<HomeController> logger, StationeryShoppingContext db)
        {
            _logger = logger;
			_db = db;
		}

        public IActionResult Index()
        {
            //var list = _db.NguoiDungs.ToList();
            //ViewBag.list = list;
			return View();
        }


		[HttpGet]
		public IActionResult Login()
		{
			var user = HttpContext.Session.GetString("User");
			if (user != null)
			{
				return RedirectToAction("Index", "NguoiDung");
			}
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(NguoiDung nd)
		{
			

			var tk = nd.TaiKhoan;
			var mk = nd.MatKhau;
			var userCheck = _db.NguoiDungs.Where(s => s.TaiKhoan == tk && s.MatKhau == mk).FirstOrDefault();

			if (userCheck != null)
			{
				// Lưu thông tin người dùng vào Session
				HttpContext.Session.SetString("User", JsonConvert.SerializeObject(userCheck));
				
				return RedirectToAction("Index", "NguoiDung");
			}
			else
			{
				
				ViewBag.error = "Sai tài khoản hoặc mật khẩu";
				return View("Login");
			}
		}

		[AuthorizeUser]
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("User");
			return RedirectToAction("Login", "Home");
		}
		public IActionResult Forbidden()
		{
			return View("Forbidden");
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
