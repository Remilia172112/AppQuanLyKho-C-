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

        public int Insert(LoaiSanPhamDTO lsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "INSERT INTO LOAISANPHAM (TEN, GHICHU, TT) VALUES (@TEN, @GHICHU, 1)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TEN", lsp.TEN);
                        cmd.Parameters.AddWithValue("@GHICHU", lsp.GHICHU);
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

        public int Update(LoaiSanPhamDTO lsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE LOAISANPHAM SET TEN=@TEN, GHICHU=@GHICHU WHERE MLSP=@MLSP";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TEN", lsp.TEN);
                        cmd.Parameters.AddWithValue("@GHICHU", lsp.GHICHU);
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

        public int Delete(int mlsp)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Xóa mềm (Soft delete)
                    string sql = "UPDATE LOAISANPHAM SET TT=0 WHERE MLSP=@MLSP";
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
    }
}