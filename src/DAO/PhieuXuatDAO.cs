using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class PhieuXuatDAO : DAOinterface<PhieuXuatDTO>
    {
        // Singleton Pattern
        private static PhieuXuatDAO instance;
        public static PhieuXuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new PhieuXuatDAO();
                return instance;
            }
        }

        private PhieuXuatDAO() { }

        // 1. Thêm phiếu xuất (INSERT) - Mặc định TT=2 (chờ duyệt)
        // Trả về MPX mới được insert (LAST_INSERT_ID)
        public int insert(PhieuXuatDTO t)
        {
            int newMPX = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MPX tự tăng, TT mặc định = 2 (chờ duyệt), KHÔNG trừ kho ngay
                    string sql = "INSERT INTO PHIEUXUAT (MNV, MKH, TIEN, TG, TT) VALUES (@mnv, @mkh, @tien, @tg, 2)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@mkh", t.MKH);
                        cmd.Parameters.AddWithValue("@tien", t.TIEN);
                        cmd.Parameters.AddWithValue("@tg", t.TG); // DateTime
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Lấy ID vừa insert (AUTO_INCREMENT)
                            newMPX = (int)cmd.LastInsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert PhieuXuat: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
            return newMPX;
        }

        // 2. Cập nhật phiếu xuất (UPDATE)
        public int update(PhieuXuatDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE PHIEUXUAT SET MNV=@mnv, MKH=@mkh, TIEN=@tien, TG=@tg, TT=@tt WHERE MPX=@mpx";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@mkh", t.MKH);
                        cmd.Parameters.AddWithValue("@tien", t.TIEN);
                        cmd.Parameters.AddWithValue("@tg", t.TG);
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        cmd.Parameters.AddWithValue("@mpx", t.MPX);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa phiếu xuất (DELETE MỀM - UPDATE TT=0) - CHỈ XÓA KHI TT=2 (chờ duyệt)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Chỉ cho phép xóa phiếu chưa duyệt (TT=2)
                    string sql = "UPDATE PHIEUXUAT SET TT = 0 WHERE MPX = @mpx AND TT = 2";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả phiếu xuất
        public List<PhieuXuatDTO> selectAll()
        {
            List<PhieuXuatDTO> result = new List<PhieuXuatDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUXUAT ORDER BY MPX DESC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpx = rs.GetInt32("MPX");
                                int mkh = rs.GetInt32("MKH");
                                int mnv = rs.GetInt32("MNV");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                // DTO Constructor: (mpx, mnv, mkh, tien, tg, tt)
                                PhieuXuatDTO px = new PhieuXuatDTO(mpx, mnv, mkh, tien, tg, tt);
                                result.Add(px);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy phiếu theo ID
        public PhieuXuatDTO selectById(string t)
        {
            PhieuXuatDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUXUAT WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mpx = rs.GetInt32("MPX");
                                int mkh = rs.GetInt32("MKH");
                                int mnv = rs.GetInt32("MNV");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                result = new PhieuXuatDTO(mpx, mnv, mkh, tien, tg, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 6. Lấy phiếu theo Mã Khách Hàng
        public List<PhieuXuatDTO> selectByMKH(string t)
        {
            List<PhieuXuatDTO> result = new List<PhieuXuatDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUXUAT WHERE MKH = @mkh ORDER BY MPX DESC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mkh", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpx = rs.GetInt32("MPX");
                                int mkh = rs.GetInt32("MKH");
                                int mnv = rs.GetInt32("MNV");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                PhieuXuatDTO px = new PhieuXuatDTO(mpx, mnv, mkh, tien, tg, tt);
                                result.Add(px);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectByMKH PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 7. Hủy phiếu xuất (Cancel) - Logic quan trọng
        public int cancelPhieuXuat(int maphieu)
        {
            int result = 0;
            // 7.1 Lấy chi tiết phiếu xuất
            List<ChiTietPhieuXuatDTO> listCT = ChiTietPhieuXuatDAO.Instance.selectAll(maphieu.ToString());
            
            // 7.2 Hoàn trả số lượng tồn kho (Cộng lại)
            foreach (var item in listCT)
            {
                // Khi xuất hàng thì trừ, giờ hủy phiếu xuất -> Hàng quay về kho -> Cộng lại (+)
                SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, item.SL); 
            }

            // 7.3 Xóa chi tiết phiếu
            ChiTietPhieuXuatDAO.Instance.delete(maphieu.ToString());

            // 7.4 Xóa phiếu xuất (DELETE CỨNG)
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM PHIEUXUAT WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", maphieu);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi CancelPhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 8. Kiểm tra tồn kho trước khi xuất (CheckSLPx)
        public bool checkSLPx(int maphieu)
        {
            List<ChiTietPhieuXuatDTO> listCT = new List<ChiTietPhieuXuatDTO>();
            List<SanPhamDTO> listSP = new List<SanPhamDTO>();

            try
            {
                // Lấy chi tiết phiếu (thường là từ giỏ hàng tạm)
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM CTPHIEUXUAT WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", maphieu);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int msp = rs.GetInt32("MSP");
                                int sl = rs.GetInt32("SL");
                                int tien = rs.GetInt32("TIENXUAT");

                                ChiTietPhieuXuatDTO ct = new ChiTietPhieuXuatDTO(maphieu, msp, 0, sl, tien);
                                listCT.Add(ct);

                                // Lấy thông tin sản phẩm hiện tại
                                SanPhamDTO sp = SanPhamDAO.Instance.selectById(msp.ToString());
                                listSP.Add(sp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi checkSLPx: " + ex.Message);
                return false;
            }

            // So sánh: Nếu SL cần xuất > SL tồn kho -> False
            for (int i = 0; i < listSP.Count; i++)
            {
                if (listCT[i].SL > listSP[i].SL)
                {
                    return false;
                }
            }
            return true;
        }

        // 9. Lấy Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Đã sửa tên DB thành 'quanlykho'
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'PHIEUXUAT'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement PhieuXuat: " + ex.Message);
            }
            return result;
        }
        
        // 10. UpdateDP (Duyệt phiếu? - Update TT=1)
        public int updateDP(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE PHIEUXUAT SET TT = 1 WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpdateDP PhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 11. Duyệt phiếu xuất - Chuyển TT=2 sang TT=1 và trừ tồn kho
        public int DuyetPhieuXuat(int mpx)
        {
            int result = 0;
            try
            {
                // 11.1 Kiểm tra phiếu xuất có TT=2 không
                PhieuXuatDTO phieu = selectById(mpx.ToString());
                if (phieu == null || phieu.TT != 2)
                {
                    return 0; // Phiếu không tồn tại hoặc đã được duyệt/xóa
                }

                // 11.2 Trừ tồn kho cho các sản phẩm trong phiếu
                List<ChiTietPhieuXuatDTO> listCT = ChiTietPhieuXuatDAO.Instance.selectAll(mpx.ToString());
                foreach (var item in listCT)
                {
                    // Trừ số lượng tồn kho (số âm vì xuất hàng)
                    SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, -(item.SL));
                }

                // 11.3 Cập nhật trạng thái phiếu xuất sang TT=1 (đã duyệt)
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE PHIEUXUAT SET TT = 1 WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", mpx);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DuyetPhieuXuat: " + ex.Message);
            }
            return result;
        }
    }
}