﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoppingStationery.Models;

public partial class StationeryShoppingContext : DbContext
{
    public StationeryShoppingContext()
    {
    }

    public StationeryShoppingContext(DbContextOptions<StationeryShoppingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDeNghiM> ChiTietDeNghiMs { get; set; }

    public virtual DbSet<ChiTietDeNghiSc> ChiTietDeNghiScs { get; set; }

    public virtual DbSet<ChiTietPhieuMh> ChiTietPhieuMhs { get; set; }

    public virtual DbSet<ChiTietPhieuSc> ChiTietPhieuScs { get; set; }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<DonVi> DonVis { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhieuDeNghiM> PhieuDeNghiMs { get; set; }

    public virtual DbSet<PhieuDeNghiSc> PhieuDeNghiScs { get; set; }

    public virtual DbSet<PhieuMuaHang> PhieuMuaHangs { get; set; }

    public virtual DbSet<PhieuSuaChua> PhieuSuaChuas { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThietBi> ThietBis { get; set; }

    public virtual DbSet<VanPhongPham> VanPhongPhams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=QUANGPHU\\SQLEXPRESS;Initial Catalog=stationeryShopping;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDeNghiM>(entity =>
        {
            entity.HasKey(e => new { e.MaDnms, e.MaVpp }).HasName("PK__ChiTietD__6FA7F57B69AB86C4");

            entity.ToTable("ChiTietDeNghiMS");

            entity.Property(e => e.MaDnms).HasColumnName("MaDNMS");
            entity.Property(e => e.MaVpp).HasColumnName("MaVPP");
            entity.Property(e => e.Dvt)
                .HasMaxLength(20)
                .HasColumnName("DVT");
            entity.Property(e => e.Lydo)
                .HasMaxLength(255)
                .HasColumnName("lydo");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaDnmsNavigation).WithMany(p => p.ChiTietDeNghiMs)
                .HasForeignKey(d => d.MaDnms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDe__MaDNM__44FF419A");

            entity.HasOne(d => d.MaVppNavigation).WithMany(p => p.ChiTietDeNghiMs)
                .HasForeignKey(d => d.MaVpp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDe__MaVPP__45F365D3");
        });

        modelBuilder.Entity<ChiTietDeNghiSc>(entity =>
        {
            entity.HasKey(e => new { e.MaDnsc, e.MaTb }).HasName("PK__ChiTietD__DEC9777445261277");

            entity.ToTable("ChiTietDeNghiSC");

            entity.Property(e => e.MaDnsc).HasColumnName("MaDNSC");
            entity.Property(e => e.MaTb).HasColumnName("MaTB");
            entity.Property(e => e.Dvt)
                .HasMaxLength(20)
                .HasColumnName("DVT");
            entity.Property(e => e.LyDo).HasMaxLength(255);

            entity.HasOne(d => d.MaDnscNavigation).WithMany(p => p.ChiTietDeNghiScs)
                .HasForeignKey(d => d.MaDnsc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDe__MaDNS__4CA06362");

            entity.HasOne(d => d.MaTbNavigation).WithMany(p => p.ChiTietDeNghiScs)
                .HasForeignKey(d => d.MaTb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDeN__MaTB__4D94879B");
        });

        modelBuilder.Entity<ChiTietPhieuMh>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuMh, e.MaVpp }).HasName("PK__ChiTietP__DB127B142ED80702");

            entity.ToTable("ChiTietPhieuMH", tb => tb.HasTrigger("UpdateTotalPrice_PhieuMuaHang"));

            entity.Property(e => e.MaPhieuMh).HasColumnName("MaPhieuMH");
            entity.Property(e => e.MaVpp).HasColumnName("MaVPP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(500);

            entity.HasOne(d => d.MaPhieuMhNavigation).WithMany(p => p.ChiTietPhieuMhs)
                .HasForeignKey(d => d.MaPhieuMh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__MaPhi__5812160E");

            entity.HasOne(d => d.MaVppNavigation).WithMany(p => p.ChiTietPhieuMhs)
                .HasForeignKey(d => d.MaVpp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__MaVPP__59063A47");
        });

        modelBuilder.Entity<ChiTietPhieuSc>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuSc, e.MaTb }).HasName("PK__ChiTietP__6A7C7FE8933B02D3");

            entity.ToTable("ChiTietPhieuSC");

            entity.Property(e => e.MaPhieuSc).HasColumnName("MaPhieuSC");
            entity.Property(e => e.MaTb).HasColumnName("MaTB");
            entity.Property(e => e.ChiPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.NoiDung).HasMaxLength(500);

            entity.HasOne(d => d.MaPhieuScNavigation).WithMany(p => p.ChiTietPhieuScs)
                .HasForeignKey(d => d.MaPhieuSc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__MaPhi__656C112C");

            entity.HasOne(d => d.MaTbNavigation).WithMany(p => p.ChiTietPhieuScs)
                .HasForeignKey(d => d.MaTb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__MaTB__66603565");
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.MaCv).HasName("PK__ChucVu__27258E76D8B02E61");

            entity.ToTable("ChucVu");

            entity.Property(e => e.MaCv)
                .ValueGeneratedNever()
                .HasColumnName("MaCV");
            entity.Property(e => e.TenCv)
                .HasMaxLength(100)
                .HasColumnName("TenCV");
        });

        modelBuilder.Entity<DonVi>(entity =>
        {
            entity.HasKey(e => e.MaDv).HasName("PK__DonVi__27258657324B7965");

            entity.ToTable("DonVi");

            entity.Property(e => e.MaDv)
                .ValueGeneratedNever()
                .HasColumnName("MaDV");
            entity.Property(e => e.TenD).HasMaxLength(255);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNd).HasName("PK__NguoiDun__2725D724E87C8D78");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.MaNd)
                .ValueGeneratedNever()
                .HasColumnName("MaND");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaCv).HasColumnName("MaCV");
            entity.Property(e => e.MaDv).HasColumnName("MaDV");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaCvNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaCv)
                .HasConstraintName("FK__NguoiDung__MaCV__5FB337D6");

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaDv)
                .HasConstraintName("FK__NguoiDung__MaDV__60A75C0F");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaTk)
                .HasConstraintName("FK__NguoiDung__MaTK__619B8048");
        });

        modelBuilder.Entity<PhieuDeNghiM>(entity =>
        {
            entity.HasKey(e => e.MaDnms).HasName("PK__PhieuDeN__3CBB764370C409FE");

            entity.ToTable("PhieuDeNghiMS");

            entity.Property(e => e.MaDnms)
                .ValueGeneratedNever()
                .HasColumnName("MaDNMS");
            entity.Property(e => e.Mand).HasColumnName("MAND");
            entity.Property(e => e.TrangThai).HasMaxLength(255);
            entity.Property(e => e.YkienCsvc)
                .HasMaxLength(255)
                .HasColumnName("YKienCSVC");

            entity.HasOne(d => d.MandNavigation).WithMany(p => p.PhieuDeNghiMs)
                .HasForeignKey(d => d.Mand)
                .HasConstraintName("FK__PhieuDeNgh__MAND__412EB0B6");
        });

        modelBuilder.Entity<PhieuDeNghiSc>(entity =>
        {
            entity.HasKey(e => e.MaDnsc).HasName("PK__PhieuDeN__3CBB27728BCDA579");

            entity.ToTable("PhieuDeNghiSC");

            entity.Property(e => e.MaDnsc)
                .ValueGeneratedNever()
                .HasColumnName("MaDNSC");
            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.TrangThai).HasMaxLength(255);
            entity.Property(e => e.YkienCsvc)
                .HasMaxLength(255)
                .HasColumnName("YKienCSVC");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.PhieuDeNghiScs)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK__PhieuDeNgh__MaND__48CFD27E");
        });

        modelBuilder.Entity<PhieuMuaHang>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMh).HasName("PK__PhieuMua__880EF82CFB949D4A");

            entity.ToTable("PhieuMuaHang");

            entity.Property(e => e.MaPhieuMh)
                .ValueGeneratedNever()
                .HasColumnName("MaPhieuMH");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.MaDv).HasColumnName("MaDV");
            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.TongGiaTri).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(100);

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.PhieuMuaHangs)
                .HasForeignKey(d => d.MaDv)
                .HasConstraintName("FK__PhieuMuaHa__MaDV__5AEE82B9");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.PhieuMuaHangs)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK__PhieuMuaHa__MaND__59FA5E80");
        });

        modelBuilder.Entity<PhieuSuaChua>(entity =>
        {
            entity.HasKey(e => e.MaPhieuSc).HasName("PK__PhieuSua__880E2FEE57E37DA5");

            entity.ToTable("PhieuSuaChua");

            entity.Property(e => e.MaPhieuSc)
                .ValueGeneratedNever()
                .HasColumnName("MaPhieuSC");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.MaDv).HasColumnName("MaDV");
            entity.Property(e => e.MaNd).HasColumnName("MaND");
            entity.Property(e => e.TongGiaTri).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(100);

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.PhieuSuaChuas)
                .HasForeignKey(d => d.MaDv)
                .HasConstraintName("FK__PhieuSuaCh__MaDV__6477ECF3");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.PhieuSuaChuas)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK__PhieuSuaCh__MaND__6383C8BA");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TaiKhoan__27250070DE732B48");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.MaTk)
                .ValueGeneratedNever()
                .HasColumnName("MaTK");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.TaiKhoan1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TaiKhoan");
        });

        modelBuilder.Entity<ThietBi>(entity =>
        {
            entity.HasKey(e => e.MaTb).HasName("PK__ThietBi__2725006F457F13A6");

            entity.ToTable("ThietBi");

            entity.Property(e => e.MaTb)
                .ValueGeneratedNever()
                .HasColumnName("MaTB");
            entity.Property(e => e.LoaiTb)
                .HasMaxLength(100)
                .HasColumnName("LoaiTB");
            entity.Property(e => e.MaDv).HasColumnName("MaDV");
            entity.Property(e => e.TenTb)
                .HasMaxLength(255)
                .HasColumnName("TenTB");

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.ThietBis)
                .HasForeignKey(d => d.MaDv)
                .HasConstraintName("FK__ThietBi__MaDV__628FA481");
        });

        modelBuilder.Entity<VanPhongPham>(entity =>
        {
            entity.HasKey(e => e.MaVpp).HasName("PK__VanPhong__31C83384F69DC388");

            entity.ToTable("VanPhongPham");

            entity.Property(e => e.MaVpp)
                .ValueGeneratedNever()
                .HasColumnName("MaVPP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonViTinh)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sdtncc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDTNCC");
            entity.Property(e => e.TenNcc)
                .HasMaxLength(255)
                .HasColumnName("TenNCC");
            entity.Property(e => e.TenVpp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TenVPP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
