using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class DanhMucChucNangDAO
    {
        private static DanhMucChucNangDAO instance;
        public static DanhMucChucNangDAO Instance
        {
            get
            {
                if (instance == null) instance = new DanhMucChucNangDAO();
                return instance;
            }
        }

        private DanhMucChucNangDAO() { }

        // Lấy tất cả danh mục chức năng
        public List<DanhMucChucNangDTO> SelectAll()
        {
            List<DanhMucChucNangDTO> result = new List<DanhMucChucNangDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM DANHMUCCHUCNANG";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                string machucnang = rs.GetString("MCN");
                                string tenchucnang = rs.GetString("TEN");
                                
                                DanhMucChucNangDTO dvt = new DanhMucChucNangDTO(machucnang, tenchucnang);
                                
                                result.Add(dvt);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi SelectAll DanhMucChucNang: " + e.Message);
            }
            return result;
        }
    }
}