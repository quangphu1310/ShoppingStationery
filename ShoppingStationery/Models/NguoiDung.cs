using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class NguoiDung
{
    public int MaNd { get; set; }

    public string? HoTen { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public int? MaCv { get; set; }

    public int? MaDv { get; set; }

    public int? MaTk { get; set; }

    public virtual ChucVu? MaCvNavigation { get; set; }

    public virtual DonVi? MaDvNavigation { get; set; }

    public virtual TaiKhoan? MaTkNavigation { get; set; }

    public virtual ICollection<PhieuDeNghiM> PhieuDeNghiMs { get; set; } = new List<PhieuDeNghiM>();

    public virtual ICollection<PhieuDeNghiSc> PhieuDeNghiScs { get; set; } = new List<PhieuDeNghiSc>();

    public virtual ICollection<PhieuMuaHang> PhieuMuaHangs { get; set; } = new List<PhieuMuaHang>();

    public virtual ICollection<PhieuSuaChua> PhieuSuaChuas { get; set; } = new List<PhieuSuaChua>();
}
