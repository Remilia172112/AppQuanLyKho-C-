using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class NhaCungCapDAO : DAOinterface<NhaCungCapDTO>
    {
        private static NhaCungCapDAO instance;
        public static NhaCungCapDAO Instance
        {
            get
            {
                if (instance == null) instance = new NhaCungCapDAO();
                return instance;
            }
        }

        private NhaCungCapDAO() { }

        // 1. Thêm nhà cung cấp (INSERT)
        public int insert(NhaCungCapDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "INSERT INTO NHACUNGCAP(MNCC, TEN, DIACHI, EMAIL, SDT, TT) VALUES (?, ?, ?, ?, ?, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandText = "INSERT INTO NHACUNGCAP(MNCC, TEN, DIACHI, EMAIL, SDT, TT) VALUES (@mncc, @ten, @diachi, @email, @sdt, 1)";

                        cmd.Parameters.AddWithValue("@mncc", t.MNCC);
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
                Console.WriteLine("Lỗi Insert NhaCungCap: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật nhà cung cấp (UPDATE)
        public int update(NhaCungCapDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHACUNGCAP SET TEN=@ten, DIACHI=@diachi, EMAIL=@email, SDT=@sdt WHERE MNCC=@mncc";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@diachi", t.DIACHI);
                        cmd.Parameters.AddWithValue("@email", t.EMAIL);
                        cmd.Parameters.AddWithValue("@sdt", t.SDT);
                        cmd.Parameters.AddWithValue("@mncc", t.MNCC);

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update NhaCungCap: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa nhà cung cấp (Xóa mềm - TT = 0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHACUNGCAP SET TT = 0 WHERE MNCC = @mncc";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mncc", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete NhaCungCap: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy danh sách nhà cung cấp đang hoạt động (TT = 1)
        public List<NhaCungCapDTO> selectAll()
        {
            List<NhaCungCapDTO> result = new List<NhaCungCapDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHACUNGCAP WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mncc = rs.GetInt32("MNCC");
                                string ten = rs.GetString("TEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                string sdt = rs.IsDBNull(rs.GetOrdinal("SDT")) ? "" : rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");
                                NhaCungCapDTO ncc = new NhaCungCapDTO(mncc, ten, diachi, email, sdt, tt);
                                result.Add(ncc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll NhaCungCap: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy nhà cung cấp theo ID
        public NhaCungCapDTO selectById(string t)
        {
            NhaCungCapDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHACUNGCAP WHERE MNCC = @mncc";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mncc", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mncc = rs.GetInt32("MNCC");
                                string ten = rs.GetString("TEN");
                                string diachi = rs.IsDBNull(rs.GetOrdinal("DIACHI")) ? "" : rs.GetString("DIACHI");
                                
                                string email = rs.IsDBNull(rs.GetOrdinal("EMAIL")) ? "" : rs.GetString("EMAIL");
                                string sdt = rs.IsDBNull(rs.GetOrdinal("SDT")) ? "" : rs.GetString("SDT");
                                int tt = rs.GetInt32("TT");

                                result = new NhaCungCapDTO(mncc, ten, diachi, email, sdt, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById NhaCungCap: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy giá trị Auto Increment tiếp theo
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'NHACUNGCAP'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement NhaCungCap: " + ex.Message);
            }
            return result;
        }
    }
}