using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ChiTietDeNghiSc
{
    public int MaDnsc { get; set; }

    public int MaTb { get; set; }

    public string? Dvt { get; set; }

    public int? SoLuong { get; set; }

    public string? LyDo { get; set; }

    public virtual PhieuDeNghiSc MaDnscNavigation { get; set; } = null!;

    public virtual ThietBi MaTbNavigation { get; set; } = null!;
}
