using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class NhaSanXuatDAO : DAOinterface<NhaSanXuatDTO>
    {
        private static NhaSanXuatDAO instance;
        public static NhaSanXuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new NhaSanXuatDAO();
                return instance;
            }
        }

        private NhaSanXuatDAO() { }

        // 1. Thêm Nhà sản xuất (INSERT)
        public int insert(NhaSanXuatDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Code Java insert cả ID, mình giữ nguyên logic này
                    string sql = "INSERT INTO NHASANXUAT(MNSX, TEN, DIACHI, EMAIL, SDT, TT) VALUES (@mnsx, @ten, @diachi, @email, @sdt, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnsx", t.MNSX);
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@diachi", t.DIACHI);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert NhaSanXuat: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật Nhà sản xuất (UPDATE)
        public int update(NhaSanXuatDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHASANXUAT SET TEN=@ten, DIACHI=@diachi, EMAIL=@email, SDT=@sdt WHERE MNSX=@mnsx";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@diachi", t.DIACHI);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@mnsx", t.MNSX);

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update NhaSanXuat: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa Nhà sản xuất (DELETE MỀM)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHASANXUAT SET TT = 0 WHERE MNSX = @mnsx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnsx", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete NhaSanXuat: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy danh sách đang hoạt động (SELECT ALL)
        public List<NhaSanXuatDTO> selectAll()
        {
            List<NhaSanXuatDTO> result = new List<NhaSanXuatDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHASANXUAT WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnsx = rs.GetInt32("MNSX");
                                string ten = rs.GetString("TEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                string sdt = rs.IsDBNull(rs.GetOrdinal("SDT")) ? "" : rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");

                                NhaSanXuatDTO nsx = new NhaSanXuatDTO(mnsx, ten, diachi, email, sdt, tt);
                                result.Add(nsx);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll NhaSanXuat: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy theo ID
        public NhaSanXuatDTO selectById(string t)
        {
            NhaSanXuatDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHASANXUAT WHERE MNSX = @mnsx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnsx", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnsx = rs.GetInt32("MNSX");
                                string ten = rs.GetString("TEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                
                                // Đã sửa lỗi logic của Java: Lấy đúng cột EMAIL
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                string sdt = rs.IsDBNull(rs.GetOrdinal("SDT")) ? "" : rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");

                                result = new NhaSanXuatDTO(mnsx, ten, diachi, email, sdt, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById NhaSanXuat: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'NHASANXUAT'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement NhaSanXuat: " + ex.Message);
            }
            return result;
        }
    }
}