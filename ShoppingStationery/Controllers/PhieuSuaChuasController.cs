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
    public class PhieuSuaChuasController : Controller
    {
        private readonly PhieuSuaChuaService phieuSuaChuaService;
        private readonly StationeryShoppingContext _context;

        public PhieuSuaChuasController(StationeryShoppingContext context)
        {
            _context = context;
            phieuSuaChuaService = new PhieuSuaChuaService(context);
        }

        // GET: PhieuSuaChuas
        public async Task<IActionResult> Index()
        {
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv");
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd");
            ViewData["DVDV"] = new SelectList(_context.DonVis.ToList(), "MaDv", "TenD");

            
            var stationeryShoppingContext = _context.PhieuSuaChuas.Include(p => p.MaDvNavigation).Include(p => p.MaNdNavigation);
            return View(await stationeryShoppingContext.ToListAsync());
        }

        // GET: PhieuSuaChuas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuSuaChua = await _context.PhieuSuaChuas
                .Include(p => p.MaDvNavigation)
                .Include(p => p.MaNdNavigation)
                .FirstOrDefaultAsync(m => m.MaPhieuSc == id);
            if (phieuSuaChua == null)
            {
                return NotFound();
            }

            return View(phieuSuaChua);
        }

        // GET: PhieuSuaChuas/Create
        public IActionResult Create()
        {
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv");
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd");
            return View();
        }

        public IActionResult CreateNew(IFormCollection form)
        {
            string ghiChu = form["GhiChu"];
            string maDV = form["MaDv"];
            var UserID = form["UserID"];

            int id = phieuSuaChuaService.createPhieuSuaChua(ghiChu, maDV, UserID);
            ViewBag.phieuSuaChuaId = id;
            return RedirectToAction(nameof(Index));
        }
        

        // POST: PhieuSuaChuas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhieuSc,NgayLap,TongGiaTri,TrangThai,GhiChu,MaNd,MaDv")] PhieuSuaChua phieuSuaChua)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phieuSuaChua);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv", phieuSuaChua.MaDv);
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd", phieuSuaChua.MaNd);
            return View(phieuSuaChua);
        }

        // GET: PhieuSuaChuas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuSuaChua = await _context.PhieuSuaChuas.FindAsync(id);
            if (phieuSuaChua == null)
            {
                return NotFound();
            }
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv", phieuSuaChua.MaDv);
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd", phieuSuaChua.MaNd);
            return View(phieuSuaChua);
        }

        // POST: PhieuSuaChuas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhieuSc,NgayLap,TongGiaTri,TrangThai,GhiChu,MaNd,MaDv")] PhieuSuaChua phieuSuaChua)
        {
            if (id != phieuSuaChua.MaPhieuSc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuSuaChua);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuSuaChuaExists(phieuSuaChua.MaPhieuSc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv", phieuSuaChua.MaDv);
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd", phieuSuaChua.MaNd);
            return View(phieuSuaChua);
        }

        // GET: PhieuSuaChuas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuSuaChua = await _context.PhieuSuaChuas
                .Include(p => p.MaDvNavigation)
                .Include(p => p.MaNdNavigation)
                .FirstOrDefaultAsync(m => m.MaPhieuSc == id);
            if (phieuSuaChua == null)
            {
                return NotFound();
            }

            return View(phieuSuaChua);
        }

        // POST: PhieuSuaChuas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuSuaChua = await _context.PhieuSuaChuas.FindAsync(id);
            if (phieuSuaChua != null)
            {
                _context.PhieuSuaChuas.Remove(phieuSuaChua);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhieuSuaChuaExists(int id)
        {
            return _context.PhieuSuaChuas.Any(e => e.MaPhieuSc == id);
        }



        public async Task<IActionResult> HoanThanh(int id)
        {
            _context.PhieuSuaChuas.Where(p => p.MaPhieuSc == id).First()
                .TrangThai = "Đã hoàn thành";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Huy(int id)
        {
            _context.PhieuSuaChuas.Where(p => p.MaPhieuSc == id).First()
                .TrangThai = "Đã bị huỹ";
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
