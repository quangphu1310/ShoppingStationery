using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class PhieuSuaChua
{
    public int MaPhieuSc { get; set; }

    public DateOnly? NgayLap { get; set; }

    public decimal? TongGiaTri { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public int? MaNd { get; set; }

    public int? MaDv { get; set; }

    public virtual ICollection<ChiTietPhieuSc> ChiTietPhieuScs { get; set; } = new List<ChiTietPhieuSc>();

    public virtual DonVi? MaDvNavigation { get; set; }

    public virtual NguoiDung? MaNdNavigation { get; set; }
}
