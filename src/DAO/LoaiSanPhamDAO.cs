using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.config;
using src.DTO;

namespace src.DAO
{
    public class LoaiSanPhamDAO
    {
        private static LoaiSanPhamDAO instance;

        public static LoaiSanPhamDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoaiSanPhamDAO();
                return instance;
            }
        }

        private LoaiSanPhamDAO() { }

        public List<LoaiSanPhamDTO> selectAll()
        {
            List<LoaiSanPhamDTO> result = new List<LoaiSanPhamDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM LOAISANPHAM WHERE TT = 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                LoaiSanPhamDTO lsp = new LoaiSanPhamDTO();
                                lsp.MLSP = rs.GetInt32("MLSP");
                                lsp.TEN = rs.GetString("TEN");
                                lsp.TLGX = rs.GetInt32("TLGX"); // <--- MỚI: Đọc TLGX
                                lsp.GHICHU = rs.IsDBNull(rs.GetOrdinal("GHICHU")) ? "" : rs.GetString("GHICHU");
                                lsp.TT = rs.GetInt32("TT");
                                result.Add(lsp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll LoaiSanPham: " + ex.Message);
            }
            return result;
        }

        public int insert(LoaiSanPhamDTO lsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Thêm TLGX vào câu lệnh INSERT
                    string sql = "INSERT INTO LOAISANPHAM (TEN, TLGX, GHICHU, TT) VALUES (@TEN, @TLGX, @GHICHU, 1)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TEN", lsp.TEN);
                        cmd.Parameters.AddWithValue("@TLGX", lsp.TLGX); // <--- MỚI
                        cmd.Parameters.AddWithValue("@GHICHU", lsp.GHICHU ?? "");
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert LoaiSanPham: " + ex.Message);
            }
            return result;
        }

        public int update(LoaiSanPhamDTO lsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Thêm TLGX vào câu lệnh UPDATE
                    string sql = "UPDATE LOAISANPHAM SET TEN = @TEN, TLGX = @TLGX, GHICHU = @GHICHU WHERE MLSP = @MLSP";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TEN", lsp.TEN);
                        cmd.Parameters.AddWithValue("@TLGX", lsp.TLGX); // <--- MỚI
                        cmd.Parameters.AddWithValue("@GHICHU", lsp.GHICHU ?? "");
                        cmd.Parameters.AddWithValue("@MLSP", lsp.MLSP);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update LoaiSanPham: " + ex.Message);
            }
            return result;
        }

        public int delete(int mlsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE LOAISANPHAM SET TT = 0 WHERE MLSP = @MLSP";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MLSP", mlsp);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete LoaiSanPham: " + ex.Message);
            }
            return result;
        }

        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'LOAISANPHAM'";
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
                Console.WriteLine("Lỗi getAutoIncrement LoaiSanPham: " + ex.Message);
            }
            return result;
        }
    }
}