using ShoppingStationery.Models;

namespace ShoppingStationery.Dto
{
    public class YeuCauTaoPhieuSuaChua
    {
        public int MaPhieuSc { get; set; }

        public int MaTb { get; set; }

        public string? NoiDung { get; set; }

        public decimal? ChiPhi { get; set; }

        public string? GhiChu { get; set; }
        public virtual PhieuSuaChua MaPhieuScNavigation { get; set; } = null!;

        public virtual ThietBi MaTbNavigation { get; set; } = null!;
    }
}
