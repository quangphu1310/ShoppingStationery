using System;
using System.Collections;
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
    public class ChiTietPhieuScsController : Controller
    {
        private readonly StationeryShoppingContext _context;
        private readonly PhieuSuaChuaService phieuSuaChua;

        public ChiTietPhieuScsController(StationeryShoppingContext context)
        {
            _context = context;
            phieuSuaChua=new PhieuSuaChuaService(context);
        }

        // GET: ChiTietPhieuScs/IndexByPhieu/5
        public async Task<IActionResult> IndexByPhieu(int? id)
        {
            ViewBag.PhieuId = id;
            var stationeryShoppingContext = _context.ChiTietPhieuScs.Where(ct => ct.MaPhieuSc.Equals(id)).Include(c => c.MaPhieuScNavigation).Include(c => c.MaTbNavigation);
            return View(await stationeryShoppingContext.ToListAsync());
        }

       
        public IActionResult Create(int id)
        {
            ViewBag.MaPhieuSc = id;
            var list=_context.ChiTietPhieuScs.ToList().Where(ct=>ct.MaPhieuSc==id).Select(ct => ct.MaTb)
                   .ToList();
            var tbs = _context.ThietBis.Where(ct => !list.Contains(ct.MaTb));
            if(tbs.Any())
                ViewData["MaTb"] = new SelectList(tbs, "MaTb", "TenTb");
            else
                ViewData["MaTb"] = new SelectList(new ArrayList(), "MaTb", "TenTb");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhieuSc,MaTb,NoiDung,ChiPhi,GhiChu")] ChiTietPhieuSc chiTietPhieuSc)
        {
            _context.Add(chiTietPhieuSc);
            _context.SaveChanges();
            var lis = _context.ChiTietPhieuScs;
            _context.PhieuSuaChuas.Where(t => t.MaPhieuSc == chiTietPhieuSc.MaPhieuSc).First().TongGiaTri = lis.Where(t => t.MaPhieuSc == chiTietPhieuSc.MaPhieuSc).ToList()
                .Sum(t => t.ChiPhi);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexByPhieu", "ChiTietPhieuScs", new { id = chiTietPhieuSc.MaPhieuSc });
        }


  
        public async Task<IActionResult> Delete(int imaTB, int maPSC)
        {
            var chiTietPhieuSc = await _context.ChiTietPhieuScs
                .Where(a => a.MaPhieuSc.Equals(maPSC) && a.MaTb.Equals(imaTB))
                .FirstOrDefaultAsync();

            if (chiTietPhieuSc != null)
            {
                _context.ChiTietPhieuScs.Remove(chiTietPhieuSc);
                await _context.SaveChangesAsync();
                var lis = _context.ChiTietPhieuScs;
                _context.PhieuSuaChuas.Where(t => t.MaPhieuSc == maPSC).First().TongGiaTri = lis.Where(t => t.MaPhieuSc == maPSC).ToList()
                    .Sum(t => t.ChiPhi);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("IndexByPhieu", "ChiTietPhieuScs", new { id = maPSC });

        }

        private bool ChiTietPhieuScExists(int id)
        {
            return _context.ChiTietPhieuScs.Any(e => e.MaPhieuSc == id);
        }
    }
}
