using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class NhanVienDAO : DAOinterface<NhanVienDTO>
    {
        // Singleton Pattern
        private static NhanVienDAO instance;
        public static NhanVienDAO Instance
        {
            get
            {
                if (instance == null) instance = new NhanVienDAO();
                return instance;
            }
        }

        private NhanVienDAO() { }

        // 1. Thêm nhân viên (INSERT)
        public int insert(NhanVienDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MNV tự tăng
                    string sql = "INSERT INTO NHANVIEN (HOTEN, GIOITINH, SDT, NGAYSINH, TT, EMAIL) VALUES (@hoten, @gioitinh, @sdt, @ngaysinh, @tt, @email)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hoten", t.HOTEN);
                        cmd.Parameters.AddWithValue("@gioitinh", t.GIOITINH);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@ngaysinh", t.NGAYSINH); // DateTime
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert NhanVien: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật nhân viên (UPDATE)
        public int update(NhanVienDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHANVIEN SET HOTEN=@hoten, GIOITINH=@gioitinh, NGAYSINH=@ngaysinh, SDT=@sdt, TT=@tt, EMAIL=@email WHERE MNV=@mnv";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hoten", t.HOTEN);
                        cmd.Parameters.AddWithValue("@gioitinh", t.GIOITINH);
                        cmd.Parameters.AddWithValue("@ngaysinh", t.NGAYSINH);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update NhanVien: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa nhân viên (DELETE MỀM - UPDATE TT=-1)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Code Java set TT = -1 khi xóa
                    string sql = "UPDATE NHANVIEN SET TT = -1 WHERE MNV = @mnv";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete NhanVien: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy danh sách nhân viên đang hoạt động (TT=1)
        public List<NhanVienDTO> selectAll()
        {
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHANVIEN WHERE TT = 1";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string hoten = rs.GetString("HOTEN");
                                int gioitinh = rs.GetInt32("GIOITINH");
                                DateTime ngaysinh = rs.GetDateTime("NGAYSINH");
                                string sdt = rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                string email = rs.GetString("EMAIL");
                                
                                // DTO Constructor: (mnv, hoten, gioitinh, ngaysinh, sdt, tt, email)
                                NhanVienDTO nv = new NhanVienDTO(mnv, hoten, gioitinh, ngaysinh, sdt, tt, email);
                                result.Add(nv);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll NhanVien: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy TẤT CẢ nhân viên (Kể cả đã xóa) - Tương ứng selectAlll()
        public List<NhanVienDTO> SelectAllFull()
        {
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHANVIEN";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string hoten = rs.GetString("HOTEN");
                                int gioitinh = rs.GetInt32("GIOITINH");
                                DateTime ngaysinh = rs.GetDateTime("NGAYSINH");
                                string sdt = rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                string email = rs.GetString("EMAIL");
                                
                                NhanVienDTO nv = new NhanVienDTO(mnv, hoten, gioitinh, ngaysinh, sdt, tt, email);
                                result.Add(nv);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAllFull NhanVien: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy nhân viên CHƯA CÓ TÀI KHOẢN (selectAllNV)
        public List<NhanVienDTO> SelectAllNVNoAccount()
        {
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Lấy nhân viên hoạt động (TT=1) và chưa có trong bảng TAIKHOAN
                    string sql = "SELECT * FROM NHANVIEN nv WHERE nv.TT = 1 AND NOT EXISTS(SELECT * FROM TAIKHOAN tk WHERE nv.MNV = tk.MNV)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string hoten = rs.GetString("HOTEN");
                                int gioitinh = rs.GetInt32("GIOITINH");
                                DateTime ngaysinh = rs.GetDateTime("NGAYSINH");
                                string sdt = rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                string email = rs.GetString("EMAIL");
                                
                                NhanVienDTO nv = new NhanVienDTO(mnv, hoten, gioitinh, ngaysinh, sdt, tt, email);
                                result.Add(nv);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAllNVNoAccount NhanVien: " + ex.Message);
            }
            return result;
        }

        // 7. Lấy nhân viên theo ID
        public NhanVienDTO selectById(string t)
        {
            NhanVienDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHANVIEN WHERE MNV = @mnv";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string hoten = rs.GetString("HOTEN");
                                int gioitinh = rs.GetInt32("GIOITINH");
                                DateTime ngaysinh = rs.GetDateTime("NGAYSINH");
                                string sdt = rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                string email = rs.GetString("EMAIL");
                                
                                result = new NhanVienDTO(mnv, hoten, gioitinh, ngaysinh, sdt, tt, email);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById NhanVien: " + ex.Message);
            }
            return result;
        }

        // 8. Lấy nhân viên theo Email
        public NhanVienDTO selectByEmail(string t)
        {
            NhanVienDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHANVIEN WHERE EMAIL = @email";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string hoten = rs.GetString("HOTEN");
                                int gioitinh = rs.GetInt32("GIOITINH");
                                DateTime ngaysinh = rs.GetDateTime("NGAYSINH");
                                string sdt = rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                string email = rs.GetString("EMAIL");
                                
                                result = new NhanVienDTO(mnv, hoten, gioitinh, ngaysinh, sdt, tt, email);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectByEmail NhanVien: " + ex.Message);
            }
            return result;
        }

        // 9. Lấy Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Đã đổi tên DB thành 'quanlykho'
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'NHANVIEN'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement NhanVien: " + ex.Message);
            }
            return result;
        }
    }
}