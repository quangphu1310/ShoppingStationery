using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class DonVi
{
    public int MaDv { get; set; }

    public string? TenD { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();

    public virtual ICollection<PhieuMuaHang> PhieuMuaHangs { get; set; } = new List<PhieuMuaHang>();

    public virtual ICollection<PhieuSuaChua> PhieuSuaChuas { get; set; } = new List<PhieuSuaChua>();

    public virtual ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
}
