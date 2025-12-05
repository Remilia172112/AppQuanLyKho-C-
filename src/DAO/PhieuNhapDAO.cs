using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class PhieuNhapDAO : DAOinterface<PhieuNhapDTO>
    {
        private static PhieuNhapDAO instance;
        public static PhieuNhapDAO Instance
        {
            get
            {
                if (instance == null) instance = new PhieuNhapDAO();
                return instance;
            }
        }

        private PhieuNhapDAO() { }

        // 1. Thêm phiếu nhập (INSERT)
        public int insert(PhieuNhapDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MPN tự tăng, nên không insert
                    string sql = "INSERT INTO PHIEUNHAP (MNV, MNCC, TIEN, TG, TT) VALUES (@mnv, @mncc, @tien, @tg, @tt)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@mncc", t.MNCC);
                        cmd.Parameters.AddWithValue("@tien", t.TIEN);
                        cmd.Parameters.AddWithValue("@tg", t.TG); // DateTime
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 2. Cập nhật phiếu nhập (UPDATE)
        public int update(PhieuNhapDTO t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE PHIEUNHAP SET TG=@tg, MNCC=@mncc, TIEN=@tien, TT=@tt WHERE MPN=@mpn";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tg", t.TG);
                        cmd.Parameters.AddWithValue("@mncc", t.MNCC);
                        cmd.Parameters.AddWithValue("@tien", t.TIEN);
                        cmd.Parameters.AddWithValue("@tt", t.TT);
                        cmd.Parameters.AddWithValue("@mpn", t.MPN);
                        
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Update PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 3. Xóa phiếu nhập (DELETE MỀM - UPDATE TT=0)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "UPDATE PHIEUNHAP SET TT = 0 WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả phiếu nhập
        public List<PhieuNhapDTO> selectAll()
        {
            List<PhieuNhapDTO> result = new List<PhieuNhapDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUNHAP ORDER BY MPN DESC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpn = rs.GetInt32("MPN");
                                int mnv = rs.GetInt32("MNV");
                                int mncc = rs.GetInt32("MNCC");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                // DTO Constructor: (mpn, mncc, mnv, tien, tg, tt)
                                PhieuNhapDTO pn = new PhieuNhapDTO(mpn, mncc, mnv, tien, tg, tt);
                                result.Add(pn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy phiếu theo ID
        public PhieuNhapDTO selectById(string t)
        {
            PhieuNhapDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUNHAP WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mpn = rs.GetInt32("MPN");
                                int mnv = rs.GetInt32("MNV");
                                int mncc = rs.GetInt32("MNCC");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                result = new PhieuNhapDTO(mpn, mncc, mnv, tien, tg, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 6. Thống kê theo khoảng tiền (Statistical)
        public List<PhieuNhapDTO> statistical(long min, long max)
        {
            List<PhieuNhapDTO> result = new List<PhieuNhapDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUNHAP WHERE TIEN BETWEEN @min AND @max";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@min", min);
                        cmd.Parameters.AddWithValue("@max", max);
                        
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpn = rs.GetInt32("MPN");
                                int mnv = rs.GetInt32("MNV");
                                int mncc = rs.GetInt32("MNCC");
                                int tien = rs.GetInt32("TIEN");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");

                                PhieuNhapDTO pn = new PhieuNhapDTO(mpn, mncc, mnv, tien, tg, tt);
                                result.Add(pn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Statistical PhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 7. Kiểm tra số lượng phiếu nhập so với tồn kho (Logic này hơi lạ: Nhập thì SL luôn tăng chứ?)
        // Có lẽ ý bạn là: Kiểm tra xem có đủ hàng để hủy phiếu nhập không?
        public bool checkSLPn(int maphieu)
        {
            // Trong C#, mình sẽ dùng List thay vì ArrayList
            List<SanPhamDTO> listSP = new List<SanPhamDTO>();
            List<ChiTietPhieuNhapDTO> listCT = new List<ChiTietPhieuNhapDTO>();

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM CTPHIEUNHAP WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", maphieu);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int msp = rs.GetInt32("MSP");
                                int sl = rs.GetInt32("SL");
                                int tiennhap = rs.GetInt32("TIENNHAP");
                                int hinhthuc = rs.GetInt32("HINHTHUC");

                                ChiTietPhieuNhapDTO ct = new ChiTietPhieuNhapDTO(maphieu, msp, sl, tiennhap, hinhthuc);
                                listCT.Add(ct);
                                
                                // Lấy thông tin sản phẩm hiện tại để so sánh tồn kho
                                // Lưu ý: Mình gọi trực tiếp SanPhamDAO để tránh vòng lặp BUS <-> DAO
                                SanPhamDTO sp = SanPhamDAO.Instance.selectById(msp.ToString()); 
                                // Cần đảm bảo SanPhamDAO có hàm selectById
                                listSP.Add(sp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi checkSLPn: " + ex.Message);
                return false;
            }

            // So sánh
            for (int i = 0; i < listSP.Count; i++)
            {
                // Nếu số lượng trong phiếu nhập > số lượng tồn hiện tại 
                // -> Nghĩa là hàng đã bị xuất đi rồi, không thể hủy phiếu nhập này được
                if (listCT[i].SL > listSP[i].SL)
                {
                    return false;
                }
            }
            return true;
        }

        // 8. Hủy phiếu nhập (Cancel)
        public int cancelPhieuNhap(int maphieu)
        {
            int result = 0;
            // Lấy chi tiết phiếu
            List<ChiTietPhieuNhapDTO> listCT = ChiTietPhieuNhapDAO.Instance.selectAll(maphieu.ToString());
            
            // Trừ số lượng tồn kho
            foreach (var item in listCT)
            {
                int soLuongTru = -(item.SL); // Số âm
                SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, soLuongTru);
            }

            // Xóa chi tiết phiếu
            ChiTietPhieuNhapDAO.Instance.delete(maphieu.ToString());

            // Xóa phiếu nhập (DELETE CỨNG)
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM PHIEUNHAP WHERE MPN = @mpn";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpn", maphieu);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi CancelPhieuNhap: " + ex.Message);
            }
            return result;
        }

        // 9. Lấy Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'PHIEUNHAP'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement PhieuNhap: " + ex.Message);
            }
            return result;
        }
    }
}