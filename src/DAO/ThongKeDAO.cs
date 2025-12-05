using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using src.DTO.ThongKe;
using src.config;

namespace src.DAO
{
    public class ThongKeDAO
    {
        // Singleton Pattern
        private static ThongKeDAO instance;
        public static ThongKeDAO Instance
        {
            get
            {
                if (instance == null) instance = new ThongKeDAO();
                return instance;
            }
        }

        private ThongKeDAO() { }

        // 1. Thống kê Tồn kho (Có đầu kỳ, nhập, xuất, cuối kỳ)
        public List<ThongKeTonKhoDTO> GetThongKeTonKho(string text, DateTime timeStart, DateTime timeEnd)
        {
            List<ThongKeTonKhoDTO> result = new List<ThongKeTonKhoDTO>();
            
            // Set thời gian kết thúc về cuối ngày (23:59:59)
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Câu lệnh SQL phức tạp dùng CTE (Common Table Expression)
                    string sql = @"
                        WITH nhap AS (
                            SELECT MSP, SUM(SL) AS sl_nhap
                            FROM CTPHIEUNHAP
                            JOIN PHIEUNHAP ON PHIEUNHAP.MPN = CTPHIEUNHAP.MPN
                            WHERE TG BETWEEN @start AND @end
                            GROUP BY MSP
                        ),
                        xuat AS (
                            SELECT MSP, SUM(SL) AS sl_xuat
                            FROM CTPHIEUXUAT
                            JOIN PHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                            WHERE TG BETWEEN @start AND @end
                            GROUP BY MSP
                        ),
                        nhap_dau AS (
                            SELECT CTPHIEUNHAP.MSP, SUM(CTPHIEUNHAP.SL) AS sl_nhap_dau
                            FROM PHIEUNHAP
                            JOIN CTPHIEUNHAP ON PHIEUNHAP.MPN = CTPHIEUNHAP.MPN
                            WHERE PHIEUNHAP.TG < @start
                            GROUP BY CTPHIEUNHAP.MSP
                        ),
                        xuat_dau AS (
                            SELECT CTPHIEUXUAT.MSP, SUM(CTPHIEUXUAT.SL) AS sl_xuat_dau
                            FROM PHIEUXUAT
                            JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                            WHERE PHIEUXUAT.TG < @start
                            GROUP BY CTPHIEUXUAT.MSP
                        ),
                        dau_ky AS (
                            SELECT
                                SANPHAM.MSP,
                                COALESCE(nhap_dau.sl_nhap_dau, 0) - COALESCE(xuat_dau.sl_xuat_dau, 0) AS SLdauky
                            FROM SANPHAM
                            LEFT JOIN nhap_dau ON SANPHAM.MSP = nhap_dau.MSP
                            LEFT JOIN xuat_dau ON SANPHAM.MSP = xuat_dau.MSP
                        ),
                        temp_table AS (
                            SELECT SANPHAM.MSP, SANPHAM.TEN, dau_ky.SLdauky, 
                                   COALESCE(nhap.sl_nhap, 0) AS SLnhap, 
                                   COALESCE(xuat.sl_xuat, 0) AS SLxuat, 
                                   (dau_ky.SLdauky + COALESCE(nhap.sl_nhap, 0) - COALESCE(xuat.sl_xuat, 0)) AS SLcuoiky
                            FROM dau_ky
                            LEFT JOIN nhap ON dau_ky.MSP = nhap.MSP
                            LEFT JOIN xuat ON dau_ky.MSP = xuat.MSP
                            JOIN SANPHAM ON dau_ky.MSP = SANPHAM.MSP
                        )
                        SELECT * FROM temp_table
                        WHERE TEN LIKE @text OR MSP LIKE @text
                        ORDER BY MSP;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", timeStart);
                        cmd.Parameters.AddWithValue("@end", timeEndFixed);
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int msp = rs.GetInt32("MSP");
                                // Dùng Maphienbansp giả tạm thời là 0 hoặc lấy MSP nếu DTO yêu cầu
                                int maphienbansp = 0; 
                                string ten = rs.GetString("TEN");
                                
                                // Các thông số RAM, ROM, MauSac không có trong bảng SANPHAM mới -> Để tạm
                                int ram = 0;
                                int rom = 0;
                                string mausac = "";

                                int slDauKy = rs.GetInt32("SLdauky");
                                int slNhap = rs.GetInt32("SLnhap");
                                int slXuat = rs.GetInt32("SLxuat");
                                int slCuoiKy = rs.GetInt32("SLcuoiky");

                                ThongKeTonKhoDTO p = new ThongKeTonKhoDTO(msp, maphienbansp, ten, ram, rom, mausac, slDauKy, slNhap, slXuat, slCuoiKy);
                                result.Add(p);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeTonKho: " + e.Message);
            }
            return result;
        }

        // 2. Doanh thu theo từng năm (Dùng Session Variable @start_year)
        public List<ThongKeDoanhThuDTO> GetDoanhThuTheoTungNam(int yearStart, int yearEnd)
        {
            List<ThongKeDoanhThuDTO> result = new List<ThongKeDoanhThuDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Thiết lập biến session
                    using (MySqlCommand cmdSet = new MySqlCommand("SET @start_year = @s; SET @end_year = @e;", conn))
                    {
                        cmdSet.Parameters.AddWithValue("@s", yearStart);
                        cmdSet.Parameters.AddWithValue("@e", yearEnd);
                        cmdSet.ExecuteNonQuery();
                    }

                    string sqlSelect = @"
                        WITH RECURSIVE years(year) AS (
                            SELECT @start_year
                            UNION ALL
                            SELECT year + 1
                            FROM years
                            WHERE year < @end_year
                        )
                        SELECT 
                            years.year AS nam,
                            COALESCE(SUM(CTPHIEUNHAP.TIENNHAP), 0) AS chiphi, 
                            COALESCE(SUM(CTPHIEUXUAT.TIENXUAT), 0) AS doanhthu
                        FROM years
                        LEFT JOIN PHIEUXUAT ON YEAR(PHIEUXUAT.TG) = years.year
                        LEFT JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                        LEFT JOIN SANPHAM ON SANPHAM.MSP = CTPHIEUXUAT.MSP
                        LEFT JOIN CTPHIEUNHAP ON SANPHAM.MSP = CTPHIEUNHAP.MSP
                        GROUP BY years.year
                        ORDER BY years.year;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlSelect, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int tg = rs.GetInt32("nam");
                                long chiphi = rs.GetInt64("chiphi"); // Dùng GetInt64 cho Long
                                long doanhthu = rs.GetInt64("doanhthu");
                                ThongKeDoanhThuDTO x = new ThongKeDoanhThuDTO(tg, chiphi, doanhthu, doanhthu - chiphi);
                                result.Add(x);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetDoanhThuTheoTungNam: " + e.Message);
            }
            return result;
        }

        // 3. Thống kê khách hàng
        public List<ThongKeKhachHangDTO> GetThongKeKhachHang(string text, DateTime timeStart, DateTime timeEnd)
        {
            List<ThongKeKhachHangDTO> result = new List<ThongKeKhachHangDTO>();
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"
                        WITH kh AS (
                            SELECT KHACHHANG.MKH, KHACHHANG.HOTEN , COUNT(PHIEUXUAT.MPX) AS tongsophieu, SUM(PHIEUXUAT.TIEN) AS tongsotien
                            FROM KHACHHANG
                            JOIN PHIEUXUAT ON KHACHHANG.MKH = PHIEUXUAT.MKH
                            WHERE PHIEUXUAT.TG BETWEEN @start AND @end
                            GROUP BY KHACHHANG.MKH, KHACHHANG.HOTEN
                        )
                        SELECT MKH, HOTEN, COALESCE(kh.tongsophieu, 0) AS SL, COALESCE(kh.tongsotien, 0) AS total 
                        FROM kh WHERE HOTEN LIKE @text OR MKH LIKE @text";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", timeStart);
                        cmd.Parameters.AddWithValue("@end", timeEndFixed);
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mkh = rs.GetInt32("MKH");
                                string tenkh = rs.GetString("HOTEN");
                                int sl = rs.GetInt32("SL");
                                long tien = rs.GetInt64("total");
                                ThongKeKhachHangDTO x = new ThongKeKhachHangDTO(mkh, tenkh, sl, tien);
                                result.Add(x);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeKhachHang: " + e.Message);
            }
            return result;
        }

        // 4. Thống kê nhà cung cấp
        public List<ThongKeNhaCungCapDTO> GetThongKeNCC(string text, DateTime timeStart, DateTime timeEnd)
        {
            List<ThongKeNhaCungCapDTO> result = new List<ThongKeNhaCungCapDTO>();
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"
                        WITH ncc AS (
                            SELECT NHACUNGCAP.MNCC, NHACUNGCAP.TEN, COUNT(PHIEUNHAP.MPN) AS tongsophieu, SUM(PHIEUNHAP.TIEN) AS tongsotien
                            FROM NHACUNGCAP
                            JOIN PHIEUNHAP ON NHACUNGCAP.MNCC = PHIEUNHAP.MNCC
                            WHERE PHIEUNHAP.TG BETWEEN @start AND @end
                            GROUP BY NHACUNGCAP.MNCC, NHACUNGCAP.TEN
                        )
                        SELECT MNCC, TEN, COALESCE(ncc.tongsophieu, 0) AS SL, COALESCE(ncc.tongsotien, 0) AS total 
                        FROM ncc WHERE TEN LIKE @text OR MNCC LIKE @text";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", timeStart);
                        cmd.Parameters.AddWithValue("@end", timeEndFixed);
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mancc = rs.GetInt32("MNCC");
                                string tenncc = rs.GetString("TEN");
                                int sl = rs.GetInt32("SL");
                                long tien = rs.GetInt64("total");
                                ThongKeNhaCungCapDTO x = new ThongKeNhaCungCapDTO(mancc, tenncc, sl, tien);
                                result.Add(x);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeNCC: " + e.Message);
            }
            return result;
        }

        // 5. Thống kê theo tháng (Trong 1 năm)
        public List<ThongKeTheoThangDTO> GetThongKeTheoThang(int nam)
        {
            List<ThongKeTheoThangDTO> result = new List<ThongKeTheoThangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"
                        SELECT months.month AS thang, 
                               COALESCE(SUM(CTPHIEUNHAP.TIENNHAP), 0) AS chiphi,
                               COALESCE(SUM(CTPHIEUXUAT.TIENXUAT), 0) AS doanhthu
                        FROM (
                               SELECT 1 AS month
                               UNION ALL SELECT 2
                               UNION ALL SELECT 3
                               UNION ALL SELECT 4
                               UNION ALL SELECT 5
                               UNION ALL SELECT 6
                               UNION ALL SELECT 7
                               UNION ALL SELECT 8
                               UNION ALL SELECT 9
                               UNION ALL SELECT 10
                               UNION ALL SELECT 11
                               UNION ALL SELECT 12
                             ) AS months
                        LEFT JOIN PHIEUXUAT ON MONTH(PHIEUXUAT.TG) = months.month AND YEAR(PHIEUXUAT.TG) = @nam
                        LEFT JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                        LEFT JOIN SANPHAM ON SANPHAM.MSP = CTPHIEUXUAT.MSP
                        LEFT JOIN CTPHIEUNHAP ON SANPHAM.MSP = CTPHIEUNHAP.MSP
                        GROUP BY months.month
                        ORDER BY months.month;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nam", nam);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int thang = rs.GetInt32("thang");
                                int chiphi = rs.GetInt32("chiphi");
                                int doanhthu = rs.GetInt32("doanhthu");
                                int loinhuan = doanhthu - chiphi;
                                ThongKeTheoThangDTO thongke = new ThongKeTheoThangDTO(thang, chiphi, doanhthu, loinhuan);
                                result.Add(thongke);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeTheoThang: " + e.Message);
            }
            return result;
        }

        // 6. Thống kê từng ngày trong tháng
        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTungNgayTrongThang(int thang, int nam)
        {
            List<ThongKeTungNgayTrongThangDTO> result = new List<ThongKeTungNgayTrongThangDTO>();
            try
            {
                string ngayString = $"{nam}-{thang}-01"; // Format yyyy-MM-dd

                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Lưu ý: Chuỗi SQL này dùng String Interpolation để chèn ngàyString cho khớp logic Java
                    // Vì SQL này generate ra số ngày động dựa trên LAST_DAY
                    string sql = $@"
                        SELECT 
                         dates.date AS ngay, 
                         COALESCE(SUM(CTPHIEUNHAP.TIENNHAP), 0) AS chiphi, 
                         COALESCE(SUM(CTPHIEUXUAT.TIENXUAT), 0) AS doanhthu
                        FROM (
                          SELECT DATE('{ngayString}') + INTERVAL c.number DAY AS date
                          FROM (
                            SELECT 0 AS number UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
                            UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                            UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12 UNION ALL SELECT 13 UNION ALL SELECT 14
                            UNION ALL SELECT 15 UNION ALL SELECT 16 UNION ALL SELECT 17 UNION ALL SELECT 18 UNION ALL SELECT 19
                            UNION ALL SELECT 20 UNION ALL SELECT 21 UNION ALL SELECT 22 UNION ALL SELECT 23 UNION ALL SELECT 24
                            UNION ALL SELECT 25 UNION ALL SELECT 26 UNION ALL SELECT 27 UNION ALL SELECT 28 UNION ALL SELECT 29
                            UNION ALL SELECT 30
                          ) AS c
                          WHERE DATE('{ngayString}') + INTERVAL c.number DAY <= LAST_DAY('{ngayString}')
                        ) AS dates
                        LEFT JOIN PHIEUXUAT ON DATE(PHIEUXUAT.TG) = dates.date
                        LEFT JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                        LEFT JOIN SANPHAM ON SANPHAM.MSP = CTPHIEUXUAT.MSP
                        LEFT JOIN CTPHIEUNHAP ON SANPHAM.MSP = CTPHIEUNHAP.MSP
                        GROUP BY dates.date
                        ORDER BY dates.date;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                DateTime ngay = rs.GetDateTime("ngay");
                                int chiphi = rs.GetInt32("chiphi");
                                int doanhthu = rs.GetInt32("doanhthu");
                                int loinhuan = doanhthu - chiphi;
                                ThongKeTungNgayTrongThangDTO tn = new ThongKeTungNgayTrongThangDTO(ngay, chiphi, doanhthu, loinhuan);
                                result.Add(tn);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeTungNgayTrongThang: " + e.Message);
            }
            return result;
        }

        // 7. Thống kê 7 ngày gần nhất
        public List<ThongKeTungNgayTrongThangDTO> GetThongKe7NgayGanNhat()
        {
            List<ThongKeTungNgayTrongThangDTO> result = new List<ThongKeTungNgayTrongThangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"
                        WITH RECURSIVE dates(date) AS (
                            SELECT DATE_SUB(CURDATE(), INTERVAL 7 DAY)
                            UNION ALL
                            SELECT DATE_ADD(date, INTERVAL 1 DAY)
                            FROM dates
                            WHERE date < CURDATE()
                        )
                        SELECT 
                            dates.date AS ngay,
                            COALESCE(SUM(CTPHIEUXUAT.TIENXUAT), 0) AS doanhthu,
                            COALESCE(SUM(CTPHIEUNHAP.TIENNHAP), 0) AS chiphi
                        FROM dates
                        LEFT JOIN PHIEUXUAT ON DATE(PHIEUXUAT.TG) = dates.date
                        LEFT JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                        LEFT JOIN SANPHAM ON SANPHAM.MSP = CTPHIEUXUAT.MSP
                        LEFT JOIN CTPHIEUNHAP ON SANPHAM.MSP = CTPHIEUNHAP.MSP
                        GROUP BY dates.date
                        ORDER BY dates.date;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                DateTime ngay = rs.GetDateTime("ngay");
                                int chiphi = rs.GetInt32("chiphi");
                                int doanhthu = rs.GetInt32("doanhthu");
                                int loinhuan = doanhthu - chiphi;
                                ThongKeTungNgayTrongThangDTO tn = new ThongKeTungNgayTrongThangDTO(ngay, chiphi, doanhthu, loinhuan);
                                result.Add(tn);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKe7NgayGanNhat: " + e.Message);
            }
            return result;
        }

        // 8. Thống kê từ ngày A đến ngày B
        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTuNgayDenNgay(string start, string end)
        {
            List<ThongKeTungNgayTrongThangDTO> result = new List<ThongKeTungNgayTrongThangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    using (MySqlCommand cmdSet = new MySqlCommand("SET @start_date = @s; SET @end_date = @e;", conn))
                    {
                        cmdSet.Parameters.AddWithValue("@s", start);
                        cmdSet.Parameters.AddWithValue("@e", end);
                        cmdSet.ExecuteNonQuery();
                    }

                    string sqlSelect = @"
                        SELECT 
                         dates.date AS ngay, 
                         COALESCE(SUM(CTPHIEUNHAP.TIENNHAP), 0) AS chiphi, 
                         COALESCE(SUM(CTPHIEUXUAT.TIENXUAT), 0) AS doanhthu
                        FROM (
                          SELECT DATE_ADD(@start_date, INTERVAL c.number DAY) AS date
                          FROM (
                            SELECT a.number + b.number * 31 AS number
                            FROM (
                              SELECT 0 AS number UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
                              UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                              UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12 UNION ALL SELECT 13 UNION ALL SELECT 14
                              UNION ALL SELECT 15 UNION ALL SELECT 16 UNION ALL SELECT 17 UNION ALL SELECT 18 UNION ALL SELECT 19
                              UNION ALL SELECT 20 UNION ALL SELECT 21 UNION ALL SELECT 22 UNION ALL SELECT 23 UNION ALL SELECT 24
                              UNION ALL SELECT 25 UNION ALL SELECT 26 UNION ALL SELECT 27 UNION ALL SELECT 28 UNION ALL SELECT 29
                              UNION ALL SELECT 30
                            ) AS a
                            CROSS JOIN (
                              SELECT 0 AS number UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
                              UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                              UNION ALL SELECT 10
                            ) AS b
                          ) AS c
                          WHERE DATE_ADD(@start_date, INTERVAL c.number DAY) <= @end_date
                        ) AS dates
                        LEFT JOIN PHIEUXUAT ON DATE(PHIEUXUAT.TG) = dates.date
                        LEFT JOIN CTPHIEUXUAT ON PHIEUXUAT.MPX = CTPHIEUXUAT.MPX
                        LEFT JOIN SANPHAM ON SANPHAM.MSP = CTPHIEUXUAT.MSP
                        LEFT JOIN CTPHIEUNHAP ON SANPHAM.MSP = CTPHIEUNHAP.MSP
                        GROUP BY dates.date
                        ORDER BY dates.date;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlSelect, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                DateTime ngay = rs.GetDateTime("ngay");
                                int chiphi = rs.GetInt32("chiphi");
                                int doanhthu = rs.GetInt32("doanhthu");
                                int loinhuan = doanhthu - chiphi;
                                ThongKeTungNgayTrongThangDTO tn = new ThongKeTungNgayTrongThangDTO(ngay, chiphi, doanhthu, loinhuan);
                                result.Add(tn);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi GetThongKeTuNgayDenNgay: " + e.Message);
            }
            return result;
        }
    }
}