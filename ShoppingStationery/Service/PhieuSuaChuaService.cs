using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingStationery.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using ShoppingStationery.Models;
using Microsoft.EntityFrameworkCore;

namespace ShoppingStationery.Service
{
    public class PhieuSuaChuaService
    {
        private StationeryShoppingContext context;

        public PhieuSuaChuaService(StationeryShoppingContext context)
        {
            this.context = context;
        }

        public int createPhieuSuaChua(string ghiChu, string maDV, String user)
        {
            Random random = new Random();
            int randomId = random.Next();
            while (PhieuExists(randomId))
            {
                randomId = random.Next();
            }
            PhieuSuaChua phieuSuaChua = new PhieuSuaChua(randomId, DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), 0, "Chưa hoàn thành", ghiChu, Int32.Parse(user), Int32.Parse(maDV));
            context.PhieuSuaChuas.Add(phieuSuaChua);
            context.SaveChanges();
            return randomId;

        }

        public int createPhieuSuaChua(string ghiChu, string maDV, String user, String tt)
        {
            Random random = new Random();
            int randomId = random.Next();
            while (PhieuExists(randomId))
            {
                randomId = random.Next();
            }
            PhieuSuaChua phieuSuaChua = new PhieuSuaChua(randomId, DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), 0, tt, ghiChu, Int32.Parse(user), Int32.Parse(maDV));
            context.PhieuSuaChuas.Add(phieuSuaChua);
            context.SaveChanges();
            return randomId;

        }

        private bool PhieuExists(int randomId)
        {
            return context.PhieuSuaChuas.Any(a => a.MaPhieuSc.Equals(randomId));
        }

        internal async Task addChiTietAsync(ChiTietPhieuSc chiTietPhieuSc)
        {
            context.Add(chiTietPhieuSc);
            await context.SaveChangesAsync();
        }

        internal void createPhieuSuaChua(PhieuDeNghiSc phieuDNSC)
        {
            var nd = context.NguoiDungs.Where(nd => nd.MaNd == phieuDNSC.MaNd).First();
            var phieuSCID = createPhieuSuaChua(phieuDNSC.YkienCsvc, nd.MaDv.ToString(), "2", "Đợi khởi tạo");
            var lsCT = context.ChiTietDeNghiScs.Where(ct => ct.MaDnsc == phieuDNSC.MaDnsc).ToList();
            foreach (var l in lsCT)
            {
                var tb = context.ThietBis.Where(t => t.MaTb == l.MaTb).FirstOrDefault();
                var ct = new ChiTietPhieuSc(phieuSCID, l.MaTb, tb.TenTb, 100000, phieuDNSC.YkienCsvc);
                context.Add(ct);

            }
            context.SaveChanges();

            context.PhieuSuaChuas.Where(phi => phi.MaPhieuSc == phieuSCID).First().TongGiaTri =
                context.ChiTietPhieuScs.Where(phi => phi.MaPhieuSc == phieuSCID).Sum(phi => phi.ChiPhi);
            context.SaveChanges();
        }
    }
}
