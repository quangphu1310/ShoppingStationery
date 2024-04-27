using System;
using System.Collections.Generic;

namespace ShoppingStationery.Models;

public partial class ChucVu
{
    public int MaCv { get; set; }

    public string? TenCv { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
