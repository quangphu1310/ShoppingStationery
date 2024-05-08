using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{
    public class ChiTietDeNghiMsController : Controller
    {
        private readonly StationeryShoppingContext _context;

        public ChiTietDeNghiMsController(StationeryShoppingContext context)
        {
            _context = context;
        }

        public IActionResult TimKiemCTPhieuDNMS(int idMaDnms, string searchTerm)
        {
            return RedirectToAction("Index", new { idMaDnms, searchTerm });
        }


        // GET: ChiTietDeNghiMs
        public async Task<IActionResult> Index(int? idMaDnms, string searchTerm)
        {
            TempData["IdMaDnms"] = idMaDnms;
            var userString = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userString);

            IQueryable<ChiTietDeNghiM> danhSachCTPhieu = _context.ChiTietDeNghiMs.Where(CTphieu => CTphieu.MaDnmsNavigation.Mand == currentUser.MaNd && CTphieu.MaDnms == idMaDnms);
            // Thực hiện tìm kiếm nếu có từ khóa tìm kiếm được cung cấp
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachCTPhieu = danhSachCTPhieu.Where(CTphieu =>
                    CTphieu.MaVppNavigation.TenVpp.Contains(searchTerm) ||
                    CTphieu.Dvt.Contains(searchTerm) ||
                    CTphieu.SoLuong.ToString().Contains(searchTerm) ||
                    CTphieu.Lydo.ToString().Contains(searchTerm));
                TempData["SearchTerm"] = searchTerm;
            }

            var danhSachCTPhieuContext = await danhSachCTPhieu.Select(CTphieu => new ChiTietDeNghiM
            {
                MaDnms = CTphieu.MaDnms,
                MaVpp = CTphieu.MaVpp,
                Dvt = CTphieu.Dvt,
                SoLuong = CTphieu.SoLuong,
                Lydo = CTphieu.Lydo,
                MaDnmsNavigation = CTphieu.MaDnmsNavigation,
                MaVppNavigation = CTphieu.MaVppNavigation
            }).ToListAsync();

            return View(danhSachCTPhieuContext);
        }

        // GET: ChiTietDeNghiMs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDeNghiM = await _context.ChiTietDeNghiMs
                .Include(c => c.MaDnmsNavigation)
                .Include(c => c.MaVppNavigation)
                .FirstOrDefaultAsync(m => m.MaDnms == id);
            if (chiTietDeNghiM == null)
            {
                return NotFound();
            }

            return View(chiTietDeNghiM);
        }

        // GET: ChiTietDeNghiMs/Create
        public IActionResult Create(int? idMaDnms)
        {
            TempData["IdMaDnms"] = idMaDnms;
            ViewData["MaDnms"] = new SelectList(_context.PhieuDeNghiMs, "MaDnms", "MaDnms");
            ViewData["MaVpp"] = new SelectList(_context.VanPhongPhams, "MaVpp", "TenVpp");
            return View();
        }

        public IActionResult GetDvt(int maVpp)
        {
            string dvt = _context.VanPhongPhams.FirstOrDefault(vpp => vpp.MaVpp == maVpp)?.DonViTinh;
            return Json(dvt);
        }

        // POST: ChiTietDeNghiMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idMaDnms, [Bind("MaVpp,Dvt,SoLuong,Lydo")] ChiTietDeNghiM chiTietDeNghiM)
        {
            Console.WriteLine("MaDNMS: " + idMaDnms);
            if (ModelState.IsValid)
            {
                var existingChiTiet = await _context.ChiTietDeNghiMs
                .SingleOrDefaultAsync(c => c.MaDnms == idMaDnms && c.MaVpp == chiTietDeNghiM.MaVpp);

                if (existingChiTiet != null)
                {
                    if (existingChiTiet.Lydo != chiTietDeNghiM.Lydo)
                    {
                        existingChiTiet.Lydo += ", " + chiTietDeNghiM.Lydo;
                    }
                    existingChiTiet.SoLuong += chiTietDeNghiM.SoLuong;
                }
                else
                {
                    chiTietDeNghiM.MaDnms = idMaDnms;
                    _context.Add(chiTietDeNghiM);
                }
                await _context.SaveChangesAsync();

                var numberOfLines = await _context.ChiTietDeNghiMs
                    .Where(c => c.MaDnms == idMaDnms)
                    .CountAsync();

                var phieuDeNghi = await _context.PhieuDeNghiMs.FindAsync(idMaDnms);
                if (phieuDeNghi != null)
                {
                    phieuDeNghi.TongSoLoai = numberOfLines;
                    _context.Update(phieuDeNghi);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { idMaDnms });
            }
            ViewData["MaDnms"] = new SelectList(_context.PhieuDeNghiMs, "MaDnms", "MaDnms", chiTietDeNghiM.MaDnms);
            ViewData["MaVpp"] = new SelectList(_context.VanPhongPhams, "MaVpp", "TenVpp", chiTietDeNghiM.MaVpp);
            return View(chiTietDeNghiM);
        }

        // GET: ChiTietDeNghiMs/Edit/5
        public async Task<IActionResult> Edit(int maDnms, int maVpp)
        {
            var chiTietDeNghiM = await _context.ChiTietDeNghiMs.FindAsync(maDnms, maVpp);
            if (chiTietDeNghiM == null)
            {
                return NotFound();
            }
            TempData["IdMaDnms"] = maDnms;
            var tenVpp = _context.VanPhongPhams
                                .Where(vpp => vpp.MaVpp == maVpp)
                                .Select(vpp => vpp.TenVpp)
                                .FirstOrDefault();
            TempData["TenVpp"] = tenVpp;
            ViewData["MaDnms"] = new SelectList(_context.PhieuDeNghiMs, "MaDnms", "MaDnms", chiTietDeNghiM.MaDnms);
            ViewData["MaVpp"] = new SelectList(_context.VanPhongPhams, "MaVpp", "TenVpp", chiTietDeNghiM.MaVpp);
            return View(chiTietDeNghiM);
        }

        // POST: ChiTietDeNghiMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maDnms, int maVpp, [Bind("MaDnms,MaVpp,Dvt,SoLuong,Lydo")] ChiTietDeNghiM chiTietDeNghiM)
        {
            int idMaDnms = maDnms;
            if (maDnms != chiTietDeNghiM.MaDnms || maVpp != chiTietDeNghiM.MaVpp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDeNghiM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDeNghiMExists(chiTietDeNghiM.MaDnms) || !ChiTietDeNghiMExists(chiTietDeNghiM.MaVpp))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { idMaDnms });
            }
            ViewData["MaDnms"] = new SelectList(_context.PhieuDeNghiMs, "MaDnms", "MaDnms", chiTietDeNghiM.MaDnms);
            ViewData["MaVpp"] = new SelectList(_context.VanPhongPhams, "MaVpp", "MaVpp", chiTietDeNghiM.MaVpp);
            return View(chiTietDeNghiM);
        }


        // GET: ChiTietDeNghiMs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDeNghiM = await _context.ChiTietDeNghiMs
                .Include(c => c.MaDnmsNavigation)
                .Include(c => c.MaVppNavigation)
                .FirstOrDefaultAsync(m => m.MaDnms == id);
            if (chiTietDeNghiM == null)
            {
                return NotFound();
            }
            TempData["IdMaDnms"] = id;
            return View(chiTietDeNghiM);
        }

        // POST: ChiTietDeNghiMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maDnms, int maVpp)
        {
            int idMaDnms = maDnms;
            var chiTietDeNghiM = await _context.ChiTietDeNghiMs.FindAsync(maDnms, maVpp);
            if (chiTietDeNghiM != null)
            {
                _context.ChiTietDeNghiMs.Remove(chiTietDeNghiM);
                await _context.SaveChangesAsync();

                var numberOfLines = await _context.ChiTietDeNghiMs
                    .Where(c => c.MaDnms == idMaDnms)
                    .CountAsync();

                var phieuDeNghi = await _context.PhieuDeNghiMs.FindAsync(idMaDnms);
                if (phieuDeNghi != null)
                {
                    phieuDeNghi.TongSoLoai = numberOfLines;
                    _context.Update(phieuDeNghi);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { idMaDnms });
        }

        private bool ChiTietDeNghiMExists(int id)
        {
            return _context.ChiTietDeNghiMs.Any(e => e.MaDnms == id);
        }
    }
}
