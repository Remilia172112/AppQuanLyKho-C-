using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class SanPhamDAO : DAOinterface<SanPhamDTO>
    {
        private static SanPhamDAO instance;
        public static SanPhamDAO Instance
        {
            get
            {
                if (instance == null) instance = new SanPhamDAO();
                return instance;
            }
        }

        private SanPhamDAO() { }

        // 1. Thêm sản phẩm mới (INSERT)
        public int insert(SanPhamDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MSP tự tăng, không cần insert
                    // Các cột mới: MSX (Nhà SX), MKVK (Khu vực), MLSP (Loại SP)
                    string sql = @"INSERT INTO SANPHAM 
                        (TEN, HINHANH, DANHMUC, MSX, MKVK, MLSP, TIENX, TIENN, SL, TT) 
                        VALUES 
                        (@ten, @hinhanh, @danhmuc, @msx, @mkvk, @mlsp, @tienx, @tienn, @sl, 1)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@hinhanh", t.HINHANH);
                        cmd.Parameters.AddWithValue("@danhmuc", t.DANHMUC);
                        cmd.Parameters.AddWithValue("@msx", t.MSX);
                        cmd.Parameters.AddWithValue("@mkvk", t.MKVK);
                        cmd.Parameters.AddWithValue("@mlsp", t.MLSP);
                        cmd.Parameters.AddWithValue("@tienx", t.TIENX);
                        cmd.Parameters.AddWithValue("@tienn", t.TIENN);
                        cmd.Parameters.AddWithValue("@sl", t.SL);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert SanPham: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật thông tin sản phẩm (UPDATE)
        public int update(SanPhamDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"UPDATE SANPHAM SET 
                        TEN = @ten, 
                        HINHANH = @hinhanh, 
                        DANHMUC = @danhmuc, 
                        MSX = @msx, 
                        MKVK = @mkvk, 
                        MLSP = @mlsp, 
                        TIENX = @tienx, 
                        TIENN = @tienn, 
                        SL = @sl
                        WHERE MSP = @msp";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@hinhanh", t.HINHANH);
                        cmd.Parameters.AddWithValue("@danhmuc", t.DANHMUC);
                        cmd.Parameters.AddWithValue("@msx", t.MSX);
                        cmd.Parameters.AddWithValue("@mkvk", t.MKVK);
                        cmd.Parameters.AddWithValue("@mlsp", t.MLSP);
                        cmd.Parameters.AddWithValue("@tienx", t.TIENX);
                        cmd.Parameters.AddWithValue("@tienn", t.TIENN);
                        cmd.Parameters.AddWithValue("@sl", t.SL);
                        cmd.Parameters.AddWithValue("@msp", t.MSP);

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update SanPham: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa sản phẩm (Xóa mềm TT=0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE SANPHAM SET TT = 0 WHERE MSP = @msp";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@msp", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete SanPham: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả sản phẩm đang hoạt động
        public List<SanPhamDTO> selectAll()
        {
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM SANPHAM WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                SanPhamDTO sp = new SanPhamDTO();
                                sp.MSP = rs.GetInt32("MSP");
                                sp.TEN = rs.GetString("TEN");
                                sp.HINHANH = rs.IsDBNull(rs.GetOrdinal("HINHANH")) ? "" : rs.GetString("HINHANH");
                                sp.DANHMUC = rs.IsDBNull(rs.GetOrdinal("DANHMUC")) ? "" : rs.GetString("DANHMUC");
                                
                                // Các cột khóa ngoại mới
                                sp.MSX = rs.GetInt32("MSX");
                                sp.MKVK = rs.GetInt32("MKVK");
                                sp.MLSP = rs.GetInt32("MLSP");
                                
                                sp.TIENX = rs.GetInt32("TIENX");
                                sp.TIENN = rs.GetInt32("TIENN");
                                sp.SL = rs.GetInt32("SL");
                                sp.TT = rs.GetInt32("TT");

                                result.Add(sp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll SanPham: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy sản phẩm theo ID
        public SanPhamDTO selectById(string t)
        {
            SanPhamDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM SANPHAM WHERE MSP = @msp";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@msp", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                result = new SanPhamDTO();
                                result.MSP = rs.GetInt32("MSP");
                                result.TEN = rs.GetString("TEN");
                                result.HINHANH = rs.IsDBNull(rs.GetOrdinal("HINHANH")) ? "" : rs.GetString("HINHANH");
                                result.DANHMUC = rs.IsDBNull(rs.GetOrdinal("DANHMUC")) ? "" : rs.GetString("DANHMUC");
                                result.MSX = rs.GetInt32("MSX");
                                result.MKVK = rs.GetInt32("MKVK");
                                result.MLSP = rs.GetInt32("MLSP");
                                result.TIENX = rs.GetInt32("TIENX");
                                result.TIENN = rs.GetInt32("TIENN");
                                result.SL = rs.GetInt32("SL");
                                result.TT = rs.GetInt32("TT");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById SanPham: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy sản phẩm theo Danh mục (Có thể là tên danh mục hoặc MLSP)
        // Code Java cũ đang select theo cột `DANHMUC` (String)
        public List<SanPhamDTO> selectByDanhMuc(string t)
        {
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM SANPHAM WHERE DANHMUC = @danhmuc AND TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@danhmuc", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                SanPhamDTO sp = new SanPhamDTO();
                                sp.MSP = rs.GetInt32("MSP");
                                sp.TEN = rs.GetString("TEN");
                                sp.HINHANH = rs.IsDBNull(rs.GetOrdinal("HINHANH")) ? "" : rs.GetString("HINHANH");
                                sp.DANHMUC = rs.IsDBNull(rs.GetOrdinal("DANHMUC")) ? "" : rs.GetString("DANHMUC");
                                sp.MSX = rs.GetInt32("MSX");
                                sp.MKVK = rs.GetInt32("MKVK");
                                sp.MLSP = rs.GetInt32("MLSP");
                                sp.TIENX = rs.GetInt32("TIENX");
                                sp.TIENN = rs.GetInt32("TIENN");
                                sp.SL = rs.GetInt32("SL");
                                sp.TT = rs.GetInt32("TT");
                                result.Add(sp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi selectByDanhMuc SanPham: " + ex.Message);
            }
            return result;
        }

        // 7. Lấy Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'SANPHAM'";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                result = rs.IsDBNull(0) ? 1 : rs.GetInt32("AUTO_INCREMENT");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi getAutoIncrement SanPham: " + ex.Message);
            }
            return result;
        }

        // 8. Cập nhật số lượng tồn kho (Dùng cho Nhập/Xuất hàng)
        // Logic: SL Mới = SL Cũ + Biến động (Nhập là +, Xuất là -)
        public int UpdateSoLuongTon(int MSP, int soluong)
        {
            int result = 0;
            try
            {
                // Bước 1: Lấy số lượng hiện tại
                SanPhamDTO currentSp = this.selectById(MSP.ToString());
                if (currentSp == null) return 0;

                int quantity_current = currentSp.SL;
                int quantity_change = quantity_current + soluong; // Cộng dồn (số âm sẽ thành trừ)

                // Bước 2: Update vào DB
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE SANPHAM SET SL = @sl WHERE MSP = @msp";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sl", quantity_change);
                        cmd.Parameters.AddWithValue("@msp", MSP);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpdateSoLuongTon SanPham: " + ex.Message);
            }
            return result;
        }
    }
}