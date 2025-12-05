using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class KhachHangDAO : DAOinterface<KhachHangDTO>
    {
        private static KhachHangDAO instance;
        public static KhachHangDAO Instance
        {
            get
            {
                if (instance == null) instance = new KhachHangDAO();
                return instance;
            }
        }

        private KhachHangDAO() { }

        // 1. Thêm khách hàng (INSERT)
        public int insert(KhachHangDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Lưu ý: Nếu MKH là Auto Increment thì thường không cần insert MKH (trừ khi giá trị là 0)
                    string sql = "INSERT INTO KHACHHANG (MKH, HOTEN, DIACHI, SDT, EMAIL, NGAYTHAMGIA, TT) VALUES (@mkh, @hoten, @diachi, @sdt, @email, @ngaythamgia, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkh", t.MKH);
                        cmd.Parameters.AddWithValue("@hoten", t.HOTEN);
                        cmd.Parameters.AddWithValue("@diachi", t.DIACHI);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@ngaythamgia", DateTime.Now); 

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert KhachHang: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật khách hàng (UPDATE)
        public int update(KhachHangDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Đã sửa lỗi thiếu dấu phẩy trước EMAIL
                    string sql = "UPDATE KHACHHANG SET MKH=@mkh, HOTEN=@hoten, DIACHI=@diachi, SDT=@sdt, EMAIL=@email WHERE MKH=@mkh_cond";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkh", t.MKH);
                        cmd.Parameters.AddWithValue("@hoten", t.HOTEN);
                        cmd.Parameters.AddWithValue("@diachi", t.DIACHI);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@mkh_cond", t.MKH); // Điều kiện WHERE

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update KhachHang: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa khách hàng (DELETE MỀM - Update TT=0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE KHACHHANG SET TT=0 WHERE MKH = @mkh";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkh", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete KhachHang: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy danh sách khách hàng đang hoạt động (TT = 1)
        public List<KhachHangDTO> selectAll()
        {
            List<KhachHangDTO> result = new List<KhachHangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM KHACHHANG WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mkh = rs.GetInt32("MKH");
                                string hoten = rs.GetString("HOTEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                string sdt = rs.GetString("SDT");
                                DateTime ngaythamgia = rs.GetDateTime("NGAYTHAMGIA");
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");

                                // Giả sử DTO có constructor đầy đủ
                                KhachHangDTO kh = new KhachHangDTO(mkh, hoten, ngaythamgia, diachi, sdt, email, 1);
                                result.Add(kh);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll KhachHang: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy TẤT CẢ khách hàng (Bao gồm đã xóa)
        // Hàm này tương ứng với selectAlll() trong Java
        public List<KhachHangDTO> SelectAllFull()
        {
            List<KhachHangDTO> result = new List<KhachHangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM KHACHHANG";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mkh = rs.GetInt32("MKH");
                                string hoten = rs.GetString("HOTEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                string sdt = rs.GetString("SDT");
                                DateTime ngaythamgia = rs.GetDateTime("NGAYTHAMGIA");
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                int tt = rs.GetInt32("TT");

                                KhachHangDTO kh = new KhachHangDTO(mkh, hoten, ngaythamgia, diachi, sdt, email, tt);
                                result.Add(kh);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAllFull KhachHang: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy khách hàng theo ID
        public KhachHangDTO selectById(string t)
        {
            KhachHangDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM KHACHHANG WHERE MKH = @mkh";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkh", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mkh = rs.GetInt32("MKH");
                                string hoten = rs.GetString("HOTEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                string sdt = rs.GetString("SDT");
                                DateTime ngaythamgia = rs.GetDateTime("NGAYTHAMGIA");
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                int tt = rs.GetInt32("TT");

                                result = new KhachHangDTO(mkh, hoten, ngaythamgia, diachi, sdt, email, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById KhachHang: " + ex.Message);
            }
            return result;
        }

        // 7. Lấy giá trị Auto Increment tiếp theo
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'KHACHHANG'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement KhachHang: " + ex.Message);
            }
            return result;
        }
    }
}