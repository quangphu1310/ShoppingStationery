using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ShoppingStationery.Models;

namespace ShoppingStationery.Service
{
    public class PhieuMuaHangServie
    {
        private StationeryShoppingContext context;

        public PhieuMuaHangServie(StationeryShoppingContext context)
        {
            this.context = context;
        }

        internal int createDefPhieuMuaHang(string? ghiChu, string? maDV, StringValues userID)
        {
            Random random = new Random();

            int randomId = 1;
            while (PhieuMuaHangExists(randomId))
            {
                randomId = random.Next();
            }

            var phieuMuaHang = new PhieuMuaHang(randomId, DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")) , 0, "Chưa thanh toán", ghiChu, Int32.Parse(userID), Int32.Parse(maDV));
            context.PhieuMuaHangs.Add(phieuMuaHang);
            context.SaveChanges();
            return randomId;
            


        }
        internal int createPhieuMuaHang(string? ghiChu, string? maDV, StringValues userID, String trangthai)
        {
            Random random = new Random();

            int randomId = 1;
            while (PhieuMuaHangExists(randomId))
            {
                randomId = random.Next();
            }

            var phieuMuaHang = new PhieuMuaHang(randomId, DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), 0, trangthai, ghiChu, Int32.Parse(userID), Int32.Parse(maDV));
            context.PhieuMuaHangs.Add(phieuMuaHang);
            context.SaveChanges();
            return randomId;



        }

        internal void createWaitedPhieuMuaHang(int id)
        {
            var phieuDNMH=context.PhieuDeNghiMs.Where(p=>p.MaDnms==id).FirstOrDefault();
            var nd = context.NguoiDungs.Where(nd => nd.MaNd == phieuDNMH.Mand).First();
            var phieuMsId = this.createPhieuMuaHang(phieuDNMH.YkienCsvc, nd.MaDv.ToString(),"3", "Đợi khởi tạo");
            addChiTietMuaHang(id, phieuMsId);
        }

        private void addChiTietMuaHang(int? id, int phieuMsId)
        {
            var phieuDNMHChiTiet = context.ChiTietDeNghiMs.Where(p => p.MaDnms == id).ToList();
            foreach(var ct in phieuDNMHChiTiet)
            {
                var cpp = context.VanPhongPhams.Where(vpp => vpp.MaVpp == ct.MaVpp).First();
                var phieuCT = new ChiTietPhieuMh(phieuMsId, ct.MaVpp, ct.SoLuong, cpp.DonGia, "Được tạo tự đồng từ yêu cầu mua sắm được thông qua");
                context.Add(phieuCT);

            }
            context.SaveChanges();
            context.PhieuMuaHangs.Where(p => p.MaPhieuMh == phieuMsId).First().TongGiaTri =
                context.ChiTietPhieuMhs.Where(p => p.MaPhieuMh == phieuMsId).ToList().Sum(p => p.DonGia * p.SoLuong);
            context.SaveChanges(true);
        }

        private bool PhieuMuaHangExists(int id)
        {
            return context.PhieuMuaHangs.Any(e => e.MaPhieuMh == id);
        }
    }
}
