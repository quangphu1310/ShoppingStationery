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
    [AuthorizeUser]
    public class PhieuDeNghiScsController : Controller
    {
        private readonly StationeryShoppingContext _context;

        public PhieuDeNghiScsController(StationeryShoppingContext context)
        {
            _context = context;
        }

        public IActionResult TimKiemPhieuDNSC(string searchTerm)
        {
            return RedirectToAction("Index", new { searchTerm });
        }

        // GET: PhieuDeNghiScs
        public async Task<IActionResult> Index(string searchTerm)
        {
            var userString = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userString);

            IQueryable<PhieuDeNghiSc> danhSachPhieu = _context.PhieuDeNghiScs.Where(phieu => phieu.MaNd == currentUser.MaNd);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachPhieu = danhSachPhieu.Where(phieu =>
                    phieu.YkienCsvc.Contains(searchTerm) ||
                    phieu.TrangThai.Contains(searchTerm) ||
                    phieu.TongSoLoai.ToString().Contains(searchTerm));
                TempData["SearchTerm"] = searchTerm;
            }

            var danhSachPhieuContext = await danhSachPhieu.Select(phieu => new PhieuDeNghiSc
            {
                MaDnsc = phieu.MaDnsc,
                YkienCsvc = phieu.YkienCsvc,
                NgayDeNghi = phieu.NgayDeNghi,
                TrangThai = phieu.TrangThai,
                TongSoLoai = phieu.TongSoLoai,
            }).ToListAsync();
            return View(danhSachPhieuContext);
        }

        // GET: PhieuDeNghiScs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiSc = await _context.PhieuDeNghiScs
                .Include(p => p.MaNdNavigation)
                .FirstOrDefaultAsync(m => m.MaDnsc == id);
            if (phieuDeNghiSc == null)
            {
                return NotFound();
            }

            return View(phieuDeNghiSc);
        }

        // GET: PhieuDeNghiScs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhieuDeNghiScs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("YkienCsvc,NgayDeNghi,TrangThai,TongSoLoai")] PhieuDeNghiSc phieuDeNghiSc)
        {
            if (ModelState.IsValid)
            {
                int maxId = _context.PhieuDeNghiScs.Max(p => p.MaDnsc);
                int newId = maxId + 1;
                phieuDeNghiSc.MaDnsc = newId;

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
                phieuDeNghiSc.MaNd = userInfo.MaNd;

                _context.Add(phieuDeNghiSc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phieuDeNghiSc);
        }

        // GET: PhieuDeNghiScs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiSc = await _context.PhieuDeNghiScs.FindAsync(id);
            if (phieuDeNghiSc == null)
            {
                return NotFound();
            }
            var trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Đang xem xét", Text = "Đang xem xét" },
                new SelectListItem { Value = "Được phê duyệt", Text = "Được phê duyệt" },
                new SelectListItem { Value = "Không thông qua", Text = "Không thông qua" }
            };
            ViewBag.TrangThai = trangThaiList;
            return View(phieuDeNghiSc);
        }

        // POST: PhieuDeNghiScs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDnsc,MaNd,YkienCsvc,NgayDeNghi,TrangThai,TongSoLoai")] PhieuDeNghiSc phieuDeNghiSc)
        {
            if (id != phieuDeNghiSc.MaDnsc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuDeNghiSc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuDeNghiScExists(phieuDeNghiSc.MaDnsc))
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
            return View(phieuDeNghiSc);
        }

        // GET: PhieuDeNghiScs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDeNghiSc = await _context.PhieuDeNghiScs
                .Include(p => p.MaNdNavigation)
                .FirstOrDefaultAsync(m => m.MaDnsc == id);
            if (phieuDeNghiSc == null)
            {
                return NotFound();
            }

            return View(phieuDeNghiSc);
        }

        // POST: PhieuDeNghiScs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuDeNghiSc = await _context.PhieuDeNghiScs.FindAsync(id);
            if (phieuDeNghiSc == null)
            {
                return NotFound();
            }
            var hasChiTietPhieu = _context.ChiTietDeNghiScs.Any(ct => ct.MaDnsc == id);
            if (hasChiTietPhieu)
            {
                TempData["ErrorMessage"] = "Không thể xóa phiếu này vui lòng kiểm tra chi tiết phiếu đề nghị.";
                return RedirectToAction(nameof(Delete));
            }
            _context.PhieuDeNghiScs.Remove(phieuDeNghiSc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhieuDeNghiScExists(int id)
        {
            return _context.PhieuDeNghiScs.Any(e => e.MaDnsc == id);
        }
    }
}
