using Microsoft.AspNetCore.Mvc;
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

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
