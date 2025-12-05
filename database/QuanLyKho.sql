/*Chạy bằng MySQL*/
DROP DATABASE quanlykho;
CREATE DATABASE quanlykho;
USE quanlykho;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*Tạo bảng*/
CREATE TABLE `DANHMUCCHUCNANG` (
    `MCN` VARCHAR(50) NOT NULL COMMENT 'Mã chức năng',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên chức năng',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MCN) 
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `CTQUYEN` (
    `MNQ` INT(11) NOT NULL COMMENT 'Mã nhóm quyền',
    `MCN` VARCHAR(50) NOT NULL COMMENT 'Mã chức năng',
    `HANHDONG` VARCHAR(255) NOT NULL COMMENT 'Hành động thực hiện',
    PRIMARY KEY(MNQ, MCN, HANHDONG)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `NHOMQUYEN` (
    `MNQ` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã nhóm quyền',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên nhóm quyền',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MNQ) 
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `NHANVIEN` (
    `MNV` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã nhân viên',
    `HOTEN` VARCHAR(255) NOT NULL COMMENT 'Họ và tên NV',
    `GIOITINH` INT(11) NOT NULL COMMENT 'Giới tính',
    `NGAYSINH` DATE NOT NULL COMMENT 'Ngày sinh',
    `SDT` VARCHAR(11) NOT NULL COMMENT 'Số điện thoại',
    `EMAIL` VARCHAR(50) NOT NULL UNIQUE COMMENT 'Email',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MNV)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;


CREATE TABLE `TAIKHOAN` (
    `MNV` INT(11) NOT NULL COMMENT 'Mã nhân viên',
    `MK` VARCHAR(255) NOT NULL COMMENT 'Mật khẩu',
    `TDN` VARCHAR(255) NOT NULL UNIQUE COMMENT 'Tên đăng nhập',
    `MNQ` INT(11) NOT NULL COMMENT 'Mã nhóm quyền',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    `OTP` VARCHAR(50) DEFAULT NULL COMMENT 'Mã OTP',
    PRIMARY KEY(MNV, TDN)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;


CREATE TABLE `KHACHHANG` (
    `MKH` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã khách hàng',
    `HOTEN` VARCHAR(255) NOT NULL COMMENT 'Họ và tên KH',
    `NGAYTHAMGIA` DATE NOT NULL COMMENT 'Ngày tạo dữ liệu',
    `DIACHI` VARCHAR(255) COMMENT 'Địa chỉ',
    `SDT` VARCHAR(11) UNIQUE NOT NULL COMMENT 'Số điện thoại',
    `EMAIL` VARCHAR(50) UNIQUE COMMENT 'Email',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MKH)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `PHIEUXUAT` (
    `MPX` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã phiếu xuất',
    `MNV` INT(11) DEFAULT 1 COMMENT 'Mã nhân viên',
    `MKH` INT(11) NOT NULL COMMENT 'Mã khách hàng',
    `TIEN` INT(11) NOT NULL COMMENT 'Tổng tiền',
    `TG` DATETIME DEFAULT current_timestamp() COMMENT 'Thời gian tạo',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MPX)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `CTPHIEUXUAT` (
    `MPX` INT(11) NOT NULL COMMENT 'Mã phiếu xuất',
    `MSP` INT(11) NOT NULL COMMENT 'Mã sản phẩm',
    `SL` INT(11) NOT NULL COMMENT 'Số lượng',
    `TIENXUAT` INT(11) NOT NULL COMMENT 'Tiền xuất',
    PRIMARY KEY(MPX, MSP)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;


CREATE TABLE `NHACUNGCAP` (
    `MNCC` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã nhà cung cấp',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên NCC',
    `DIACHI` VARCHAR(255) COMMENT 'Địa chỉ',
    `SDT` VARCHAR(12) COMMENT 'Số điện thoại',
    `EMAIL` VARCHAR(50) COMMENT 'Email',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MNCC)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `PHIEUNHAP` (
    `MPN` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã phiếu nhập',
    `MNV` INT(11) NOT NULL COMMENT 'Mã nhân viên',
    `MNCC` INT(11) NOT NULL COMMENT 'Mã nhà cung cấp',
    `TIEN` INT(11) NOT NULL COMMENT 'Tổng tiền',
    `TG` DATETIME DEFAULT current_timestamp() COMMENT 'Thời gian tạo',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MPN)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `CTPHIEUNHAP` (
    `MPN` INT(11) NOT NULL COMMENT 'Mã phiếu nhập',
    `MSP` INT(11) NOT NULL COMMENT 'Mã sản phẩm',
    `SL` INT(11) NOT NULL COMMENT 'Số lượng',
    `TIENNHAP` INT(11) NOT NULL COMMENT 'Tiền nhập',
    `HINHTHUC` INT(11) NOT NULL DEFAULT 0 COMMENT 'Tổng tiền',
    PRIMARY KEY(MPN, MSP)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;
CREATE TABLE `PHIEUKIEMKE` (
    `MPKK` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã phiếu kiểm kê',
    `MNV` INT(11) NOT NULL COMMENT 'Mã nhân viên',
    `TG` DATETIME DEFAULT current_timestamp() COMMENT 'Thời gian tạo',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MPKK)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `CTPHIEUKIEMKE` (
    `MPKK` INT(11) NOT NULL COMMENT 'Mã phiếu kiểm kê',
    `MSP` INT(11) NOT NULL COMMENT 'Mã sản phẩm',
    `TRANGTHAISP` INT(11) NOT NULL COMMENT 'Trạng thái sản phẩm',
    `GHICHU` VARCHAR(255) COMMENT 'Ghi chú',
    PRIMARY KEY(MPKK, MSP)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `SANPHAM` (
    `MSP` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã sản phẩm',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên sản phẩm',
    `HINHANH` VARCHAR(255) DEFAULT NULL COMMENT 'Hình ảnh sản phẩm',
    `DANHMUC` VARCHAR(255) DEFAULT NULL COMMENT 'Danh mục',
    `MNSX` INT(11) NOT NULL COMMENT 'Mã nhà sản xuất',
    `MKVK` INT(11) NOT NULL COMMENT 'Mã khu vực kho',
    `MLSP` INT(11) NOT NULL COMMENT 'Mã loại sản phẩm',
    `TIENX` INT(11) NOT NULL DEFAULT 0 COMMENT 'Tiền xuất',
    `TIENN` INT(11) NOT NULL DEFAULT 0 COMMENT 'Tiền nhập',
    `SL` INT(11) NOT NULL DEFAULT 0 COMMENT 'Số lượng tồn',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY (`MSP`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `LOAISANPHAM` (
    `MLSP` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã loại sản phẩm',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên loại sản phẩm',
    `GHICHU` VARCHAR(255) DEFAULT '' COMMENT 'Ghi chú',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(`MLSP`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `NHASANXUAT` (
    `MNSX` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã nhà sản xuất',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên nhà xuất bản',
    `DIACHI` VARCHAR(255) COMMENT 'Địa chỉ',
    `SDT` VARCHAR(12) COMMENT 'Số điện thoại',
    `EMAIL` VARCHAR(50) COMMENT 'Email',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MNSX)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

CREATE TABLE `KHUVUCKHO` (
    `MKVK` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Mã khu vực',
    `TEN` VARCHAR(255) NOT NULL COMMENT 'Tên khu vực sách',
    `GHICHU` VARCHAR(255) DEFAULT '' COMMENT 'Ghi chú',
    `TT` INT(11) NOT NULL DEFAULT 1 COMMENT 'Trạng thái',
    PRIMARY KEY(MKVK)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

/*Thêm dữ liệu*/

INSERT INTO `DANHMUCCHUCNANG`(`MCN`, `TEN`, `TT`)
VALUES 
        ('sanpham', 'Quản lý sản phẩm', 1),
        ('khachhang', 'Quản lý khách hàng', 1),
        ('nhacungcap', 'Quản lý nhà cung cấp', 1),
        ('nhasanxuat', 'Quản lý nhà sản xuất', 1),
        ('nhanvien', 'Quản lý nhân viên', 1),
        ('nhaphang', 'Quản lý nhập hàng', 1),
        ('xuathang', 'Quản lý xuất hàng', 1),
        ('kiemke', 'Quản lý kiểm kê', 1),
        ('khuvuckho', 'Quản lý khu vực kho', 1),
        ('nhomquyen', 'Quản lý nhóm quyền', 1),
        ('taikhoan', 'Quản lý tài khoản', 1),
        ('thongke', 'Quản lý thống kê', 1);

INSERT INTO `CTQUYEN` (`MNQ`, `MCN`, `HANHDONG`)
VALUES
		(1, 'sanpham', 'create'),
        (1, 'sanpham', 'delete'),
        (1, 'sanpham', 'update'),
        (1, 'sanpham', 'view'),
        (1, 'khachhang', 'create'),
        (1, 'khachhang', 'delete'),
        (1, 'khachhang', 'update'),
        (1, 'khachhang', 'view'),
        (1, 'nhacungcap', 'create'),
        (1, 'nhacungcap', 'delete'),
        (1, 'nhacungcap', 'update'),
        (1, 'nhacungcap', 'view'),
        (1, 'nhasanxuat', 'create'),
        (1, 'nhasanxuat', 'delete'),
        (1, 'nhasanxuat', 'update'),
        (1, 'nhasanxuat', 'view'),
        (1, 'nhanvien', 'create'),
        (1, 'nhanvien', 'delete'),
        (1, 'nhanvien', 'update'),
        (1, 'nhanvien', 'view'),
        (1, 'nhaphang', 'create'),
        (1, 'nhaphang', 'delete'),
        (1, 'nhaphang', 'update'),
        (1, 'nhaphang', 'view'),
        (1, 'xuathang', 'create'),
        (1, 'xuathang', 'delete'),
        (1, 'xuathang', 'update'),
        (1, 'xuathang', 'view'),
        (1, 'kiemke', 'create'),
        (1, 'kiemke', 'delete'),
        (1, 'kiemke', 'update'),
        (1, 'kiemke', 'view'),
        (1, 'khuvuckho', 'create'),
        (1, 'khuvuckho', 'delete'),
        (1, 'khuvuckho', 'update'),
        (1, 'khuvuckho', 'view'),
        (1, 'nhomquyen', 'create'),
        (1, 'nhomquyen', 'delete'),
        (1, 'nhomquyen', 'update'),
        (1, 'nhomquyen', 'view'),
        (1, 'taikhoan', 'create'),
        (1, 'taikhoan', 'delete'),
        (1, 'taikhoan', 'update'),
        (1, 'taikhoan', 'view'),
        (1, 'thongke', 'create'),
        (1, 'thongke', 'delete'),
        (1, 'thongke', 'update'),
        (1, 'thongke', 'view'),
        (2, 'sanpham', 'view'),
        (2, 'nhasanxuat', 'view'),
        (2, 'khachhang', 'create'),
        (2, 'khachhang', 'delete'),
        (2, 'khachhang', 'update'),
        (2, 'khachhang', 'view'),
        (2, 'xuathang', 'create'),
        (2, 'xuathang', 'update'),
        (2, 'xuathang', 'view'),
        (3, 'sanpham', 'create'),
        (3, 'sanpham', 'delete'),
        (3, 'sanpham', 'update'),
        (3, 'sanpham', 'view'),
        (3, 'nhaphang', 'create'),
        (3, 'nhaphang', 'update'),
        (3, 'nhaphang', 'view'),
        (3, 'kiemke', 'create'),
        (3, 'kiemke', 'delete'),
        (3, 'kiemke', 'update'),
        (3, 'kiemke', 'view'),
        (3, 'nhacungcap', 'create'),
        (3, 'nhacungcap', 'delete'),
        (3, 'nhacungcap', 'update'),
        (3, 'nhacungcap', 'view'),
        (3, 'nhasanxuat', 'create'),
        (3, 'nhasanxuat', 'delete'),
        (3, 'nhasanxuat', 'update'),
        (3, 'nhasanxuat', 'view');

INSERT INTO `NHOMQUYEN` (`TEN`, `TT`)
VALUES
        ('Quản lý cửa hàng', 1),
        ('Nhân viên bán hàng', 1),
        ('Nhân viên quản lý kho', 1);


INSERT INTO `NHANVIEN` (`HOTEN`, `GIOITINH`, `NGAYSINH`, `SDT`, `EMAIL`, `TT`)
VALUES
        ('Lê Thế Minh', 0, '2077-01-01', '0505555505', 'remchan.com@gmail.com', 1),
        ('Huỳnh Khôi Nguyên', 1, '2023-05-06', '0123456789', 'nguyeney111@gmail.com', 1),
        ('Trần Gia Nguyễn', 1, '2004-07-17', '0387913347', 'trangianguyen.com@gmail.com', 1),
        ('Hoàng Gia Bảo', 1, '2003-04-11', '0355374322', 'musicanime2501@gmail.com', 1),
        ('Đỗ Nam Công Chính', 1, '2003-04-11', '0123456789', 'chinchin@gmail.com', 1),
        ('Đinh Ngọc Ân', 1, '2003-04-03', '0123456789', 'ngocan@gmail.com', 1);

INSERT INTO `TAIKHOAN` (`MNV`, `TDN`, `MK`, `MNQ`, `TT`, `OTP`)
VALUES
        (1, 'admin', '$2a$12$6GSkiQ05XjTRvCW9MB6MNuf7hOJEbbeQx11Eb8oELil1OrCq6uBXm', 1, 1, 'null'),
        (2, 'NV2', '$2a$12$6GSkiQ05XjTRvCW9MB6MNuf7hOJEbbeQx11Eb8oELil1OrCq6uBXm', 2, 1, 'null'),
        (3, 'NV3', '$2a$12$6GSkiQ05XjTRvCW9MB6MNuf7hOJEbbeQx11Eb8oELil1OrCq6uBXm', 3, 1, 'null');

INSERT INTO `KHACHHANG` (`HOTEN`, `DIACHI`, `SDT`, `TT`, `NGAYTHAMGIA`)
VALUES
        ('Nguyễn Văn A', 'Gia Đức, Ân Đức, Hoài Ân, Bình Định', '0387913347', 1, '2024-04-15 09:52:29'),
        ('Trần Nhất Nhất', '205 Trần Hưng Đạo, Phường 10, Quận 5, Thành phố Hồ Chí Minh', '0123456789', 1, '2024-04-15 09:52:29'),
        ('Hoàng Gia Bo', 'Khoa Trường, Hoài Ân, Bình Định', '0987654321', 1, '2024-04-15 09:52:29'),
        ('Hồ Minh Hưng', 'Khoa Trường, Hoài Ân, Bình Định', '0867987456', 1, '2024-04-15 09:52:29'),
        ('Nguyễn Thị Minh Anh', '123 Phố Huế, Quận Hai Bà Trưng, Hà Nội', '0935123456', 1, '2024-04-16 17:59:57'),
        ('Trần Đức Minh', '789 Đường Lê Hồng Phong, Thành phố Đà Nẵng', '0983456789', 1, '2024-04-16 18:08:12'),
        ('Lê Hải Yến', '456 Tôn Thất Thuyết, Quận 4, Thành phố Hồ Chí Minh', '0977234567', 1, '2024-04-16 18:08:47'),
        ('Phạm Thanh Hằng', '102 Lê Duẩn, Thành phố Hải Phòng', '0965876543', 1, '2024-04-16 18:12:59'),
        ('Hoàng Đức Anh', '321 Lý Thường Kiệt, Thành phố Cần Thơ', '0946789012', 1, '2024-04-16 18:13:47'),
        ('Ngô Thanh Tùng', '987 Trần Hưng Đạo, Quận 1, Thành phố Hồ Chí Minh', '0912345678', 1, '2024-04-16 18:14:12'),
        ('Võ Thị Kim Ngân', '555 Nguyễn Văn Linh, Quận Nam Từ Liêm, Hà Nội', '0916789123', 1, '2024-04-16 18:15:11'),
        ('Đỗ Văn Tú', '777 Hùng Vương, Thành phố Huế', '0982345678', 1, '2024-04-30 18:15:56'),
        ('Lý Thanh Trúc', '888 Nguyễn Thái Học, Quận Ba Đình, Hà Nội', '0982123456', 1, '2024-04-16 18:16:22'),
        ('Bùi Văn Hoàng', '222 Đường 2/4, Thành phố Nha Trang', '0933789012', 1, '2024-04-16 18:16:53'),
        ('Lê Văn Thành', '23 Đường 3 Tháng 2, Quận 10, TP. Hồ Chí Minh', '0933456789', 1, '2024-04-16 18:17:46'),
        ('Nguyễn Thị Lan Anh', '456 Lê Lợi, Quận 1, TP. Hà Nội', '0965123456', 1, '2024-04-16 18:18:10'),
        ('Phạm Thị Mai', '234 Lê Hồng Phong, Quận 5, TP. Hồ Chí Minh', '0946789013', 1, '2024-04-17 18:18:34'),
        ('Hoàng Văn Nam', ' 567 Phố Huế, Quận Hai Bà Trưng, Hà Nội', '0912345679', 1, '2024-04-17 18:19:16');


INSERT INTO `PHIEUXUAT` (`MNV`, `MKH`, `TIEN`, `TG`, `TT`)
VALUES
        (1, 1, 100000, '2024-04-18 17:34:12', 1),
        (1, 2, 200000, '2024-04-17 18:19:51', 1);

INSERT INTO `CTPHIEUXUAT` (`MPX`, `MSP`, `SL`,  `TIENXUAT`)
VALUES
        (1, 1, 2, 100000),
        (1, 2, 2, 200000),
        (2, 3, 2, 300000),
        (2, 4, 2, 400000);

INSERT INTO `NHACUNGCAP` (`TEN`, `DIACHI`, `SDT`, `EMAIL`, `TT`) 
VALUES
    ('Công ty Synnex FPT', 'KCX Tân Thuận, Quận 7, TP.HCM', '02873006666', 'phanphoi@fpt.com.vn', 1),
    ('Digiworld (DGW)', '195-197 Nguyễn Thái Bình, Quận 1, TP.HCM', '02839290059', 'support@dgw.com.vn', 1),
    ('Viễn Sơn Technology', '175 Nguyễn Thị Minh Khai, Quận 1, TP.HCM', '02839250713', 'sales@vienson.com.vn', 1),
    ('Thùy Minh Technology (TMC)', 'Số 5 ngõ 15 Thái Hà, Đống Đa, Hà Nội', '02435375566', 'info@thuyminh.vn', 1),
    ('Công ty PSD (Petrosetco)', 'Số 1 Nam Kỳ Khởi Nghĩa, Quận 1, TP.HCM', '02839115578', 'contact@psd.com.vn', 1),
    ('Hải Anh Computer', '103 Lê Thanh Nghị, Hai Bà Trưng, Hà Nội', '02436288137', 'kinhdoanh@haianh.com.vn', 1),
    ('GearVN Enterprise', '78-80 Hoàng Hoa Thám, Tân Bình, TP.HCM', '18006975', 'business@gearvn.com', 1),
    ('Elite JSC', '289/1 Ung Văn Khiêm, Bình Thạnh, TP.HCM', '02835123959', 'sales@elite-jsc.com', 1);


INSERT INTO `PHIEUNHAP` (`MNV`, `MNCC`, `TIEN`, `TG`, `TT`)
VALUES
        (1, 1, 100000, '2024-04-01 01:09:27', 1),
        (1, 1, 200000, '2024-04-02 01:09:27', 1);

INSERT INTO `CTPHIEUNHAP` (`MPN`, `MSP`, `SL`, `TIENNHAP`, `HINHTHUC`)
VALUES
        (1, 1, 2, 20000, 0),
        (1, 2, 2, 40000, 0),
        (2, 3, 2, 40000, 0),
        (2, 4, 2, 80000, 0);

INSERT INTO `PHIEUKIEMKE` (`MNV` , `TG` , `TT`) 
VALUES
        (1 , '2024-04-01 01:09:27' , 1);

INSERT INTO `CTPHIEUKIEMKE` (`MPKK`,`MSP` ,`TRANGTHAISP`, `GHICHU`)
VALUES 
        (1, 1, 1 ,'Hư' );


INSERT INTO `SANPHAM` (`TEN`, `HINHANH`, `DANHMUC`, `MNSX`, `MKVK`, `MLSP`, `TIENX`, `TIENN`, `SL`, `TT`) 
VALUES
        ('Laptop Dell XPS 13', 'dell_xps.png', 'Laptop', 1, 1, 1, 25000000, 22000000, 10, 1),
        ('iPhone 15 Pro Max', 'iphone15.png', 'Điện thoại', 2, 1, 1, 35000000, 31000000, 25, 1),
        ('Chuột Logitech G502', 'mouse_g502.png', 'Phụ kiện', 3, 2, 2, 1200000, 900000, 100, 1),
        ('Bàn phím cơ Keychron', 'keychron_k2.png', 'Phụ kiện', 4, 2, 2, 1800000, 1400000, 50, 1),
        ('Màn hình LG 27 inch', 'lg_27.png', 'Màn hình', 5, 1, 1, 5500000, 4800000, 15, 1),
        
	    -- LAPTOP & PC (MLSP: 1)
	    ('MacBook Air M2 2023', 'macbook_m2.png', 'Laptop', 2, 1, 1, 28000000, 24500000, 12, 1),
	    ('Asus ROG Strix G15', 'asus_rog.png', 'Laptop', 7, 1, 1, 32000000, 28000000, 8, 1),
	    ('Asus Vivobook 15', 'asus_vivo.png', 'Laptop', 7, 1, 1, 15000000, 12500000, 20, 1),
	    ('Laptop Dell Inspiron 15', 'dell_inspiron.png', 'Laptop', 1, 1, 1, 18500000, 16000000, 15, 1),
	    ('PC Gaming GVN Titan', 'pc_titan.png', 'PC', 7, 1, 1, 45000000, 38000000, 5, 1),
	
	    -- ĐIỆN THOẠI (MLSP: 1)
	    ('Samsung Galaxy S24 Ultra', 's24_ultra.png', 'Điện thoại', 6, 1, 1, 30000000, 26000000, 18, 1),
	    ('Samsung Galaxy A54', 'samsung_a54.png', 'Điện thoại', 6, 1, 1, 8500000, 6800000, 30, 1),
	    ('iPhone 14 Plus', 'iphone14_plus.png', 'Điện thoại', 2, 1, 1, 21000000, 19000000, 10, 1),
	    ('Sony Xperia 1 V', 'sony_xperia.png', 'Điện thoại', 11, 1, 1, 29000000, 25000000, 4, 1),
	
	    -- MÀN HÌNH (MLSP: 1)
	    ('Màn hình Samsung Odyssey G5', 'samsung_g5.png', 'Màn hình', 6, 1, 1, 7500000, 6200000, 10, 1),
	    ('Màn hình Dell Ultrasharp U2422', 'dell_u2422.png', 'Màn hình', 1, 1, 1, 6200000, 5000000, 12, 1),
	    ('Màn hình Asus ProArt', 'asus_proart.png', 'Màn hình', 7, 1, 1, 9000000, 7800000, 6, 1),
	
	    -- PHỤ KIỆN (MLSP: 2)
	    ('Chuột Logitech MX Master 3S', 'logitech_mx3.png', 'Phụ kiện', 3, 2, 2, 2500000, 1900000, 25, 1),
	    ('Bàn phím Logitech K380', 'logi_k380.png', 'Phụ kiện', 3, 2, 2, 750000, 550000, 40, 1),
	    ('Tai nghe Sony WH-1000XM5', 'sony_xm5.png', 'Phụ kiện', 11, 2, 2, 8500000, 6900000, 15, 1),
	    ('Tai nghe Apple AirPods Pro 2', 'airpods_pro2.png', 'Phụ kiện', 2, 2, 2, 5800000, 4800000, 20, 1),
	    ('Bàn phím cơ Keychron K8 Pro', 'keychron_k8.png', 'Phụ kiện', 4, 2, 2, 2800000, 2100000, 15, 1),
	    ('Lót chuột Razer Gigantus', 'razer_pad.png', 'Phụ kiện', 3, 2, 2, 450000, 250000, 50, 1),
	
	    -- LINH KIỆN ĐIỆN TỬ (MLSP: 3)
	    ('RAM Kingston Fury 16GB', 'kingston_ram16.png', 'Linh kiện', 8, 2, 3, 1200000, 950000, 60, 1),
	    ('SSD Samsung 980 Pro 1TB', 'ssd_980pro.png', 'Linh kiện', 6, 2, 3, 2500000, 1800000, 35, 1),
	    ('SSD Kingston NV2 500GB', 'ssd_nv2.png', 'Linh kiện', 8, 2, 3, 950000, 700000, 45, 1),
	    ('CPU Intel Core i9-13900K', 'intel_i9.png', 'Linh kiện', 12, 2, 3, 14500000, 12000000, 8, 1),
	    ('CPU Intel Core i5-12400F', 'intel_i5.png', 'Linh kiện', 12, 2, 3, 3500000, 2900000, 25, 1),
	    ('Mainboard Asus ROG Z790', 'main_z790.png', 'Linh kiện', 7, 2, 3, 8500000, 7000000, 5, 1),
	
	    -- THIẾT BỊ MẠNG (MLSP: 4)
	    ('Router Wifi 6 TP-Link Archer', 'tplink_ax73.png', 'Thiết bị mạng', 9, 3, 4, 2200000, 1600000, 20, 1),
	    ('Switch TP-Link 8 Port', 'tplink_switch.png', 'Thiết bị mạng', 9, 3, 4, 350000, 220000, 60, 1),
	    ('Bộ phát Wifi Mesh Asus', 'asus_mesh.png', 'Thiết bị mạng', 7, 3, 4, 4500000, 3600000, 10, 1),
	    ('Dây cáp mạng CAT6 100m', 'cable_cat6.png', 'Thiết bị mạng', 9, 3, 4, 800000, 500000, 15, 1),
	
	    -- THIẾT BỊ VĂN PHÒNG (MLSP: 5)
	    ('Máy in Canon LBP 2900', 'canon_2900.png', 'Văn phòng', 10, 3, 5, 3800000, 3200000, 12, 1),
	    ('Máy in màu Canon G3010', 'canon_g3010.png', 'Văn phòng', 10, 3, 5, 4500000, 3700000, 8, 1),
	    ('Máy chiếu Sony VPL-DX221', 'sony_projector.png', 'Văn phòng', 11, 3, 5, 10500000, 8900000, 5, 1),
	    ('Máy hủy tài liệu Silicon', 'shredder.png', 'Văn phòng', 10, 3, 5, 2100000, 1500000, 10, 1),
	    ('Máy chấm công vân tay', 'fingerprint.png', 'Văn phòng', 9, 3, 5, 2500000, 1800000, 7, 1);

INSERT INTO `LOAISANPHAM` (`TEN`, `GHICHU`, `TT`) 
VALUES
        ('Thiết bị điện tử', 'Các thiết bị như Laptop, PC, Màn hình, Điện thoại', 1),
        ('Phụ kiện máy tính', 'Bàn phím, Chuột, Tai nghe, Lót chuột', 1),
        ('Linh kiện điện tử', 'RAM, SSD, HDD, Mainboard, CPU', 1),
        ('Thiết bị mạng', 'Router, Switch, Dây cáp mạng', 1),
        ('Thiết bị văn phòng', 'Máy in, Máy chiếu, Máy hủy tài liệu', 1);


INSERT INTO `NHASANXUAT` (`TEN`, `DIACHI`, `SDT`, `EMAIL`, `TT`) 
VALUES
        ('Dell Inc.', 'Round Rock, Texas, USA', '18005550199', 'contact@dell.com', 1),
        ('Apple Inc.', 'Cupertino, California, USA', '18006927753', 'support@apple.com', 1),
        ('Logitech', 'Lausanne, Switzerland', '02831234567', 'sales@logitech.com', 1),
        ('Keychron', 'Hong Kong', '0909888777', 'support@keychron.com', 1),
        ('LG Electronics', 'Seoul, South Korea', '02435558888', 'service@lg.com', 1),
        ('Samsung', 'Seoul, South Korea', '02839112233', 'support@samsung.com', 1), -- ID: 6
    	('Asus', 'Taipei, Taiwan', '18006588', 'contact@asus.com', 1),            -- ID: 7
   	 	('Kingston', 'Fountain Valley, USA', '1800333444', 'sales@kingston.com', 1), -- ID: 8
    	('TP-Link', 'Shenzhen, China', '02862615079', 'support@tp-link.com', 1),   -- ID: 9
    	('Canon', 'Tokyo, Japan', '02838200466', 'service@canon.com.vn', 1),       -- ID: 10
    	('Sony', 'Tokyo, Japan', '1800588885', 'info@sony.com.vn', 1),             -- ID: 11
    	('Intel', 'Santa Clara, USA', '1800555999', 'support@intel.com', 1);       -- ID: 12

INSERT INTO `KHUVUCKHO` (`TEN`, `GHICHU`, `TT`)
VALUES
        ('Khu vực A', 'Sách dành cho giới trẻ', 1),
        ('Khu vực B', 'Văn học - Nghệ thuật', 1),
        ('Khu vực C', 'Văn học thiếu nhi', 1),
        ('Khu vực D', 'Sách Chính trị - Xã hội', 1);

/*Tạo quan hệ*/

ALTER TABLE `CTQUYEN` ADD CONSTRAINT FK_MNQ_CTQUYEN FOREIGN KEY (MNQ) REFERENCES `NHOMQUYEN`(MNQ) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `CTQUYEN` ADD CONSTRAINT FK_MCN_CTQUYEN FOREIGN KEY (MCN) REFERENCES `DANHMUCCHUCNANG`(MCN) ON DELETE NO ACTION ON UPDATE NO ACTION;           

ALTER TABLE `TAIKHOAN` ADD CONSTRAINT FK_MNV_TAIKHOAN FOREIGN KEY (MNV) REFERENCES `NHANVIEN`(MNV) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `TAIKHOAN` ADD CONSTRAINT FK_MNQ_TAIKHOAN FOREIGN KEY (MNQ) REFERENCES `NHOMQUYEN`(MNQ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `PHIEUXUAT` ADD CONSTRAINT FK_MNV_PHIEUXUAT FOREIGN KEY (MNV) REFERENCES `NHANVIEN`(MNV) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `PHIEUXUAT` ADD CONSTRAINT FK_MKH_PHIEUXUAT FOREIGN KEY (MKH) REFERENCES `KHACHHANG`(MKH) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `CTPHIEUXUAT` ADD CONSTRAINT FK_MPX_CTPHIEUXUAT FOREIGN KEY (MPX) REFERENCES `PHIEUXUAT`(MPX) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `CTPHIEUXUAT` ADD CONSTRAINT FK_MSP_CTPHIEUXUAT FOREIGN KEY (MSP) REFERENCES `SANPHAM`(MSP) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `PHIEUNHAP` ADD CONSTRAINT FK_MNV_PHIEUNHAP FOREIGN KEY (MNV) REFERENCES `NHANVIEN`(MNV) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `PHIEUNHAP` ADD CONSTRAINT FK_MNCC_PHIEUNHAP FOREIGN KEY (MNCC) REFERENCES `NHACUNGCAP`(MNCC) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `CTPHIEUNHAP` ADD CONSTRAINT FK_MPN_CTPHIEUNHAP FOREIGN KEY (MPN) REFERENCES `PHIEUNHAP`(MPN) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `CTPHIEUNHAP` ADD CONSTRAINT FK_MSP_CTPHIEUNHAP FOREIGN KEY (MSP) REFERENCES `SANPHAM`(MSP) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `PHIEUKIEMKE` ADD CONSTRAINT FK_MNV_PHIEUKIEMKE FOREIGN KEY (MNV) REFERENCES `NHANVIEN`(MNV) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `CTPHIEUKIEMKE` ADD CONSTRAINT FK_MPKK_CTPHIEUKIEMKE FOREIGN KEY (MPKK) REFERENCES `PHIEUKIEMKE`(MPKK) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `CTPHIEUKIEMKE` ADD CONSTRAINT FK_MSP_CTPHIEUKIEMKE FOREIGN KEY (MSP) REFERENCES `SANPHAM`(MSP) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `SANPHAM` ADD CONSTRAINT FK_MLSP_SANPHAM FOREIGN KEY (MLSP) REFERENCES `LOAISANPHAM`(MLSP) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `SANPHAM` ADD CONSTRAINT FK_MNSX_SANPHAM FOREIGN KEY (MNSX) REFERENCES `NHASANXUAT`(MNSX) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE `SANPHAM` ADD CONSTRAINT FK_MKVK_SANPHAM FOREIGN KEY (MKVK) REFERENCES `KHUVUCKHO`(MKVK) ON DELETE NO ACTION ON UPDATE NO ACTION;

COMMIT;