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
    }
}
