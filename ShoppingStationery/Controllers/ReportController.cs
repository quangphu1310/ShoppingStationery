using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingStationery.Models;
using System.Data;
using System;
using ClosedXML.Excel;

namespace ShoppingStationery.Controllers
{
    [AuthorizeUser]
    public class ReportController : Controller
	{
		public StationeryShoppingContext _db;
		public ReportController(StationeryShoppingContext db)
		{
			_db = db;
		}
        //[HttpPost]
        public static List<PhieuMuaHang> list1 = new List<PhieuMuaHang>();
        public static List<PhieuSuaChua> list2 = new List<PhieuSuaChua>();
        static decimal? sumCostMH = 0;
        static decimal? sumCostSC = 0;
        public IActionResult Index(string? loai, int? donVi, DateOnly? dateStart, DateOnly? dateEnd)
		{
			
			foreach( var item in _db.PhieuMuaHangs)
			{
				sumCostMH += item.TongGiaTri;
			}
			foreach( var item in _db.PhieuSuaChuas)
			{
                sumCostSC += item.TongGiaTri;
			}
 
            ViewBag.listDV = _db.DonVis.ToList();
            ViewBag.SumCost = sumCostMH + sumCostSC;
			ViewBag.sumCostMH = sumCostMH;
			ViewBag.sumCostSC = sumCostSC;
			ViewBag.loai = loai;

			
            if(dateStart == null)
			{
                dateStart = new DateOnly(2000, 1, 1);
            }
            if (dateEnd == null)
                dateEnd = new DateOnly(2030, 1, 1);
			if(donVi == null)
			{
				list1 = _db.PhieuMuaHangs.Include(x => x.MaDvNavigation)
                                        .Where(x => x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                        .ToList();
				list2 = _db.PhieuSuaChuas.Include(x => x.MaDvNavigation)
                                                 .Where(x => x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                                 .ToList();
            }
			else {
                list1 = _db.PhieuMuaHangs.Include(x => x.MaDvNavigation)
                                        .Where(x => x.MaDv == donVi && x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                        .ToList();

                list2 = _db.PhieuSuaChuas.Include(x => x.MaDvNavigation)
                                                     .Where(x => x.MaDv == donVi && x.NgayLap >= dateStart && x.NgayLap <= dateEnd)
                                                     .ToList();
            }
            ViewBag.list1 = list1;
            ViewBag.list2 = list2;

            return View();
		}

        [HttpGet]
        public async Task<FileResult> Print()
        {
            var people = await _db.PhieuMuaHangs.ToListAsync();
            var fileName = "baocaochiphi.xlsx";
            return GenerateExcel(fileName, people);
        }

        private FileResult GenerateExcel(string fileName, IEnumerable<PhieuMuaHang> people)
        {
            DataTable dataTable = new DataTable("Report");
            
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Mã phiếu"),
                new DataColumn("Ngày lập"),
                new DataColumn("Trạng thái"),
                new DataColumn("Ghi chú"),
                new DataColumn("Đơn vị sử dụng"),
                new DataColumn("Tổng giá trị")
            });

            foreach (var person in list1)
            {
                dataTable.Rows.Add(person.MaPhieuMh, person.NgayLap, person.TrangThai, person.GhiChu, person.MaDvNavigation.TenD, person.TongGiaTri);
            }
            dataTable.Rows.Add("Tổng", "", "", "", "", sumCostMH);
            foreach (var person in list2)
            {
                dataTable.Rows.Add(person.MaPhieuSc, person.NgayLap, person.TrangThai, person.GhiChu, person.MaDvNavigation.TenD, person.TongGiaTri);
            }
            dataTable.Rows.Add("Tổng", "", "", "", "", sumCostSC);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }

        }
    }
}
