using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;
using src.Helper;

namespace src.DAO
{
    public class TaiKhoanDAO : DAOinterface<TaiKhoanDTO>
    {
        // Singleton Pattern
        private static TaiKhoanDAO instance;
        public static TaiKhoanDAO Instance
        {
            get
            {
                if (instance == null) instance = new TaiKhoanDAO();
                return instance;
            }
        }

        private TaiKhoanDAO() { }

        // 1. Thêm tài khoản mới (Có mã hóa mật khẩu)
        public int insert(TaiKhoanDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "INSERT INTO TAIKHOAN (MNV, TDN, MK, MNQ, TT) VALUES (@mnv, @tdn, @mk, @mnq, @tt)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@tdn", t.TDN);
                        
                        // Sử dụng BCrypt từ src.Helper để mã hóa
                        // Logic giống hệt Java: BCrypt.hashpw(t.getMK(), BCrypt.gensalt(12))
                        string hashedPassword = BCrypts.hashpw(t.MK, BCrypts.gensalt(12));
                        cmd.Parameters.AddWithValue("@mk", hashedPassword);
                        
                        cmd.Parameters.AddWithValue("@mnq", t.MNQ);
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật thông tin (Không đổi mật khẩu)
        public int update(TaiKhoanDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE TAIKHOAN SET TDN = @tdn, TT = @tt, MNQ = @mnq WHERE MNV = @mnv";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tdn", t.TDN);
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        cmd.Parameters.AddWithValue("@mnq", t.MNQ);
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 3. Cập nhật trạng thái chờ xử lý (TT=2) theo Email
        public int updateTTCXL(string email)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE TAIKHOAN TK JOIN NHANVIEN NV ON TK.MNV = NV.MNV SET TK.TT = 2 WHERE NV.EMAIL = @email";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpdateTTCXL TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 4. Đổi mật khẩu mới (Có mã hóa lại)
        public void updatePass(string email, string newPassword)
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE TAIKHOAN TK JOIN NHANVIEN NV ON TK.MNV = NV.MNV SET TK.MK = @mk WHERE NV.EMAIL = @email";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        // Mã hóa mật khẩu mới trước khi lưu
                        string hashedPassword = BCrypts.hashpw(newPassword, BCrypts.gensalt(12));
                        cmd.Parameters.AddWithValue("@mk", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", email);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpdatePass TaiKhoan: " + ex.Message);
            }
        }

        // 5. Tìm tài khoản theo Email (Join với bảng NhanVien)
        public TaiKhoanDTO selectByEmail(string email)
        {
            TaiKhoanDTO tk = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Lấy tất cả cột của TaiKhoan
                    string sql = "SELECT TK.* FROM TAIKHOAN TK JOIN NHANVIEN NV ON TK.MNV = NV.MNV WHERE NV.EMAIL = @email";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string tdn = rs.GetString("TDN");
                                string mk = rs.GetString("MK");
                                int tt = rs.GetInt32("TT");
                                int mnq = rs.GetInt32("MNQ");
                                
                                tk = new TaiKhoanDTO(mnv, tdn, mk, mnq, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectByEmail TaiKhoan: " + ex.Message);
            }
            return tk;
        }

        // 6. Gửi OTP (Lưu vào DB)
        public void sendOpt(string email, string otp)
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE TAIKHOAN TK JOIN NHANVIEN NV ON TK.MNV = NV.MNV SET TK.OTP = @otp WHERE NV.EMAIL = @email";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@otp", otp);
                        cmd.Parameters.AddWithValue("@email", email);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SendOTP TaiKhoan: " + ex.Message);
            }
        }

        // 7. Kiểm tra OTP
        public bool checkOtp(string email, string otp)
        {
            bool check = false;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM TAIKHOAN TK JOIN NHANVIEN NV ON TK.MNV = NV.MNV WHERE NV.EMAIL = @email AND TK.OTP = @otp";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@otp", otp);
                        
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.HasRows) // Nếu tìm thấy dòng nào khớp thì return true
                            {
                                check = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi CheckOTP TaiKhoan: " + ex.Message);
            }
            return check;
        }

        // 8. Xóa tài khoản (Khóa tài khoản: TT = -1)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE TAIKHOAN SET TT = -1 WHERE MNV = @mnv";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 9. Lấy danh sách tất cả tài khoản (Trừ TT = -1)
        public List<TaiKhoanDTO> selectAll()
        {
            List<TaiKhoanDTO> result = new List<TaiKhoanDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Lấy TT = 0 (Khóa?), 1 (Hoạt động), 2 (Chờ xử lý)
                    string sql = "SELECT * FROM TAIKHOAN WHERE TT IN (0, 1, 2)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string tdn = rs.GetString("TDN");
                                string mk = rs.GetString("MK");
                                int mnq = rs.GetInt32("MNQ");
                                int tt = rs.GetInt32("TT");
                                
                                TaiKhoanDTO tk = new TaiKhoanDTO(mnv, tdn, mk, mnq, tt);
                                result.Add(tk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 10. Lấy tài khoản theo Mã nhân viên (ID)
        public TaiKhoanDTO selectById(string t)
        {
            TaiKhoanDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM TAIKHOAN WHERE MNV = @mnv";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string tdn = rs.GetString("TDN");
                                string mk = rs.GetString("MK");
                                int mnq = rs.GetInt32("MNQ");
                                int tt = rs.GetInt32("TT");
                                
                                result = new TaiKhoanDTO(mnv, tdn, mk, mnq, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 11. Lấy tài khoản theo Tên đăng nhập (TDN) - Dùng cho Đăng Nhập
        public TaiKhoanDTO selectByUser(string username)
        {
            TaiKhoanDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM TAIKHOAN WHERE TDN = @tdn";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tdn", username);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnv = rs.GetInt32("MNV");
                                string tdn = rs.GetString("TDN");
                                string mk = rs.GetString("MK");
                                int mnq = rs.GetInt32("MNQ");
                                int tt = rs.GetInt32("TT");
                                
                                result = new TaiKhoanDTO(mnv, tdn, mk, mnq, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectByUser TaiKhoan: " + ex.Message);
            }
            return result;
        }

        // 12. Lấy giá trị Auto Increment (Dù bảng TaiKhoan không có AutoInc, nó dùng khóa ngoại MNV)
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Đổi tên DB thành 'quanlykho'
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'TAIKHOAN'";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                // Xử lý nếu NULL
                                result = rs.IsDBNull(0) ? 1 : rs.GetInt32("AUTO_INCREMENT");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi getAutoIncrement TaiKhoan: " + ex.Message);
            }
            return result;
        }
    }
}