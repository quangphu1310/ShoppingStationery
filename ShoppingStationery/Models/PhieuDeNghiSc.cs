using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class PhieuDeNghiSc
{
    public int MaDnsc { get; set; }

    public int? MaNd { get; set; }

    public string? YkienCsvc { get; set; }

    public DateOnly? NgayDeNghi { get; set; }

    public string? TrangThai { get; set; }

    public int? TongSoLoai { get; set; }

    public virtual ICollection<ChiTietDeNghiSc> ChiTietDeNghiScs { get; set; } = new List<ChiTietDeNghiSc>();

    public virtual NguoiDung? MaNdNavigation { get; set; }
}
