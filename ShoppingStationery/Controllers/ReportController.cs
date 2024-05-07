using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{
	public class ReportController : Controller
	{
		public StationeryShoppingContext _db;
		public ReportController(StationeryShoppingContext db)
		{
			_db = db;
		}
		//[HttpPost]
		public IActionResult Index(string? loai, int? donVi, DateOnly? dateStart, DateOnly? dateEnd)
		{
			decimal? sumCostMH = 0;
			decimal? sumCostSC = 0;
			foreach( var item in _db.PhieuMuaHangs)
			{
				sumCostMH += item.TongGiaTri;
			}
			foreach( var item in _db.PhieuSuaChuas)
			{
                sumCostSC += item.TongGiaTri;
			}
 
            ViewBag.listDV = _db.DonVis.ToList();
            ViewBag.SumCost = sumCostMH + sumCostSC;
			ViewBag.sumCostMH = sumCostMH;
			ViewBag.sumCostSC = sumCostSC;
			ViewBag.loai = loai;

			List<PhieuMuaHang> list1 = new List<PhieuMuaHang>();
			List<PhieuSuaChua> list2 = new List<PhieuSuaChua>();
            if(dateStart == null)
			{
                dateStart = new DateOnly(2000, 1, 1);
            }
            if (dateEnd == null)
                dateEnd = new DateOnly(2030, 1, 1);
			if(donVi == null)
			{
				list1 = _db.PhieuMuaHangs.Include(x => x.MaDvNavigation)
                                        .Where(x => x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                        .ToList();
				list2 = _db.PhieuSuaChuas.Include(x => x.MaDvNavigation)
                                                 .Where(x => x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                                 .ToList();
            }
			else {
                list1 = _db.PhieuMuaHangs.Include(x => x.MaDvNavigation)
                                        .Where(x => x.MaDv == donVi && x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                        .ToList();

                list2 = _db.PhieuSuaChuas.Include(x => x.MaDvNavigation)
                                                     .Where(x => x.MaDv == donVi && x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                                     .ToList();
            }
            ViewBag.list1 = list1;
            ViewBag.list2 = list2;

            return View();
		}
        public IActionResult Print()
		{
			//comming soon
            return Ok();
		}
	}
}
