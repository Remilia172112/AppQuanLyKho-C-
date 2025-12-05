using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class ChiTietQuyenDAO : ChiTietInterface<ChiTietQuyenDTO>
    {
        // Singleton Pattern
        private static ChiTietQuyenDAO instance;
        public static ChiTietQuyenDAO Instance
        {
            get
            {
                if (instance == null) instance = new ChiTietQuyenDAO();
                return instance;
            }
        }

        private ChiTietQuyenDAO() { }

        // 1. Xóa tất cả quyền của một nhóm (Theo MNQ)
        // SQL: DELETE FROM CTQUYEN WHERE MNQ = ...
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM CTQUYEN WHERE MNQ = @mnq";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnq", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete CTQuyen: " + ex.Message);
            }
            return result;
        }

        // 2. Thêm danh sách quyền vào nhóm
        // SQL: INSERT INTO CTQUYEN (MNQ, MCN, HANHDONG) ...
        public int insert(List<ChiTietQuyenDTO> t)
        {
            int result = 0;
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                foreach (var item in t)
                {
                    try
                    {
                        string sql = "INSERT INTO CTQUYEN (MNQ, MCN, HANHDONG) VALUES (@mnq, @mcn, @hanhdong)";
                        
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            // Sử dụng Properties đã tạo ở ChiTietQuyenDTO.cs
                            cmd.Parameters.AddWithValue("@mnq", item.Manhomquyen);
                            cmd.Parameters.AddWithValue("@mcn", item.Machucnang);
                            cmd.Parameters.AddWithValue("@hanhdong", item.Hanhdong);

                            result += cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi Insert CTQuyen: " + ex.Message);
                    }
                }
            }
            return result;
        }

        // 3. Lấy danh sách quyền của một nhóm (Theo MNQ)
        // SQL: SELECT * FROM CTQUYEN WHERE MNQ = ...
        public List<ChiTietQuyenDTO> selectAll(string t)
        {
            List<ChiTietQuyenDTO> result = new List<ChiTietQuyenDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM CTQUYEN WHERE MNQ = @mnq";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnq", t);
                        
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mnq = rs.GetInt32("MNQ");
                                string mcn = rs.GetString("MCN");
                                string hanhdong = rs.GetString("HANHDONG");

                                ChiTietQuyenDTO quyen = new ChiTietQuyenDTO(mnq, mcn, hanhdong);
                                result.Add(quyen);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll CTQuyen: " + ex.Message);
            }
            return result;
        }

        // 4. Cập nhật quyền = Xóa hết cũ -> Thêm mới
        public int update(List<ChiTietQuyenDTO> t, string pk)
        {
            // Bước 1: Xóa sạch quyền cũ của nhóm này
            int result = this.delete(pk);
            
            // Bước 2: Thêm lại danh sách quyền mới (nếu xóa không lỗi)
            if (result >= 0) 
            {
                result = this.insert(t);
            }
            return result;
        }
    }
}