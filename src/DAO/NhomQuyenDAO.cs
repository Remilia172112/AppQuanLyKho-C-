using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class NhomQuyenDAO : DAOinterface<NhomQuyenDTO>
    {
        private static NhomQuyenDAO instance;
        public static NhomQuyenDAO Instance
        {
            get
            {
                if (instance == null) instance = new NhomQuyenDAO();
                return instance;
            }
        }

        private NhomQuyenDAO() { }

        // 1. Thêm nhóm quyền (INSERT)
        public int insert(NhomQuyenDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MNQ tự tăng, TT mặc định là 1
                    string sql = "INSERT INTO NHOMQUYEN (TEN, TT) VALUES (@ten, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.Tennhomquyen);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert NhomQuyen: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật tên nhóm quyền (UPDATE)
        public int update(NhomQuyenDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHOMQUYEN SET TEN = @ten WHERE MNQ = @mnq";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.Tennhomquyen);
                        cmd.Parameters.AddWithValue("@mnq", t.Manhomquyen);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update NhomQuyen: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa nhóm quyền (Xóa mềm - UPDATE TT = 0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE NHOMQUYEN SET TT = 0 WHERE MNQ = @mnq";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnq", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete NhomQuyen: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả nhóm quyền đang hoạt động
        public List<NhomQuyenDTO> selectAll()
        {
            List<NhomQuyenDTO> result = new List<NhomQuyenDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHOMQUYEN WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnq = rs.GetInt32("MNQ");
                                string ten = rs.GetString("TEN");
                                
                                // DTO có constructor (int mnq, string ten)
                                NhomQuyenDTO nq = new NhomQuyenDTO(mnq, ten);
                                result.Add(nq);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll NhomQuyen: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy nhóm quyền theo ID
        public NhomQuyenDTO selectById(string t)
        {
            NhomQuyenDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM NHOMQUYEN WHERE MNQ = @mnq";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnq", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mnq = rs.GetInt32("MNQ");
                                string ten = rs.GetString("TEN");
                                
                                result = new NhomQuyenDTO(mnq, ten);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById NhomQuyen: " + ex.Message);
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
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'NHOMQUYEN'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement NhomQuyen: " + ex.Message);
            }
            return result;
        }
    }
}