using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class TaiKhoan
{
    public int MaTk { get; set; }

    public string? TaiKhoan1 { get; set; }

    public string? MatKhau { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
