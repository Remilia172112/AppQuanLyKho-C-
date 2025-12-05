using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class KhuVucKhoDAO : DAOinterface<KhuVucKhoDTO>
    {
        private static KhuVucKhoDAO instance;
        public static KhuVucKhoDAO Instance
        {
            get
            {
                if (instance == null) instance = new KhuVucKhoDAO();
                return instance;
            }
        }

        private KhuVucKhoDAO() { }

        // 1. Thêm khu vực kho
        public int insert(KhuVucKhoDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "INSERT INTO KHUVUCKHO (MKVK, TEN, GHICHU, TT) VALUES (@mkvk, @ten, @ghichu, 1)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkvk", t.MKVK);
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@ghichu", t.GHICHU);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert KhuVucKho: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật thông tin khu vực kho
        public int update(KhuVucKhoDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE KHUVUCKHO SET TEN=@ten, GHICHU=@ghichu WHERE MKVK=@mkvk";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", t.TEN);
                        cmd.Parameters.AddWithValue("@ghichu", t.GHICHU);
                        cmd.Parameters.AddWithValue("@mkvk", t.MKVK);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update KhuVucKho: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa khu vực kho (Xóa mềm TT=0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE KHUVUCKHO SET TT = 0 WHERE MKVK = @mkvk";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkvk", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete KhuVucKho: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy danh sách khu vực đang hoạt động
        public List<KhuVucKhoDTO> selectAll()
        {
            List<KhuVucKhoDTO> result = new List<KhuVucKhoDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM KHUVUCKHO WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mkvk = rs.GetInt32("MKVK");
                                string ten = rs.GetString("TEN");
                                string ghichu = rs.IsDBNull(rs.GetOrdinal("GHICHU")) ? "" : rs.GetString("GHICHU");
                                int tt = rs.GetInt32("TT");

                                KhuVucKhoDTO kvk = new KhuVucKhoDTO(mkvk, ten, ghichu, tt);
                                result.Add(kvk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll KhuVucKho: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy khu vực theo ID
        public KhuVucKhoDTO selectById(string t)
        {
            KhuVucKhoDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM KHUVUCKHO WHERE MKVK = @mkvk";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkvk", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mkvk = rs.GetInt32("MKVK");
                                string ten = rs.GetString("TEN");
                                string ghichu = rs.IsDBNull(rs.GetOrdinal("GHICHU")) ? "" : rs.GetString("GHICHU");
                                int tt = rs.GetInt32("TT");

                                result = new KhuVucKhoDTO(mkvk, ten, ghichu, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById KhuVucKho: " + ex.Message);
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
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'KHUVUCKHO'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement KhuVucKho: " + ex.Message);
            }
            return result;
        }
    }
}