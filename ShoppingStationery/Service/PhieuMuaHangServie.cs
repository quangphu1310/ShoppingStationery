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

        internal int createPhieuMuaHang(string? ghiChu, string? maDV, StringValues userID)
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
        private bool PhieuMuaHangExists(int id)
        {
            return context.PhieuMuaHangs.Any(e => e.MaPhieuMh == id);
        }
    }
}
