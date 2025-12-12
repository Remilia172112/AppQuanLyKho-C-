using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class PhieuKiemKeDAO : DAOinterface<PhieuKiemKeDTO>
    {
        private static PhieuKiemKeDAO instance;
        public static PhieuKiemKeDAO Instance
        {
            get
            {
                if (instance == null) instance = new PhieuKiemKeDAO();
                return instance;
            }
        }

        private PhieuKiemKeDAO() { }

        // 1. Thêm phiếu kiểm kê (INSERT) - Mặc định TT=2 (chờ duyệt)
        // Returns: new MPKK (auto-increment ID) if success, 0 if failed
        public int insert(PhieuKiemKeDTO t)
        {
            int newId = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // MPKK là Auto Increment, TT mặc định = 2 (chờ duyệt)
                    string sql = "INSERT INTO PHIEUKIEMKE (MNV, TG, TT) VALUES (@mnv, @tg, 2); SELECT LAST_INSERT_ID();";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mnv", t.MNV);
                        cmd.Parameters.AddWithValue("@tg", t.TG); // DateTime
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            newId = Convert.ToInt32(result);
                            t.MPKK = newId;  // Update DTO with new ID
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Insert PhieuKiemKe: " + ex.Message);
            }
            return newId;
        }

        // 2. Cập nhật (UPDATE) - Chưa hỗ trợ
        public int update(PhieuKiemKeDTO t)
        {
            throw new NotImplementedException("Chức năng cập nhật phiếu kiểm kê chưa được hỗ trợ.");
        }

        // 3. Xóa phiếu kiểm kê (DELETE MỀM - UPDATE TT=0) - CHỈ XÓA KHI TT=2 (chờ duyệt)
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Xóa mềm và chỉ cho phép xóa phiếu chưa duyệt (TT=2)
                    string sql = "UPDATE PHIEUKIEMKE SET TT = 0 WHERE MPKK = @mpkk AND TT = 2";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 4. Lấy tất cả phiếu kiểm kê
        public List<PhieuKiemKeDTO> selectAll()
        {
            List<PhieuKiemKeDTO> result = new List<PhieuKiemKeDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUKIEMKE ORDER BY MPKK DESC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpkk = rs.GetInt32("MPKK");
                                int mnv = rs.GetInt32("MNV");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");
                                
                                // DTO có constructor (int mpkk, int mnv, DateTime tg, int tt)
                                PhieuKiemKeDTO pkk = new PhieuKiemKeDTO(mpkk, mnv, tg, tt);
                                result.Add(pkk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 5. Lấy phiếu theo ID - Chưa hỗ trợ trong Java cũ, nhưng mình viết luôn cho đủ bộ
        public PhieuKiemKeDTO selectById(string t)
        {
            PhieuKiemKeDTO result = null;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM PHIEUKIEMKE WHERE MPKK = @mpkk";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpkk", t);
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                int mpkk = rs.GetInt32("MPKK");
                                int mnv = rs.GetInt32("MNV");
                                DateTime tg = rs.GetDateTime("TG");
                                int tt = rs.GetInt32("TT");
                                
                                result = new PhieuKiemKeDTO(mpkk, mnv, tg, tt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectById PhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 6. DUYỆT PHIẾU KIỂM KÊ - Chuyển TT từ 2 -> 1 và ĐIỀU CHỈNH TỒN KHO theo TRANGTHAISP
        // TRANGTHAISP: Trạng thái sản phẩm sau kiểm kê (số lượng thực tế)
        public int DuyetPhieuKiemKe(int maphieu)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    // Kiểm tra phiếu có tồn tại và đang ở trạng thái chờ duyệt (TT=2) không
                    string sqlCheck = "SELECT TT FROM PHIEUKIEMKE WHERE MPKK = @mpkk";
                    int currentStatus = -1;
                    
                    using (MySqlCommand cmdCheck = new MySqlCommand(sqlCheck, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@mpkk", maphieu);
                        object statusObj = cmdCheck.ExecuteScalar();
                        if (statusObj != null)
                            currentStatus = Convert.ToInt32(statusObj);
                    }

                    if (currentStatus != 2)
                    {
                        Console.WriteLine($"Phiếu kiểm kê {maphieu} không ở trạng thái chờ duyệt (TT={currentStatus})");
                        return 0;
                    }

                    // 1. Cập nhật trạng thái phiếu từ 2 -> 1
                    string sqlUpdate = "UPDATE PHIEUKIEMKE SET TT = 1 WHERE MPKK = @mpkk AND TT = 2";
                    using (MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@mpkk", maphieu);
                        result = cmdUpdate.ExecuteNonQuery();
                    }

                    if (result > 0)
                    {
                        // 2. Điều chỉnh tồn kho cho từng sản phẩm theo TRANGTHAISP
                        var chiTiet = ChiTietPhieuKiemKeDAO.Instance.selectAll(maphieu.ToString());
                        foreach (var item in chiTiet)
                        {
                            
                            // Lấy số lượng tồn hiện tại trong hệ thống
                            var sanpham = SanPhamDAO.Instance.selectById(item.MSP.ToString());
                            if (sanpham != null)
                            {
                                int tonHienTai = sanpham.SL;  
                                int tonThucTe = item.TRANGTHAISP;
                                int chenhLech = tonThucTe - tonHienTai; 
                                
                                if (chenhLech != 0)
                                {
                                    // Cập nhật tồn kho theo chênh lệch
                                    SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, chenhLech);
                                }
                            }
                        }
                        Console.WriteLine($"Đã duyệt phiếu kiểm kê {maphieu} và điều chỉnh tồn kho thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DuyetPhieuKiemKe: " + ex.Message);
            }
            return result;
        }

        // 7. Lấy giá trị Auto Increment
        public int getAutoIncrement()
        {
            int result = -1;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'quanlykho' AND TABLE_NAME = 'PHIEUKIEMKE'";
                    
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
                Console.WriteLine("Lỗi getAutoIncrement PhieuKiemKe: " + ex.Message);
            }
            return result;
        }
    }
}