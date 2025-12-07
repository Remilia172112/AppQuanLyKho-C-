using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class ChiTietPhieuKiemKeDAO : ChiTietInterface<ChiTietPhieuKiemKeDTO>
    {
        // Singleton Pattern
        private static ChiTietPhieuKiemKeDAO instance;
        public static ChiTietPhieuKiemKeDAO Instance
        {
            get
            {
                if (instance == null) instance = new ChiTietPhieuKiemKeDAO();
                return instance;
            }
        }

        private ChiTietPhieuKiemKeDAO() { }

        public int insert(List<ChiTietPhieuKiemKeDTO> t)
        {
            int result = 0;
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                // Duyệt qua từng phần tử trong danh sách để insert
                foreach (var item in t)
                {
                    try
                    {
                        string query = "INSERT INTO CTPHIEUKIEMKE (MPKK, MSP, TRANGTHAISP, GHICHU) VALUES (@mpkk, @msp, @tt, @ghichu)";
                        
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@mpkk", item.MPKK);
                            cmd.Parameters.AddWithValue("@msp", item.MSP);
                            cmd.Parameters.AddWithValue("@tt", item.TRANGTHAISP);
                            
                            // Xử lý ghi chú null
                            if (item.GHICHU == null) // Cần check vì DateTime? hoặc String
                                cmd.Parameters.AddWithValue("@ghichu", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@ghichu", item.GHICHU);

                            // Cộng dồn số dòng insert thành công
                            result += cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi Insert CTPhieuKiemKe (MSP: " + item.MSP + "): " + ex.Message);
                    }
                }
            }
            return result;
        }


        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = "DELETE FROM CTPHIEUKIEMKE WHERE MPKK = @mpkk";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete CTPhieuKiemKe: " + ex.Message);
            }
            return result;
        }


        public int update(List<ChiTietPhieuKiemKeDTO> t, string pk)
        {
            throw new NotImplementedException("Chức năng update chi tiết kiểm kê chưa được hỗ trợ.");
        }

        public List<ChiTietPhieuKiemKeDTO> selectAll(string t)
        {
            List<ChiTietPhieuKiemKeDTO> result = new List<ChiTietPhieuKiemKeDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT * FROM CTPHIEUKIEMKE WHERE MPKK = @mpkk";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int mpkk = reader.GetInt32("MPKK");
                                int msp = reader.GetInt32("MSP");
                                int trangthaisp = reader.GetInt32("TRANGTHAISP");
                                string ghichu = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("GHICHU")))
                                {
                                     ghichu = reader.GetString("GHICHU"); 
                                }

                                ChiTietPhieuKiemKeDTO ct = new ChiTietPhieuKiemKeDTO();
                                ct.MPKK = mpkk;
                                ct.MSP = msp;
                                ct.TRANGTHAISP = trangthaisp;
                                ct.GHICHU = ghichu;

                                result.Add(ct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll CTPhieuKiemKe: " + ex.Message);
            }
            return result;
        }
    }
}