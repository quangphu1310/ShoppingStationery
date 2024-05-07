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
    public class ChiTietPhieuMhsController : Controller
    {
        private readonly StationeryShoppingContext _context;
        private readonly PhieuMuaHangServie phieuMuaHangServie;

        public ChiTietPhieuMhsController(StationeryShoppingContext context)
        {
            _context = context;
            phieuMuaHangServie = new PhieuMuaHangServie(context);
        }

        // GET: ChiTietPhieuMhs/IndexByPhieu
        public async Task<IActionResult> IndexByPhieu(int? id)
        {
            ViewBag.phieuId = id;
            var stationeryShoppingContext = _context.ChiTietPhieuMhs.Where(a => a.MaPhieuMh.Equals(id)).Include(c => c.MaPhieuMhNavigation).Include(c => c.MaVppNavigation);
            return View(await stationeryShoppingContext.ToListAsync());
        }

        public IActionResult Create(int? id)
        {
            ViewBag.phieuId = id;
            var existedVpps=_context.ChiTietPhieuMhs.Where(a => a.MaPhieuMh.Equals(id))
                .Select(a=>a.MaVpp).ToList();
            ViewData["MaVpp"] = new SelectList(_context.VanPhongPhams.Where(a=>!existedVpps.Contains(a.MaVpp)), "MaVpp", "TenVpp");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhieuMh,MaVpp,SoLuong,DonGia,GhiChu")] ChiTietPhieuMh chiTietPhieuMh)
        {
            _context.Add(chiTietPhieuMh);
            await _context.SaveChangesAsync();
            _context.PhieuMuaHangs.Where(a => a.MaPhieuMh.Equals(chiTietPhieuMh.MaPhieuMh)).First().TongGiaTri
                = _context.ChiTietPhieuMhs.Where(a => a.MaPhieuMh == chiTietPhieuMh.MaPhieuMh).Sum(a => a.DonGia * a.SoLuong);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexByPhieu", "ChiTietPhieuMhs", new { id = chiTietPhieuMh.MaPhieuMh });


        }


        public async Task<IActionResult> Delete(int maVPP, int maPhieu)
        {
            var chiTietPhieuMh = _context.ChiTietPhieuMhs.Where(n => n.MaPhieuMh.Equals(maPhieu) && n.MaVpp.Equals(maVPP)).FirstOrDefault();
            if (chiTietPhieuMh != null)
            {
                _context.ChiTietPhieuMhs.Remove(chiTietPhieuMh);
                await _context.SaveChangesAsync();
                _context.PhieuMuaHangs.Where(a => a.MaPhieuMh.Equals(chiTietPhieuMh.MaPhieuMh)).First().TongGiaTri
                    = _context.ChiTietPhieuMhs.Where(a => a.MaPhieuMh == chiTietPhieuMh.MaPhieuMh).Sum(a => a.DonGia * a.SoLuong);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexByPhieu", "ChiTietPhieuMhs", new { id = maPhieu });
            }
           
            return RedirectToAction("IndexByPhieu", "ChiTietPhieuMhs", new { id = maPhieu });
        }

        private bool ChiTietPhieuMhExists(int id)
        {
            return _context.ChiTietPhieuMhs.Any(e => e.MaPhieuMh == id);
        }
    }
}
