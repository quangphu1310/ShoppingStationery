create database stationeryShopping
go
--use master
--drop database stationeryShopping
--go
use stationeryShopping
go

CREATE TABLE [VanPhongPham] (
  [MaVPP] int PRIMARY KEY,
  [TenVPP] nvarchar(255),
  [DonViTinh] nvarchar(50),
  [DonGia] decimal(18,2),
  [TenNCC] nvarchar(255),
  [SDTNCC] varchar(20),
  SoLuong int
)
GO

CREATE TABLE [ChiTietPhieuMH] (
  [MaPhieuMH] int,
  [MaVPP] int,
  [SoLuong] int,
  [DonGia] decimal(18,2),
  [GhiChu] nvarchar(500),
  PRIMARY KEY ([MaPhieuMH], [MaVPP])
)
GO

CREATE TABLE [PhieuMuaHang] (
  [MaPhieuMH] int PRIMARY KEY,
  [NgayLap] date,
  [TongGiaTri] decimal(18,2),
  [TrangThai] nvarchar(100),
  [GhiChu] nvarchar(500),
  [MaND] int,
  [MaDV] int
)
GO
CREATE TABLE [NguoiDung] (
  [MaND] int identity(1,1) PRIMARY KEY,
  [HoTen] nvarchar(100),
  [SDT] varchar(20),
  [Email] varchar(100),
  [MaCV] int,
  [MaDV] int,
	TaiKhoan varchar(100),
	MatKhau varchar(100)
)
GO

CREATE TABLE [ThietBi] (
  [MaTB] int PRIMARY KEY,
  [TenTB] nvarchar(255),
  [LoaiTB] nvarchar(100),
  [NamSuDung] int,
  [MaDV] int
)
GO
CREATE TABLE PhieuDeNghiMS (
  MaDNMS INT PRIMARY KEY,
  MAND INT FOREIGN KEY REFERENCES NguoiDung(MaND),
  YKienCSVC NVARCHAR(255),
  --TrangThai NVARCHAR(255) CHECK (TrangThai IN (N'Đang xem xét', N'Được phê duyệt', N'Không thông qua')),
  TrangThai NVARCHAR(255) CHECK (TrangThai IN (N'Trưởng khoa đã duyệt', N'Trưởng phòng CSVC đã duyệt', N'Ban giám hiệu đã duyệt',N'Không thông qua',N'Đang xem xét')),
  TongSoLoai int
);
CREATE TABLE ChiTietDeNghiMS (
  MaDNMS INT FOREIGN KEY REFERENCES PhieuDeNghiMS(MaDNMS),
  MaVPP INT FOREIGN KEY REFERENCES VanPhongPham(MaVPP),
  DVT nvarchar(20),
  soLuong int,
  lydo Nvarchar(255) ,
    PRIMARY KEY (MaDNMS, MaVPP)
 
);
 

 CREATE TABLE PhieuDeNghiSC (
  MaDNSC INT PRIMARY KEY,
  MaND INT FOREIGN KEY REFERENCES NguoiDung(MaND),
  YKienCSVC Nvarchar(255),
  NgayDeNghi DATE,
  --TrangThai NVARCHAR(255) CHECK (TrangThai IN (N'Đang xem xét', N'Được phê duyệt', N'Không thông qua')),
  TrangThai NVARCHAR(255) CHECK (TrangThai IN (N'Trưởng khoa đã duyệt', N'Trưởng phòng CSVC đã duyệt', N'Ban giám hiệu đã duyệt',N'Không thông qua',N'Đang xem xét')),

  TongSoLoai int
);
 
CREATE TABLE ChiTietDeNghiSC (
  MaDNSC INT FOREIGN KEY REFERENCES PhieuDeNghiSC(MaDNSC),
  MaTB INT,
  DVT nvarchar(20),
  SoLuong INT,
  LyDo Nvarchar(255),
  FOREIGN KEY (MaTB) REFERENCES ThietBi(MaTB),
  PRIMARY KEY (MaDNSC, MaTB)
 
);
 
--CREATE TABLE [TaiKhoan] (
--  [MaTK] int PRIMARY KEY,
--  [TaiKhoan] varchar(100),
--  [MatKhau] varchar(100),
--  [Role] nvarchar(50)
--)
GO

CREATE TABLE [ChucVu] (
  [MaCV] int PRIMARY KEY,
  [TenCV] nvarchar(100)
)
GO

CREATE TABLE [PhieuSuaChua] (
  [MaPhieuSC] int PRIMARY KEY,
  [NgayLap] date,
  [TongGiaTri] decimal(18,2),
  [TrangThai] nvarchar(100),
  [GhiChu] nvarchar(500),
  [MaND] int,
  [MaDV] int
)
GO

CREATE TABLE [ChiTietPhieuSC] (
  [MaPhieuSC] int,
  [MaTB] int,
  [NoiDung] nvarchar(500),
  [ChiPhi] decimal(18,2),
  [GhiChu] nvarchar(500),
  PRIMARY KEY ([MaPhieuSC], [MaTB])
)
GO

CREATE TABLE [DonVi] (
  [MaDV] int PRIMARY KEY,
  [TenD] nvarchar(255)
)
GO

ALTER TABLE [ChiTietPhieuMH] ADD FOREIGN KEY ([MaPhieuMH]) REFERENCES [PhieuMuaHang] ([MaPhieuMH])
GO

ALTER TABLE [ChiTietPhieuMH] ADD FOREIGN KEY ([MaVPP]) REFERENCES [VanPhongPham] ([MaVPP])
GO

ALTER TABLE [PhieuMuaHang] ADD FOREIGN KEY ([MaND]) REFERENCES [NguoiDung] ([MaND])
GO

ALTER TABLE [PhieuMuaHang] ADD FOREIGN KEY ([MaDV]) REFERENCES [DonVi] ([MaDV])
GO

ALTER TABLE [ChiTietDeNghiMS] ADD FOREIGN KEY ([MaVPP]) REFERENCES [VanPhongPham] ([MaVPP])
GO

ALTER TABLE [PhieuDeNghiMS] ADD FOREIGN KEY ([MaND]) REFERENCES [NguoiDung] ([MaND])
GO

ALTER TABLE [ChiTietDeNghiSC] ADD FOREIGN KEY ([MaTB]) REFERENCES [ThietBi] ([MaTB])
GO

ALTER TABLE [PhieuDeNghiSC] ADD FOREIGN KEY ([MaND]) REFERENCES [NguoiDung] ([MaND])
GO

ALTER TABLE [NguoiDung] ADD FOREIGN KEY ([MaCV]) REFERENCES [ChucVu] ([MaCV])
GO

ALTER TABLE [NguoiDung] ADD FOREIGN KEY ([MaDV]) REFERENCES [DonVi] ([MaDV])
GO

--ALTER TABLE [NguoiDung] ADD FOREIGN KEY ([MaTK]) REFERENCES [TaiKhoan] ([MaTK])
--GO

ALTER TABLE [ThietBi] ADD FOREIGN KEY ([MaDV]) REFERENCES [DonVi] ([MaDV])
GO

ALTER TABLE [PhieuSuaChua] ADD FOREIGN KEY ([MaND]) REFERENCES [NguoiDung] ([MaND])
GO

ALTER TABLE [PhieuSuaChua] ADD FOREIGN KEY ([MaDV]) REFERENCES [DonVi] ([MaDV])
GO

ALTER TABLE [ChiTietPhieuSC] ADD FOREIGN KEY ([MaPhieuSC]) REFERENCES [PhieuSuaChua] ([MaPhieuSC])
GO

ALTER TABLE [ChiTietPhieuSC] ADD FOREIGN KEY ([MaTB]) REFERENCES [ThietBi] ([MaTB])
GO


-- Thêm dữ liệu cho bảng ChucVu
INSERT INTO ChucVu (MaCV, TenCV)
VALUES 
(1, N'Ban giám hiệu'),
(2, N'Trưởng khoa'),
(3, N'Trưởng phòng CSVC'),
(4, N'Nhân viên kỹ thuật'),
(5, N'Nhân viên kế toán'),
(6, N'Quản trị viên'),
(7, N'Người dùng');

-- Thêm dữ liệu cho bảng DonVi
INSERT INTO DonVi (MaDV, TenD)
VALUES 
(1, N'Khoa công nghệ số'),
(2, N'Khoa điện'),
(3, N'Khoa xây dựng'),
(4, N'Phòng CSVC'),
(5, N'Phòng KHTC'),
(6, N'Quản trị hệ thống'),
(7, N'Ban giám hiệu');

-- Thêm dữ liệu cho bảng NguoiDung
INSERT INTO NguoiDung ( HoTen, SDT, Email, MaCV, MaDV, TaiKhoan, MatKhau)
VALUES 
( N'Trưởng khoa', '123456789', 'user1@example.com', 2, 1, 'user1','pw1'),
( N'Trưởng phòng CSVC', '987654321', 'user2@example.com', 3, 4, 'user2','pw2'),
( N'NVKT', '0987654321', 'user3@example.com', 4, 5, 'user3','pw3'),
( N'Quản trị viên', '123456789', 'qtv@example.com', 6, 6, 'qtv','1'),
( N'Người dùng ', '987654321', 'nguoidung@example.com', 7, 1, 'nguoidung','1')
-- Thêm dữ liệu cho bảng NguoiDung
INSERT INTO NguoiDung ( HoTen, SDT, Email, MaCV, MaDV, TaiKhoan, MatKhau)
VALUES 
( N'Người dùng 4', '123466789', 'user4@example.com', 1,7, 'bgh','1'),
( N'Người dùng 5', '987644321', 'user5@example.com', 5, 5, 'userketoan','1');
INSERT INTO NguoiDung ( HoTen, SDT, Email, MaCV, MaDV, TaiKhoan, MatKhau)
VALUES 
( N'Người dùng 6', '123466789', 'user9@example.com', 1,7, 'bgh','phuochuan001@#')
-- Thêm dữ liệu cho bảng ThietBi
INSERT INTO ThietBi (MaTB, TenTB, LoaiTB, NamSuDung, MaDV)
VALUES 
(1, N'Máy in', N'Máy văn phòng', 3, 1),
(2, N'Máy tính', N'Máy tính để bàn', 2, 2),
(3, N'Máy chiếu', N'Thiết bị trình chiếu', 1, 3);
-- Thêm dữ liệu cho bảng VanPhongPham
INSERT INTO VanPhongPham (MaVPP, TenVPP, DonViTinh, DonGia, TenNCC, SDTNCC,SoLuong)
VALUES 
(1, N'Bút bi xanh', N'Cái', 5000, N'Công ty ABC', '0123456789',3),
(2, N'Bút chì HB', N'Cây', 2000, N'Công ty XYZ', '0987654321',8),
(3, N'Giấy A4', N'Tờ', 10000, N'Nhà sách MNP', '0365478912',9);

-- Thêm dữ liệu cho bảng PhieuMuaHang
INSERT INTO PhieuMuaHang (MaPhieuMH, NgayLap, TongGiaTri, TrangThai, GhiChu, MaND, MaDV)
VALUES 
(1, '2024-04-10', 80000, N'Đã thanh toán', N'Ghi chú 1', 1, 1),
(2, '2024-04-11', 300000, N'Chưa thanh toán', N'Ghi chú 2', 2, 2),
(3, '2024-04-12', 150000, N'Đã thanh toán', N'Ghi chú 3', 3, 3);

-- Thêm dữ liệu cho bảng ChiTietPhieuMH
INSERT INTO ChiTietPhieuMH (MaPhieuMH, MaVPP, SoLuong, DonGia, GhiChu)
VALUES 
(1, 1, 10, 5000, N'Ghi chú 1'),
(1, 2, 20, 2000, N'Ghi chú 2'),
(2, 3, 30, 10000, N'Ghi chú 3');

-- Thêm dữ liệu cho bảng PhieuDeNghiMS
INSERT INTO PhieuDeNghiMS (MaDNMS, YKienCSVC, TrangThai, MaND,TongSoLoai)
VALUES 
(1, N'Yêu cầu mua thêm bút bi đỏ', N'Đang xem xét',2, 1),
(2, N'Yêu cầu mua thêm giấy A3', N'Đang xem xét',1, 2),
(3, N'Yêu cầu mua thêm bút chì màu', N'Đang xem xét',2, 3);

-- Thêm dữ liệu cho bảng PhieuSuaChua
INSERT INTO PhieuSuaChua (MaPhieuSC, NgayLap, TongGiaTri, TrangThai, GhiChu, MaND, MaDV)
VALUES 
(1, '2024-04-10', 700000, N'Chưa hoàn thành', N'Ghi chú 1', 1, 1),
(2, '2024-04-11', 1500000, N'Đã hoàn thành', N'Ghi chú 2', 2, 2),
(3, '2024-04-12', 500000, N'Chưa hoàn thành', N'Ghi chú 3', 3, 3);

-- Thêm dữ liệu cho bảng ChiTietPhieuSC
INSERT INTO ChiTietPhieuSC (MaPhieuSC, MaTB, NoiDung, ChiPhi, GhiChu)
VALUES 
(1, 1, N'Thuê kỹ thuật viên sửa chữa', 200000, N'Ghi chú 1'),
(1, 2, N'Thiết bị cần thay thế linh kiện mới', 500000, N'Ghi chú 2'),
(2, 3, N'Bảo dưỡng định kỳ', 150000, N'Ghi chú 3');



-- Thêm dữ liệu cho bảng ChiTietDeNghiMS
INSERT INTO ChiTietDeNghiMS (MaDNMS, MaVPP, DVT, SoLuong, LyDo)
VALUES 
(1, 1, N'Cái', 50, N'Cần dùng cho văn phòng'),
(2, 2, N'Cây', 100, N'Tiêu thụ nhiều trong tháng'),
(3, 3, N'Tờ', 200, N'Cần mua dự trữ');

-- Thêm dữ liệu cho bảng PhieuDeNghiSC
INSERT INTO PhieuDeNghiSC (MaDNSC, YKienCSVC, NgayDeNghi, TrangThai, MaND,TongSoLoai)
VALUES 
(1, N'Yêu cầu sửa chữa máy in', '2024-04-10', N'Trưởng khoa đã duyệt', 1,1),
(2, N'Yêu cầu bảo dưỡng máy tính', '2024-04-11', N'Trưởng phòng CSVC đã duyệt', 3,2),
(3, N'Yêu cầu thay thế linh kiện máy chiếu', '2024-04-12', N'Trưởng khoa đã duyệt',2 ,3);
INSERT INTO ChiTietDeNghiSC (MaDNSC, MaTB, DVT, SoLuong, LyDo)
VALUES 
(1, 1, N'Cái', 10, N'Đang hỏng'),
(1, 2, N'Cây', 5, N'Cần bảo dưỡng'),
(2, 3, N'Cái', 2, N'Cần thay thế linh kiện'),
(3, 1, N'Cái', 3, N'Cần sửa chữa'),
(3, 2, N'Cây', 1, N'Cần bảo dưỡng định kỳ');

-- Thêm dữ liệu cho bảng PhieuDeNghiSC
INSERT INTO PhieuDeNghiSC (MaDNSC, YKienCSVC, NgayDeNghi, TrangThai, MaND, TongSoLoai)
VALUES 
(4, N'Yêu cầu sửa chữa máy tính xách tay', '2024-04-10', N'Đang xem xét', 2, 1),
(5, N'Yêu cầu mua thêm giấy A4', '2024-04-11', N'Đang xem xét', 3, 2),
(6, N'Yêu cầu thay đổi linh kiện máy chiếu', '2024-04-12', N'Đang xem xét', 1, 3),
(7, N'Yêu cầu bảo dưỡng máy tính để bàn', '2024-04-13', N'Đang xem xét', 2, 1),
(8, N'Yêu cầu mua thêm bút chì màu', '2024-04-14', N'Đang xem xét', 3, 2),
(9, N'Yêu cầu sửa chữa máy in màu', '2024-04-15', N'Đang xem xét', 1, 3),
(10, N'Yêu cầu mua thêm giấy A3', '2024-04-16', N'Đang xem xét', 2, 1),
(11, N'Yêu cầu thay đổi linh kiện máy tính xách tay', '2024-04-17', N'Đang xem xét', 3, 2),
(12, N'Yêu cầu bảo dưỡng máy chiếu', '2024-04-18', N'Đang xem xét', 1, 3),
(13, N'Yêu cầu sửa chữa máy in', '2024-04-19', N'Đang xem xét', 2, 1);

-- Thêm dữ liệu cho bảng ChiTietDeNghiSC
INSERT INTO ChiTietDeNghiSC (MaDNSC, MaTB, DVT, SoLuong, LyDo)
VALUES 
(4, 1, N'Cái', 10, N'Đang hỏng'),
(4, 2, N'Cây', 5, N'Cần bảo dưỡng'),
(5, 3, N'Cái', 2, N'Cần thay thế linh kiện'),
(6, 1, N'Cái', 3, N'Cần sửa chữa'),
(6, 2, N'Cây', 1, N'Cần bảo dưỡng định kỳ'),
(7, 1, N'Cái', 10, N'Đang hỏng'),
(7, 2, N'Cây', 5, N'Cần bảo dưỡng'),
(8, 3, N'Cái', 2, N'Cần thay thế linh kiện'),
(9, 1, N'Cái', 3, N'Cần sửa chữa'),
(9, 2, N'Cây', 1, N'Cần bảo dưỡng định kỳ');


Select * from ChucVu
Select * from NguoiDung
select * from DonVi

select * from ThietBi
select * from VanPhongPham

select * from [PhieuMuaHang]
select * from [ChiTietPhieuMH] --

select * from [PhieuDeNghiSC]
select * from [ChiTietDeNghiSC] --

select * from [PhieuSuaChua]
select * from [ChiTietPhieuSC] --

select * from [PhieuDeNghiMS]
select * from [ChiTietDeNghiMS]

select * from [PhieuSuaChua]
select * from [PhieuMuaHang]
Go




Go
-- Function lọc phiếu đề nghị sửa chữa theo trạng thái
CREATE FUNCTION dbo.FilterPhieuDeNghiSCByTrangThai
(
    @TrangThai nvarchar(100)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM [PhieuDeNghiSC]
    WHERE [TrangThai] = @TrangThai
);
Go
--SELECT *
--FROM dbo.FilterPhieuDeNghiSCByTrangThai(N'Trưởng khoa đã duyệt');

go
-- Function lọc phiếu đề nghị trong khoảng thời gian cụ thể:
CREATE FUNCTION dbo.FilterPhieuDeNghiSCByThoiGian
(
    @TuNgay date,
    @DenNgay date
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM [PhieuDeNghiSC]
    WHERE [NgayDeNghi] BETWEEN @TuNgay AND @DenNgay
);
Go
--SELECT * FROM dbo.FilterPhieuDeNghiSCByThoiGian('2024-04-01', '2024-04-11');


Go
--Function lấy ra danh sách trưởng khoa của các đơn vị
CREATE FUNCTION dbo.FilterTruongKhoa
(
)
RETURNS TABLE
AS
RETURN
(
    SELECT ND.MaND, ND.HoTen, ND.SDT, ND.Email, CV.TenCV, D.TenD
    FROM NguoiDung ND
    INNER JOIN ChucVu CV ON ND.MaCV = CV.MaCV
	Inner join DonVi D ON ND.MaDV = D.MaDV
    WHERE CV.TenCV = N'Trưởng khoa'
);
Go
--SELECT * FROM dbo.FilterTruongKhoa();
go
-- Procedure Cập nhật trạng thái của Phiếu đề nghị sửa chữa
CREATE PROCEDURE dbo.UpdatePhieuDeNghiSCStatus  @MaDNSC INT, @TrangThai nvarchar(100)
AS
BEGIN
    UPDATE PhieuDeNghiSC
    SET TrangThai = @TrangThai
    WHERE MaDNSC = @MaDNSC 
END;

--Select * from PhieuDeNghiSC
--EXEC dbo.UpdatePhieuDeNghiSCStatus 1, N'Trưởng khoa đã duyệt'
--Select * from PhieuDeNghiSC

Go
-- Procedure Tạo phiếu mua hàng
CREATE PROCEDURE dbo.CreatePhieuMuaHang
(
	@MaPMH int,
    @TrangThai nvarchar(100),
    @GhiChu nvarchar(500),
    @MaND int,
    @MaDV int
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PhieuMuaHang (MaPhieuMH,NgayLap, TongGiaTri, TrangThai, GhiChu, MaND, MaDV)
    VALUES (@MaPMH, GETDATE(), 0, @TrangThai, @GhiChu, @MaND, @MaDV);
END;

--Select * From PhieuMuaHang
--EXEC dbo.CreatePhieuMuaHang 5, N'Đã thanh toán', N'Ghi chú 555', 1, 2;
--Select * From PhieuMuaHang
Go

-- Procedure Tạo chi tiết phiếu mua hàng
CREATE PROCEDURE dbo.CreateChiTietPhieuMuaHang
(
	@MaPhieuMH int,
	@MaVPP int,
	@SoLuong int,
	@DonGia decimal(18,2),
	@GhiChu nvarchar(500)
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ChiTietPhieuMH (MaPhieuMH, MaVPP, SoLuong, DonGia, GhiChu)
	VALUES 
	(@MaPhieuMH, @MaVPP, @SoLuong, @DonGia, @GhiChu);
END;
Go
	--Select * From PhieuMuaHang
	--Select * From ChiTietPhieuMH
	--EXEC dbo.CreateChiTietPhieuMuaHang 5, 2 ,3 ,3000 , N'Ghi chú testtt';
	--Select * From PhieuMuaHang
	--Select * From ChiTietPhieuMH
Go
-- Trigger Tự Động Cập Nhật Tổng Giá Trị Phiếu Mua Hàng
CREATE TRIGGER UpdateTotalPrice_PhieuMuaHang
ON ChiTietPhieuMH
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    UPDATE PhieuMuaHang
    SET TongGiaTri = (
        SELECT SUM(SoLuong * DonGia)
        FROM ChiTietPhieuMH
        WHERE ChiTietPhieuMH.MaPhieuMH = PhieuMuaHang.MaPhieuMH
    )
    WHERE MaPhieuMH IN (SELECT DISTINCT MaPhieuMH FROM inserted)
       OR MaPhieuMH IN (SELECT DISTINCT MaPhieuMH FROM deleted);
END;

--Select * from PhieuMuaHang
--Select * from ChiTietPhieuMH
--	EXEC dbo.CreateChiTietPhieuMuaHang 5, 3 ,4 ,6000 , N'Ghi chú Tessttt';
--Select * from PhieuMuaHang
--Select * from ChiTietPhieuMH
--go
