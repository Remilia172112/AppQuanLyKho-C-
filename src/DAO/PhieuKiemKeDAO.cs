using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class PhieuKiemKeDAO : DAOinterface<PhieuKiemKeDTO>
    {
        private static PhieuKiemKeDAO instance;
        public static PhieuKiemKeDAO Instance
        {
            get
            {
                if (instance == null) instance = new PhieuKiemKeDAO();
                return instance;
            }
        }

        private PhieuKiemKeDAO() { }

        // 1. Thêm phiếu kiểm kê (INSERT)
        public int insert(PhieuKiemKeDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MPKK là Auto Increment, TT mặc định là 1
                    string sql = "INSERT INTO PHIEUKIEMKE (MNV, TG, TT) VALUES (@mnv, @tg, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@tg", t.TG); // DateTime
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật (UPDATE) - Chưa hỗ trợ
        public int update(PhieuKiemKeDTO t)
        {
            throw new NotImplementedException("Chức năng cập nhật phiếu kiểm kê chưa được hỗ trợ.");
        }

        // 3. Xóa phiếu kiểm kê (DELETE CỨNG - theo code Java cũ)
        // Nếu muốn xóa mềm thì sửa thành: UPDATE PHIEUKIEMKE SET TT=0 WHERE MPKK=@mpkk
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM PHIEUKIEMKE WHERE MPKK = @mpkk";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả phiếu kiểm kê
        public List<PhieuKiemKeDTO> selectAll()
        {
            List<PhieuKiemKeDTO> result = new List<PhieuKiemKeDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUKIEMKE ORDER BY MPKK DESC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpkk = rs.GetInt32("MPKK");
                                int mnv = rs.GetInt32("MNV");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");
                                
                                // DTO có constructor (int mpkk, int mnv, DateTime tg, int tt)
                                PhieuKiemKeDTO pkk = new PhieuKiemKeDTO(mpkk, mnv, tg, tt);
                                result.Add(pkk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy phiếu theo ID - Chưa hỗ trợ trong Java cũ, nhưng mình viết luôn cho đủ bộ
        public PhieuKiemKeDTO selectById(string t)
        {
            PhieuKiemKeDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUKIEMKE WHERE MPKK = @mpkk";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mpkk = rs.GetInt32("MPKK");
                                int mnv = rs.GetInt32("MNV");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");
                                
                                result = new PhieuKiemKeDTO(mpkk, mnv, tg, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy giá trị Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'PHIEUKIEMKE'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement PhieuKiemKe: " + ex.Message);
            }
            return result;
        }
    }
}