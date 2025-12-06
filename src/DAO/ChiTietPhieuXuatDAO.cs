using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using src.DTO;
using src.config;

namespace src.DAO
{
    public class ChiTietPhieuXuatDAO : ChiTietInterface<ChiTietPhieuXuatDTO>
    {
        // Singleton Pattern
        private static ChiTietPhieuXuatDAO instance;
        public static ChiTietPhieuXuatDAO Instance
        {
            get
            {
                if (instance == null) instance = new ChiTietPhieuXuatDAO();
                return instance;
            }
        }

        private ChiTietPhieuXuatDAO() { }

        // 1. Insert chi tiết phiếu xuất - KHÔNG trừ tồn kho ngay (chờ duyệt phiếu)
        public int insert(List<ChiTietPhieuXuatDTO> t)
        {
            int result = 0;
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                foreach (var item in t)
                {
                    try
                    {
                        // 1.1 Insert vào bảng CTPHIEUXUAT
                        string sql = "INSERT INTO CTPHIEUXUAT (MPX, MSP, SL, TIENXUAT) VALUES (@mpx, @msp, @sl, @tienxuat)";
                        
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mpx", item.MPX);
                            cmd.Parameters.AddWithValue("@msp", item.MSP);
                            cmd.Parameters.AddWithValue("@sl", item.SL);
                            cmd.Parameters.AddWithValue("@tienxuat", item.TIENXUAT);

                            result += cmd.ExecuteNonQuery();
                        }

                        // KHÔNG cập nhật tồn kho tại đây - chỉ cập nhật khi phiếu xuất được DUYỆT
                        // Logic trừ kho được chuyển sang PhieuXuatDAO.DuyetPhieuXuat()
                        // int soLuongGiam = -(item.SL);
                        // SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, soLuongGiam);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi Insert CTPhieuXuat: " + ex.Message);
                    }
                }
            }
            return result;
        }

        // 2. Insert Giỏ hàng (Theo code cũ: Chỉ thêm vào database, KHÔNG trừ tồn kho ngay)
        // Thường dùng để lưu tạm
        public int insertGH(List<ChiTietPhieuXuatDTO> t)
        {
            int result = 0;
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                foreach (var item in t)
                {
                    try
                    {
                        string sql = "INSERT INTO CTPHIEUXUAT (MPX, MSP, SL, TIENXUAT) VALUES (@mpx, @msp, @sl, @tienxuat)";
                        
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mpx", item.MPX);
                            cmd.Parameters.AddWithValue("@msp", item.MSP);
                            cmd.Parameters.AddWithValue("@sl", item.SL);
                            cmd.Parameters.AddWithValue("@tienxuat", item.TIENXUAT);

                            result += cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi InsertGH CTPhieuXuat: " + ex.Message);
                    }
                }
            }
            return result;
        }

        // 3. Reset: Trả lại số lượng tồn kho và Xóa chi tiết
        // Dùng khi hủy phiếu xuất hoặc sửa phiếu (trả hàng về kho)
        public int reset(List<ChiTietPhieuXuatDTO> t)
        {
            int result = 0;
            foreach (var item in t)
            {
                // 3.1 Cộng lại số lượng tồn kho (Số dương)
                SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, item.SL);
                
                // 3.2 Xóa chi tiết phiếu đó đi
                this.delete(item.MPX.ToString());
            }
            return result;
        }

        // 4. Xóa toàn bộ chi tiết theo Mã phiếu xuất
        public int delete(string t)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "DELETE FROM CTPHIEUXUAT WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", t);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Delete CTPhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 5. Cập nhật: Xóa cũ -> Thêm mới
        public int update(List<ChiTietPhieuXuatDTO> t, string pk)
        {
            // Xóa hết chi tiết cũ
            int result = this.delete(pk);
            
            // Nếu xóa OK thì thêm mới lại
            if (result >= 0) 
            {
                result = this.insert(t);
            }
            return result;
        }

        // 6. Lấy danh sách chi tiết theo Mã phiếu xuất
        public List<ChiTietPhieuXuatDTO> selectAll(string t)
        {
            List<ChiTietPhieuXuatDTO> result = new List<ChiTietPhieuXuatDTO>();
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = "SELECT * FROM CTPHIEUXUAT WHERE MPX = @mpx";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mpx", t);
                        
                        using (MySqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                int mpx = rs.GetInt32("MPX");
                                int msp = rs.GetInt32("MSP");
                                int sl = rs.GetInt32("SL");
                                int tienxuat = rs.GetInt32("TIENXUAT");

                                // Khởi tạo DTO với dữ liệu từ SQL
                                ChiTietPhieuXuatDTO ct = new ChiTietPhieuXuatDTO(mpx, msp, 0, sl, tienxuat); 
                                // Lưu ý: Constructor DTO của bạn ở phần trước có tham số MKM (mã khuyến mãi).
                                // Nếu bạn đã bỏ MKM, hãy sửa lại Constructor DTO chỉ nhận 4 tham số.
                                // Nếu chưa sửa DTO, hãy truyền 0 hoặc null vào chỗ MKM tạm thời.
                                
                                result.Add(ct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SelectAll CTPhieuXuat: " + ex.Message);
            }
            return result;
        }

        // 7. Cập nhật lại tồn kho cho 1 phiếu (Dùng để đồng bộ nếu cần)
        public void updateSL(string t)
        {
            try
            {
                List<ChiTietPhieuXuatDTO> list = this.selectAll(t);
                foreach (var item in list)
                {
                    // Trừ tồn kho
                    int soLuongGiam = -(item.SL);
                    SanPhamDAO.Instance.UpdateSoLuongTon(item.MSP, soLuongGiam);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi updateSL: " + ex.Message);
            }
        }
    }
}