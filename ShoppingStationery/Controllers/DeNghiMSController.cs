using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingStationery.Models;
using ShoppingStationery.Service;

namespace ShoppingStationery.Controllers
{
    [AuthorizeUser]
    public class DeNghiMSController : Controller

    {
        public StationeryShoppingContext _db;
        private readonly PhieuMuaHangServie phieuMuaHangServie;

        public DeNghiMSController(StationeryShoppingContext db)
        {
            _db = db;
            phieuMuaHangServie = new PhieuMuaHangServie(db);
        }
        [UserAuthorization(1, 2, 3, 6)]
        public IActionResult Index()
        {
            var listPhieuDNSC = _db.PhieuDeNghiMs.Include(x => x.MandNavigation).ToList();
            return View(listPhieuDNSC);
        }
        public IActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var listChiTietPhieuDN = _db.ChiTietDeNghiMs.Where(x => x.MaDnms == id).ToList();
            ViewBag.listPhieuDN = _db.PhieuDeNghiMs.Include(x => x.MandNavigation).Where(x => x.MaDnms == id).FirstOrDefault();
            return View(listChiTietPhieuDN);
        }
        public IActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phieuDNSC = _db.PhieuDeNghiMs.FirstOrDefault(x => x.MaDnms == id);
            if (phieuDNSC.TrangThai == "Đang xem xét")
            {
                phieuDNSC.TrangThai = "Trưởng khoa đã duyệt";
            }
            else if (phieuDNSC.TrangThai == "Trưởng khoa đã duyệt")
            {
                phieuDNSC.TrangThai = "Trưởng phòng CSVC đã duyệt";
            }
            else if (phieuDNSC.TrangThai == "Trưởng phòng CSVC đã duyệt")
            {
                phieuDNSC.TrangThai = "Ban giám hiệu đã duyệt";
            }
            _db.SaveChanges();
            if (phieuDNSC.TrangThai == "Ban giám hiệu đã duyệt")
                phieuMuaHangServie.createWaitedPhieuMuaHang(phieuDNSC.MaDnms);
            return RedirectToAction("Index");
        }
        public IActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phieuDNSC = _db.PhieuDeNghiMs.FirstOrDefault(x => x.MaDnms == id);
            phieuDNSC.TrangThai = "Không thông qua";
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
