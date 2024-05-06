using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class PhieuMuaHang
{
    public int MaPhieuMh { get; set; }

    public DateOnly? NgayLap { get; set; }

    public decimal? TongGiaTri { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public int? MaNd { get; set; }

    public int? MaDv { get; set; }

    public virtual ICollection<ChiTietPhieuMh> ChiTietPhieuMhs { get; set; } = new List<ChiTietPhieuMh>();

    public virtual DonVi? MaDvNavigation { get; set; }

    public virtual NguoiDung? MaNdNavigation { get; set; }


    public PhieuMuaHang(int maPhieuMh, DateOnly? ngayLap, decimal? tongGiaTri, string? trangThai, string? ghiChu, int? maNd, int? maDv)
    {
        MaPhieuMh = maPhieuMh;
        NgayLap = ngayLap;
        TongGiaTri = tongGiaTri;
        TrangThai = trangThai;
        GhiChu = ghiChu;
        MaNd = maNd;
        MaDv = maDv;
    }
}
