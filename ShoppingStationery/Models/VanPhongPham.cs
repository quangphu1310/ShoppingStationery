using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class VanPhongPham
{
    public int MaVpp { get; set; }

    public string? TenVpp { get; set; }

    public string? DonViTinh { get; set; }

    public decimal? DonGia { get; set; }

    public string? TenNcc { get; set; }

    public string? Sdtncc { get; set; }

    public int? SoLuong { get; set; }

    public virtual ICollection<ChiTietDeNghiM> ChiTietDeNghiMs { get; set; } = new List<ChiTietDeNghiM>();

    public virtual ICollection<ChiTietPhieuMh> ChiTietPhieuMhs { get; set; } = new List<ChiTietPhieuMh>();
}
