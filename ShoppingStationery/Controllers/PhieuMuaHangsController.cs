using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingStationery.Models;
using ShoppingStationery.Service;

namespace ShoppingStationery.Controllers
{
    [AuthorizeUser]
    public class PhieuMuaHangsController : Controller
    {
        private readonly StationeryShoppingContext _context;
        private readonly PhieuMuaHangServie phieuMuaHangServie;

        public PhieuMuaHangsController(StationeryShoppingContext context)
        {
            _context = context;
            phieuMuaHangServie = new PhieuMuaHangServie(context);
        }

        // GET: PhieuMuaHangs
        public async Task<IActionResult> Index()
        {

            ViewData["DVDV"] = new SelectList(_context.DonVis, "MaDv", "TenD");

            var stationeryShoppingContext = _context.PhieuMuaHangs.Include(p => p.MaDvNavigation).Include(p => p.MaNdNavigation);
            return View(await stationeryShoppingContext.ToListAsync());
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(IFormCollection form)
        {
            string ghiChu = form["GhiChu"];
            string maDV = form["MaDv"];
            var UserID = form["UserID"];

            int id = phieuMuaHangServie.createDefPhieuMuaHang(ghiChu, maDV, UserID);
            ViewBag.phieuSuaChuaId = id;
            return RedirectToAction(nameof(Index));
        }



        private bool PhieuMuaHangExists(int id)
        {
            return _context.PhieuMuaHangs.Any(e => e.MaPhieuMh == id);
        }


        public async Task<IActionResult> ThanhToan(int id)
        {
            _context.PhieuMuaHangs.Where(a => a.MaPhieuMh == id).First()
                .TrangThai = "Đã thanh toán";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Huy(int id)
        {
            _context.PhieuMuaHangs.Where(a => a.MaPhieuMh == id).First()
               .TrangThai = "Đã bị huỹ";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> TaoPhieu(int id)
        {
            _context.PhieuMuaHangs.Where(a => a.MaPhieuMh == id).First()
               .TrangThai = "Chưa thanh toán";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}
