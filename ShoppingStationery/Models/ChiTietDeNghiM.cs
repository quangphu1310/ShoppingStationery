using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ChiTietDeNghiM
{
    public int MaDnms { get; set; }

    public int MaVpp { get; set; }

    public string? Dvt { get; set; }

    public int? SoLuong { get; set; }

    public string? Lydo { get; set; }

    public virtual PhieuDeNghiM? MaDnmsNavigation { get; set; }

    public virtual VanPhongPham? MaVppNavigation { get; set; }
}
