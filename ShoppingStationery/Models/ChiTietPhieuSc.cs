using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ChiTietPhieuSc
{
    public int MaPhieuSc { get; set; }

    public int MaTb { get; set; }

    public string? NoiDung { get; set; }

    public decimal? ChiPhi { get; set; }

    public string? GhiChu { get; set; }

    public virtual PhieuSuaChua MaPhieuScNavigation { get; set; } = null!;

    public virtual ThietBi MaTbNavigation { get; set; } = null!;

    public ChiTietPhieuSc(int maPhieuSc, int maTb, string? noiDung, decimal? chiPhi, string? ghiChu)
    {
        MaPhieuSc = maPhieuSc;
        MaTb = maTb;
        NoiDung = noiDung;
        ChiPhi = chiPhi;
        GhiChu = ghiChu;
    }

    public ChiTietPhieuSc()
    {
       
    }
}
