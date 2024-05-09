using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingStationery.Models;
using ShoppingStationery.Service;

namespace ShoppingStationery.Controllers
{
	[AuthorizeUser]
	public class DeNghiSCController : Controller

	{
		public StationeryShoppingContext _db;
		private readonly PhieuSuaChuaService phieuSuaChuaService;

		public DeNghiSCController(StationeryShoppingContext db)
        {
			_db = db;
            phieuSuaChuaService=new PhieuSuaChuaService(db);
        }
		[UserAuthorization(1, 2, 3,6)]
		public IActionResult Index()
		{
			var listPhieuDNSC = _db.PhieuDeNghiScs.Include(x => x.MaNdNavigation).ToList();
			return View(listPhieuDNSC);
		}
		public IActionResult Details(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}
			var listChiTietPhieuDN = _db.ChiTietDeNghiScs.Where(x => x.MaDnsc == id).ToList();
			ViewBag.listPhieuDN = _db.PhieuDeNghiScs.Include(x => x.MaNdNavigation).Where(x => x.MaDnsc == id).FirstOrDefault();
			return View(listChiTietPhieuDN);
		}
		public IActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phieuDNSC = _db.PhieuDeNghiScs.FirstOrDefault(x => x.MaDnsc == id);
			if(phieuDNSC.TrangThai == "Đang xem xét")
			{
                phieuDNSC.TrangThai = "Trưởng khoa đã duyệt";
            }
			else if(phieuDNSC.TrangThai == "Trưởng khoa đã duyệt")
			{
				phieuDNSC.TrangThai = "Trưởng phòng CSVC đã duyệt";
			}else if(phieuDNSC.TrangThai == "Trưởng phòng CSVC đã duyệt")
			{
				phieuDNSC.TrangThai = "Ban giám hiệu đã duyệt";
			}
			_db.SaveChanges();
			if (phieuDNSC.TrangThai == "Ban giám hiệu đã duyệt")
				phieuSuaChuaService.createPhieuSuaChua(phieuDNSC);
            return RedirectToAction("Index");
        }
		public IActionResult Cancel(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var phieuDNSC = _db.PhieuDeNghiScs.FirstOrDefault(x => x.MaDnsc == id);
			phieuDNSC.TrangThai = "Không thông qua";
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
    }
}
