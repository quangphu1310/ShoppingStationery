using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ChiTietPhieuMh
{
    public int MaPhieuMh { get; set; }

    public int MaVpp { get; set; }

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public string? GhiChu { get; set; }

    public virtual PhieuMuaHang MaPhieuMhNavigation { get; set; } = null!;

    public virtual VanPhongPham MaVppNavigation { get; set; } = null!;
}
