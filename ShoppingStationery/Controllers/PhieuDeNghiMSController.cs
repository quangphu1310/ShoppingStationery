using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{
    [AuthorizeUser]
    public class PhieuDeNghiMSController : Controller
    {
        private readonly StationeryShoppingContext _context;

        public PhieuDeNghiMSController(StationeryShoppingContext context)
        {
            _context = context;
        }

        public IActionResult TimKiemPhieuDNMS(string searchTerm)
        {
            return RedirectToAction("Index", new { searchTerm });
        }

        // GET: PhieuDeNghiMS
        public async Task<IActionResult> Index(string searchTerm)
        {
            var userString = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userString);

            IQueryable<PhieuDeNghiM> danhSachPhieu = _context.PhieuDeNghiMs.Where(phieu => phieu.Mand == currentUser.MaNd);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachPhieu = danhSachPhieu.Where(phieu =>
                    phieu.YkienCsvc.Contains(searchTerm) ||
                    phieu.TrangThai.Contains(searchTerm) ||
                    phieu.TongSoLoai.ToString().Contains(searchTerm));
                TempData["SearchTerm"] = searchTerm;
            }

            var danhSachPhieuContext = await danhSachPhieu.Select(phieu => new PhieuDeNghiM
            {
                MaDnms = phieu.MaDnms,
                YkienCsvc = phieu.YkienCsvc,
                TrangThai = phieu.TrangThai,
                TongSoLoai = phieu.TongSoLoai,
            }).ToListAsync();
            return View(danhSachPhieuContext);
        }

        // GET: PhieuDeNghiMS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiM = await _context.PhieuDeNghiMs
                .Include(p => p.MandNavigation)
                .FirstOrDefaultAsync(m => m.MaDnms == id);
            if (phieuDeNghiM == null)
            {
                return NotFound();
            }

            return View(phieuDeNghiM);
        }

        // GET: PhieuDeNghiMS/Create
        public IActionResult Create()
        {
            ViewData["MaNd"] = new SelectList(_context.NguoiDungs, "MaNd", "MaNd");
            return View();
        }

        // POST: PhieuDeNghiMS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("YkienCsvc,TrangThai,TongSoLoai")] PhieuDeNghiM phieuDeNghiM)
        {
            if (ModelState.IsValid)
            {
                int maxId = _context.PhieuDeNghiMs.Max(p => p.MaDnms);
                int newId = maxId + 1;
                phieuDeNghiM.MaDnms = newId;

                var userJson = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(userJson))
                {
                    return NotFound();
                }

                var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userJson);

                var userInfo = await _context.NguoiDungs
                    .Include(u => u.MaCvNavigation)
                    .Include(u => u.MaDvNavigation)
                    .FirstOrDefaultAsync(u => u.MaNd == currentUser.MaNd);
                phieuDeNghiM.Mand = userInfo.MaNd;

                _context.Add(phieuDeNghiM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phieuDeNghiM);
        }

        // GET: PhieuDeNghiMS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiM = await _context.PhieuDeNghiMs.FindAsync(id);
            if (phieuDeNghiM == null)
            {
                return NotFound();
            }
            return View(phieuDeNghiM);
        }

        // POST: PhieuDeNghiMS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDnms,Mand,YkienCsvc,TrangThai,TongSoLoai")] PhieuDeNghiM phieuDeNghiM)
        {
            if (id != phieuDeNghiM.MaDnms)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuDeNghiM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuDeNghiMExists(phieuDeNghiM.MaDnms))
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
            return View(phieuDeNghiM);
        }

        // GET: PhieuDeNghiMS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiM = await _context.PhieuDeNghiMs
                .Include(p => p.MandNavigation)
                .FirstOrDefaultAsync(m => m.MaDnms == id);
            if (phieuDeNghiM == null)
            {
                return NotFound();
            }
            return View(phieuDeNghiM);
        }

        // POST: PhieuDeNghiMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuDeNghiM = await _context.PhieuDeNghiMs.FindAsync(id);
            if (phieuDeNghiM == null)
            {
                return NotFound();
            }
            var hasChiTietPhieu = _context.ChiTietDeNghiMs.Any(ct => ct.MaDnms == id);
            if (hasChiTietPhieu)
            {
                TempData["ErrorMessage"] = "Không thể xóa phiếu này vui lòng kiểm tra chi tiết phiếu đề nghị.";
                return RedirectToAction(nameof(Delete));
            }
            _context.PhieuDeNghiMs.Remove(phieuDeNghiM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhieuDeNghiMExists(int id)
        {
            return _context.PhieuDeNghiMs.Any(e => e.MaDnms == id);
        }
    }
}
