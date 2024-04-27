using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ThietBi
{
    public int MaTb { get; set; }

    public string? TenTb { get; set; }

    public string? LoaiTb { get; set; }

    public int? NamSuDung { get; set; }

    public int? MaDv { get; set; }

    public virtual ICollection<ChiTietDeNghiSc> ChiTietDeNghiScs { get; set; } = new List<ChiTietDeNghiSc>();

    public virtual ICollection<ChiTietPhieuSc> ChiTietPhieuScs { get; set; } = new List<ChiTietPhieuSc>();

    public virtual DonVi? MaDvNavigation { get; set; }
}
