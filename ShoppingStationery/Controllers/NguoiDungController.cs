using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{

	[AuthorizeUser]
    [UserAuthorization(6)]
    public class NguoiDungController : Controller
    {
        private readonly StationeryShoppingContext _context;

        public NguoiDungController(StationeryShoppingContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Infor()
        {
            // Lấy thông tin người dùng từ session
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return NotFound();
            }

            var currentUser = JsonConvert.DeserializeObject<NguoiDung>(userJson);

            // Truy vấn thông tin người dùng từ cơ sở dữ liệu
            var userInfo = await _context.NguoiDungs
                .Include(u => u.MaCvNavigation)
                .Include(u => u.MaDvNavigation)
                .FirstOrDefaultAsync(u => u.MaNd == currentUser.MaNd);

            if (userInfo == null)
            {
                return NotFound();
            }

            var viewModel = new NguoiDungViewModel
            {
                MaNd = userInfo.MaNd,
                HoTen = userInfo.HoTen,
                Sdt = userInfo.Sdt,
                Email = userInfo.Email,
                TaiKhoan = userInfo.TaiKhoan,
                MatKhau = userInfo.MatKhau,
                TenCV = userInfo.MaCvNavigation.TenCv,
                TenD = userInfo.MaDvNavigation.TenD
            };

            return View(viewModel);
        }


        [HttpPost]
		public IActionResult TimKiemNguoiDung(string searchTerm)
		{
			return RedirectToAction("Index", new { searchTerm });
		}
		public async Task<IActionResult> Index(string searchTerm)
		{
			IQueryable<NguoiDung> danhSachNguoiDung = _context.NguoiDungs;

			if (!string.IsNullOrEmpty(searchTerm))
			{
				danhSachNguoiDung = danhSachNguoiDung.Where(nd => nd.HoTen.Contains(searchTerm) ||
																  nd.Sdt.Contains(searchTerm) ||
																  nd.Email.Contains(searchTerm) ||
																  nd.TaiKhoan.Contains(searchTerm));
				TempData["SearchTerm"] = searchTerm; // Lưu trữ từ khóa tìm kiếm vào TempData
			}

			var stationeryShoppingContext = await danhSachNguoiDung.Select(nd => new NguoiDungViewModel
			{
				MaNd = nd.MaNd,
				HoTen = nd.HoTen,
				Sdt = nd.Sdt,
				Email = nd.Email,
				TaiKhoan = nd.TaiKhoan,
				MatKhau = nd.MatKhau,
				TenCV = nd.MaCvNavigation.TenCv,
				TenD = nd.MaDvNavigation.TenD
			}).ToListAsync();

			return View(stationeryShoppingContext);
		}

		// GET: NguoiDung/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .Select(nd => new NguoiDungViewModel
                {
                    MaNd = nd.MaNd,
                    HoTen = nd.HoTen,
                    Sdt = nd.Sdt,
                    Email = nd.Email,
                    TaiKhoan = nd.TaiKhoan,
                    MatKhau = nd.MatKhau,
                    TenCV = nd.MaCvNavigation.TenCv,
                    TenD = nd.MaDvNavigation.TenD
                })
                .FirstOrDefaultAsync(m => m.MaNd == id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        		
		// GET: NguoiDung/Create
		public IActionResult Create()
        {
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "TenD");
            return View();
        }
		
		// POST: NguoiDung/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
		
		public async Task<IActionResult> Create([Bind("HoTen,Sdt,Email,MaCv,MaDv,TaiKhoan,MatKhau")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "MaCv", nguoiDung.MaCv);
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv", nguoiDung.MaDv);
            return View(nguoiDung);
        }

        // GET: NguoiDung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv", nguoiDung.MaCv);
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "TenD", nguoiDung.MaDv);
            return View(nguoiDung);
        }

        // POST: NguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNd,HoTen,Sdt,Email,MaCv,MaDv,TaiKhoan,MatKhau")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.MaNd))
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
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "MaCv", nguoiDung.MaCv);
            ViewData["MaDv"] = new SelectList(_context.DonVis, "MaDv", "MaDv", nguoiDung.MaDv);
            return View(nguoiDung);
        }

        // GET: NguoiDung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
               .Select(nd => new NguoiDungViewModel
               {
                   MaNd = nd.MaNd,
                   HoTen = nd.HoTen,
                   Sdt = nd.Sdt,
                   Email = nd.Email,
                   TaiKhoan = nd.TaiKhoan,
                   MatKhau = nd.MatKhau,
                   TenCV = nd.MaCvNavigation.TenCv,
                   TenD = nd.MaDvNavigation.TenD
               })
               .FirstOrDefaultAsync(m => m.MaNd == id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung != null)
            {
                _context.NguoiDungs.Remove(nguoiDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


		private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNd == id);
        }
    }
}
