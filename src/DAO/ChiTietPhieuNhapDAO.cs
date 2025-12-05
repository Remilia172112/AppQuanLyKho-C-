using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class ChiTietPhieuNhapDAO : ChiTietInterface<ChiTietPhieuNhapDTO>
    {
        // Singleton Pattern
        private static ChiTietPhieuNhapDAO instance;
        public static ChiTietPhieuNhapDAO Instance
        {
            get
            {
                if (instance == null) instance = new ChiTietPhieuNhapDAO();
                return instance;
            }
        }

        private ChiTietPhieuNhapDAO() { }

        // 1. Thêm danh sách chi tiết phiếu nhập (Insert)
        // Logic: Insert vào bảng CTPHIEUNHAP -> Gọi SanPhamDAO để tăng tồn kho
        public int insert(List<ChiTietPhieuNhapDTO> t)
        {
            int result = 0;
            // Dùng 1 kết nối chung cho cả vòng lặp để tối ưu hiệu năng
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                foreach (var item in t)
                {
                    try
                    {
                        // 1.1 Insert vào chi tiết phiếu nhập
                        string sql = "INSERT INTO CTPHIEUNHAP (MPN, MSP, SL, TIENNHAP, HINHTHUC) VALUES (@mpn, @msp, @sl, @tiennhap, @hinhthuc)";
                        
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mpn", item.MPN);
                            cmd.Parameters.AddWithValue("@msp", item.MSP);
                            cmd.Parameters.AddWithValue("@sl", item.SL);
                            cmd.Parameters.AddWithValue("@tiennhap", item.TIENNHAP);
                            cmd.Parameters.AddWithValue("@hinhthuc", item.HINHTHUC);

                            result += cmd.ExecuteNonQuery();
                        }

                        // 1.2 Cập nhật số lượng tồn kho (Gọi sang SanPhamDAO)
                        // Giả định bạn sẽ viết hàm UpdateSoLuongTon bên SanPhamDAO
                        // Logic nhập hàng: Tồn kho tăng lên (+)
                        // SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, item.SL); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi Insert CTPhieuNhap (MSP: " + item.MSP + "): " + ex.Message);
                    }
                }
            }
            return result;
        }

        // 2. Xóa chi tiết theo Mã phiếu nhập (Delete)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM CTPHIEUNHAP WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete CTPhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 3. Cập nhật (Update) -> Thực hiện chiến thuật: Xóa hết cũ, thêm mới
        public int update(List<ChiTietPhieuNhapDTO> t, string pk)
        {
            // Xóa hết chi tiết cũ của phiếu nhập này
            int result = this.delete(pk);
            
            // Nếu xóa thành công (hoặc không có gì để xóa nhưng không lỗi), thì insert lại list mới
            if (result >= 0) 
            {
                result = this.insert(t);
            }
            return result;
        }

        // 4. Lấy danh sách chi tiết theo Mã phiếu nhập (SelectAll)
        public List<ChiTietPhieuNhapDTO> selectAll(string t)
        {
            List<ChiTietPhieuNhapDTO> result = new List<ChiTietPhieuNhapDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM CTPHIEUNHAP WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", t);
                        
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpn = rs.GetInt32("MPN");
                                int msp = rs.GetInt32("MSP");
                                int sl = rs.GetInt32("SL");
                                int tiennhap = rs.GetInt32("TIENNHAP");
                                int hinhthuc = rs.GetInt32("HINHTHUC");

                                ChiTietPhieuNhapDTO ct = new ChiTietPhieuNhapDTO(mpn, msp, sl, tiennhap, hinhthuc);
                                result.Add(ct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll CTPhieuNhap: " + ex.Message);
            }
            return result;
        }
    }
}