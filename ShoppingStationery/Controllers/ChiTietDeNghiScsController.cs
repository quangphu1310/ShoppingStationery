using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{
    public class ChiTietDeNghiScsController : Controller
    {
        private readonly StationeryShoppingContext _context;

        public ChiTietDeNghiScsController(StationeryShoppingContext context)
        {
            _context = context;
        }

        public IActionResult TimKiemCTPhieuDNSC(int idMaDnsc, string searchTerm)
        {
            return RedirectToAction("Index", new { idMaDnsc, searchTerm });
        }

        // GET: ChiTietDeNghiScs
        public async Task<IActionResult> Index(int? idMaDnsc, string searchTerm)
        {
            TempData["idMaDnsc"] = idMaDnsc;
            var userString = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userString);

            IQueryable<ChiTietDeNghiSc> danhSachCTPhieu = _context.ChiTietDeNghiScs.Where(CTphieu => CTphieu.MaDnscNavigation.MaNd == currentUser.MaNd && CTphieu.MaDnsc == idMaDnsc);
            // Thực hiện tìm kiếm nếu có từ khóa tìm kiếm được cung cấp
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachCTPhieu = danhSachCTPhieu.Where(CTphieu =>
                    CTphieu.MaTbNavigation.TenTb.Contains(searchTerm) ||
                    CTphieu.Dvt.Contains(searchTerm) ||
                    CTphieu.SoLuong.ToString().Contains(searchTerm) ||
                    CTphieu.LyDo.ToString().Contains(searchTerm));
                TempData["SearchTerm"] = searchTerm;
            }

            var danhSachCTPhieuContext = await danhSachCTPhieu.Select(CTphieu => new ChiTietDeNghiSc
            {
                MaDnsc = CTphieu.MaDnsc,
                MaTb = CTphieu.MaTb,
                Dvt = CTphieu.Dvt,
                SoLuong = CTphieu.SoLuong,
                LyDo = CTphieu.LyDo,
                MaDnscNavigation = CTphieu.MaDnscNavigation,
                MaTbNavigation = CTphieu.MaTbNavigation
            }).ToListAsync();

            return View(danhSachCTPhieuContext);
        }

        // GET: ChiTietDeNghiScs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDeNghiSc = await _context.ChiTietDeNghiScs
                .Include(c => c.MaDnscNavigation)
                .Include(c => c.MaTbNavigation)
                .FirstOrDefaultAsync(m => m.MaDnsc == id);
            if (chiTietDeNghiSc == null)
            {
                return NotFound();
            }

            return View(chiTietDeNghiSc);
        }

        // GET: ChiTietDeNghiScs/Create
        public IActionResult Create(int? idMaDnsc)
        {
            TempData["IdMaDnsc"] = idMaDnsc;
            ViewData["MaDnsc"] = new SelectList(_context.PhieuDeNghiScs, "MaDnsc", "MaDnsc");
            ViewData["MaTb"] = new SelectList(_context.ThietBis, "MaTb", "TenTb");
            return View();
        }

        // POST: ChiTietDeNghiScs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idMaDnsc, [Bind("MaTb,Dvt,SoLuong,LyDo")] ChiTietDeNghiSc chiTietDeNghiSc)
        {
            if (ModelState.IsValid)
            {
                var existingChiTiet = await _context.ChiTietDeNghiScs
                .SingleOrDefaultAsync(c => c.MaDnsc == idMaDnsc && c.MaTb == chiTietDeNghiSc.MaTb);

                if (existingChiTiet != null)
                {
                    if (existingChiTiet.LyDo != chiTietDeNghiSc.LyDo)
                    {
                        existingChiTiet.LyDo += ", " + chiTietDeNghiSc.LyDo;
                    }
                    existingChiTiet.SoLuong += chiTietDeNghiSc.SoLuong;
                }
                else
                {
                    chiTietDeNghiSc.MaDnsc = idMaDnsc;
                    _context.Add(chiTietDeNghiSc);
                }
                await _context.SaveChangesAsync();

                var numberOfLines = await _context.ChiTietDeNghiMs
                    .Where(c => c.MaDnms == idMaDnsc)
                    .CountAsync();

                var phieuDeNghi = await _context.PhieuDeNghiMs.FindAsync(idMaDnsc);
                if (phieuDeNghi != null)
                {
                    phieuDeNghi.TongSoLoai = numberOfLines;
                    _context.Update(phieuDeNghi);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { idMaDnsc });
            }
            ViewData["MaDnsc"] = new SelectList(_context.PhieuDeNghiScs, "MaDnsc", "MaDnsc", chiTietDeNghiSc.MaDnsc);
            ViewData["MaTb"] = new SelectList(_context.ThietBis, "MaTb", "TenTb", chiTietDeNghiSc.MaTb);
            return View(chiTietDeNghiSc);
        }

        // GET: ChiTietDeNghiScs/Edit/5
        public async Task<IActionResult> Edit(int maDnsc, int maTb)
        {
            var chiTietDeNghiSc = await _context.ChiTietDeNghiScs.FindAsync(maDnsc, maTb);
            if (chiTietDeNghiSc == null)
            {
                return NotFound();
            }
            TempData["IdMaDnsc"] = maDnsc;
            var tenTb = _context.ThietBis
                                .Where(tb => tb.MaTb == maTb)
                                .Select(tb => tb.TenTb)
                                .FirstOrDefault();
            TempData["TenTb"] = tenTb;
            ViewData["MaDnsc"] = new SelectList(_context.PhieuDeNghiScs, "MaDnsc", "MaDnsc", chiTietDeNghiSc.MaDnsc);
            ViewData["MaTb"] = new SelectList(_context.ThietBis, "MaTb", "MaTb", chiTietDeNghiSc.MaTb);
            return View(chiTietDeNghiSc);
        }

        // POST: ChiTietDeNghiScs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maDnsc, int maTb, [Bind("MaDnsc,MaTb,Dvt,SoLuong,LyDo")] ChiTietDeNghiSc chiTietDeNghiSc)
        {
            int idMaDnsc = maDnsc;
            if (maDnsc != chiTietDeNghiSc.MaDnsc || maTb != chiTietDeNghiSc.MaTb)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDeNghiSc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDeNghiScExists(chiTietDeNghiSc.MaDnsc) || !ChiTietDeNghiScExists(chiTietDeNghiSc.MaTb))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { idMaDnsc });
            }
            ViewData["MaDnsc"] = new SelectList(_context.PhieuDeNghiScs, "MaDnsc", "MaDnsc", chiTietDeNghiSc.MaDnsc);
            ViewData["MaTb"] = new SelectList(_context.ThietBis, "MaTb", "MaTb", chiTietDeNghiSc.MaTb);
            return View(chiTietDeNghiSc);
        }

        // GET: ChiTietDeNghiScs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDeNghiSc = await _context.ChiTietDeNghiScs
                .Include(c => c.MaDnscNavigation)
                .Include(c => c.MaTbNavigation)
                .FirstOrDefaultAsync(m => m.MaDnsc == id);
            if (chiTietDeNghiSc == null)
            {
                return NotFound();
            }
            TempData["IdMaDnsc"] = id;
            return View(chiTietDeNghiSc);
        }

        // POST: ChiTietDeNghiScs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maDnsc, int maTb)
        {
            int idMaDnsc = maDnsc;
            var chiTietDeNghiSc = await _context.ChiTietDeNghiMs.FindAsync(maDnsc, maTb);
            if (chiTietDeNghiSc != null)
            {
                _context.ChiTietDeNghiMs.Remove(chiTietDeNghiSc);
                await _context.SaveChangesAsync();

                var numberOfLines = await _context.ChiTietDeNghiMs
                    .Where(c => c.MaDnms == idMaDnsc)
                    .CountAsync();

                var phieuDeNghi = await _context.PhieuDeNghiMs.FindAsync(idMaDnsc);
                if (phieuDeNghi != null)
                {
                    phieuDeNghi.TongSoLoai = numberOfLines;
                    _context.Update(phieuDeNghi);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { idMaDnsc });
        }

        private bool ChiTietDeNghiScExists(int id)
        {
            return _context.ChiTietDeNghiScs.Any(e => e.MaDnsc == id);
        }
    }
}
