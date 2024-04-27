using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class PhieuDeNghiM
{
    public int MaDnms { get; set; }

    public int? Mand { get; set; }

    public string? YkienCsvc { get; set; }

    public string? TrangThai { get; set; }

    public int? TongSoLoai { get; set; }

    public virtual ICollection<ChiTietDeNghiM> ChiTietDeNghiMs { get; set; } = new List<ChiTietDeNghiM>();

    public virtual NguoiDung? MandNavigation { get; set; }
}
