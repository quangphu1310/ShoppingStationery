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
            Console.WriteLine(user);
            Random random = new Random();
            int randomId = random.Next();
            PhieuSuaChua phieuSuaChua = new PhieuSuaChua(randomId, DateOnly.Parse(DateTime.Now.ToString()), 0, "Chưa hoàn thành", ghiChu, Int32.Parse(user), Int32.Parse(maDV));
            context.PhieuSuaChuas.Add(phieuSuaChua);
            context.SaveChanges();
            return randomId;


            return 0;
        }

        internal async Task addChiTietAsync(ChiTietPhieuSc chiTietPhieuSc)
        {
            context.Add(chiTietPhieuSc);
            await context.SaveChangesAsync();
        }
    }
}
