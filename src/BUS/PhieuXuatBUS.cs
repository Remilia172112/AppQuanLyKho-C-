using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class PhieuXuatBUS
    {
        private readonly PhieuXuatDAO phieuXuatDAO = PhieuXuatDAO.Instance;
        private readonly ChiTietPhieuXuatDAO chiTietPhieuXuatDAO = ChiTietPhieuXuatDAO.Instance;
        private List<PhieuXuatDTO> listPhieuXuat = new List<PhieuXuatDTO>();

        private readonly NhanVienBUS nvBUS = new NhanVienBUS();
        private readonly KhachHangBUS khBUS = new KhachHangBUS();

        public PhieuXuatBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listPhieuXuat = phieuXuatDAO.selectAll() ?? new List<PhieuXuatDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo PhieuXuatBUS: {ex.Message}");
                listPhieuXuat = new List<PhieuXuatDTO>();
            }
        }

        public List<PhieuXuatDTO> GetAll()
        {
            LoadData();
            return listPhieuXuat;
        }

        public List<PhieuXuatDTO> GetAll(int mkh)
        {
            listPhieuXuat = phieuXuatDAO.selectByMKH(mkh.ToString());
            return listPhieuXuat;
        }

        public PhieuXuatDTO GetSelect(int index) => listPhieuXuat[index];

        // LINQ: Lấy phiếu xuất theo mã
        public PhieuXuatDTO? GetById(int mapx)
            => listPhieuXuat.FirstOrDefault(p => p.MPX == mapx);

        public void Cancel(int px) => phieuXuatDAO.cancelPhieuXuat(px);

        public void Remove(int px) => listPhieuXuat.RemoveAt(px);

        public void Update(PhieuXuatDTO px)
        {
            phieuXuatDAO.update(px);
            chiTietPhieuXuatDAO.updateSL(px.MPX.ToString());
        }

        // Insert Phiếu Xuất (Trừ kho)
        public int Insert(PhieuXuatDTO px, List<ChiTietPhieuXuatDTO> ct)
        {
            // Insert phiếu xuất
            int newMPX = phieuXuatDAO.insert(px);

            // Cập nhật MPX cho chi tiết phiếu
            px.MPX = newMPX;
            ct.ForEach(item => item.MPX = newMPX);

            // Insert chi tiết (đã bao gồm logic trừ kho trong DAO)
            chiTietPhieuXuatDAO.insert(ct);

            // Cập nhật cache
            listPhieuXuat.Add(px);

            return newMPX;
        }

        // Update Phiếu Xuất (chỉ cho phép sửa phiếu chờ duyệt TT=2)
        public bool Update(PhieuXuatDTO px, List<ChiTietPhieuXuatDTO> ct)
        {
            // Chỉ cho phép sửa phiếu chờ duyệt
            if (px.TT != 2) return false;

            int result = phieuXuatDAO.update(px);
            if (result == 0) return false;

            // Xóa chi tiết cũ và thêm mới
            chiTietPhieuXuatDAO.delete(px.MPX.ToString());

            // Cập nhật MPX cho chi tiết
            ct.ForEach(item => item.MPX = px.MPX);

            chiTietPhieuXuatDAO.insert(ct);

            // LINQ: Cập nhật cache
            int index = listPhieuXuat.FindIndex(p => p.MPX == px.MPX);
            if (index >= 0)
            {
                listPhieuXuat[index] = px;
            }

            return true;
        }

        // Insert Giỏ hàng tạm (Không trừ kho?) - Theo logic code cũ
        public void InsertGH(PhieuXuatDTO px, List<ChiTietPhieuXuatDTO> ct)
        {
            phieuXuatDAO.insert(px);
            chiTietPhieuXuatDAO.insertGH(ct);
        }

        public List<ChiTietPhieuXuatDTO> SelectCTP(int maphieu)
            => chiTietPhieuXuatDAO.selectAll(maphieu.ToString());

        // LINQ: Lấy mã phiếu xuất lớn nhất
        public int GetMPMAX()
        {
            var list = phieuXuatDAO.selectAll();
            return list.Any() ? list.Max(p => p.MPX) : 1;
        }

        // LINQ: Tìm chi tiết phiếu theo mã sản phẩm
        public ChiTietPhieuXuatDTO? FindCT(List<ChiTietPhieuXuatDTO> ctphieu, int masp)
            => ctphieu.FirstOrDefault(item => item.MSP == masp);

        // LINQ: Lọc Phiếu Xuất (Filter)
        public List<PhieuXuatDTO> FillerPhieuXuat(int type, string input, int makh, int manv, DateTime timeStart, DateTime timeEnd, string priceMinStr, string priceMaxStr)
        {
            long priceMin = !string.IsNullOrEmpty(priceMinStr) ? long.Parse(priceMinStr) : 0;
            long priceMax = !string.IsNullOrEmpty(priceMaxStr) ? long.Parse(priceMaxStr) : long.MaxValue;

            // Set timeEnd về cuối ngày
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            input = input.ToLower();

            // LINQ: Filter theo type
            IEnumerable<PhieuXuatDTO> query = GetAll();

            switch (type)
            {
                case 0: // Tất cả
                    query = query.Where(px =>
                        px.MPX.ToString().Contains(input) ||
                        khBUS.GetTenKhachHang(px.MKH).ToLower().Contains(input) ||
                        nvBUS.GetNameById(px.MNV).ToLower().Contains(input));
                    break;
                case 1: // Mã phiếu
                    query = query.Where(px => px.MPX.ToString().Contains(input));
                    break;
                case 2: // Khách hàng
                    query = query.Where(px => khBUS.GetTenKhachHang(px.MKH).ToLower().Contains(input));
                    break;
                case 3: // Nhân viên
                    query = query.Where(px => nvBUS.GetNameById(px.MNV).ToLower().Contains(input));
                    break;
            }

            // LINQ: Filter theo các điều kiện khác
            return query
                .Where(px => manv == 0 || px.MNV == manv)
                .Where(px => makh == 0 || px.MKH == makh)
                .Where(px => px.TG >= timeStart && px.TG <= timeEndFixed)
                .Where(px => px.TIEN >= priceMin && px.TIEN <= priceMax)
                .ToList();
        }

        // LINQ: Lấy mảng mã phiếu xuất
        public string[] GetArrMPX()
            => listPhieuXuat.Select(px => px.MPX.ToString()).ToArray();

        public PhieuXuatDTO GetByIndex(int index) => listPhieuXuat[index];

        // LINQ: Kiểm tra số lượng tồn kho trước khi xuất
        public bool CheckSLPx(List<ChiTietPhieuXuatDTO> listctpx)
        {
            SanPhamBUS spBus = new SanPhamBUS();

            // LINQ: Kiểm tra tất cả sản phẩm có đủ số lượng không
            return listctpx.All(ct =>
            {
                var sp = spBus.GetByMaSP(ct.MSP);
                return sp != null && ct.SL <= sp.SL;
            });
        }

        // Kiểm tra số lượng tồn kho (Dựa trên Mã phiếu đã lưu) - giữ nguyên vì gọi DAO
        public bool CheckSLPx(int maphieu) => phieuXuatDAO.checkSLPx(maphieu);

        public int CancelPhieuXuat(int maphieu) => phieuXuatDAO.cancelPhieuXuat(maphieu);

        // DUYỆT PHIẾU XUẤT - Chuyển TT từ 2 (chờ duyệt) -> 1 (đã duyệt) và trừ tồn kho
        public bool DuyetPhieuXuat(int mpx)
        {
            int result = phieuXuatDAO.DuyetPhieuXuat(mpx);
            if (result > 0)
            {
                // Refresh cache sau khi duyệt thành công
                LoadData();
                return true;
            }
            return false;
        }

        // Kiểm tra có thể sửa phiếu không (chỉ sửa được khi TT=2 - chờ duyệt) - giữ nguyên vì gọi DAO
        public bool CanUpdate(int mpx)
        {
            var phieu = phieuXuatDAO.selectById(mpx.ToString());
            return phieu != null && phieu.TT == 2;
        }

        // Kiểm tra có thể xóa phiếu không (chỉ xóa được khi TT=2 - chờ duyệt) - giữ nguyên vì gọi DAO
        public bool CanDelete(int mpx)
        {
            var phieu = phieuXuatDAO.selectById(mpx.ToString());
            return phieu != null && phieu.TT == 2;
        }

        // LINQ: Lọc phiếu xuất theo trạng thái (TT)
        public List<PhieuXuatDTO> FillerPhieuXuatByStatus(int status)
            => listPhieuXuat.Where(phieu => phieu.TT == status).ToList();
        
        public List<ChiTietPhieuXuatDTO> GetChiTietPhieuChoDuyet()
        {
            List<ChiTietPhieuXuatDTO> allDetails = new List<ChiTietPhieuXuatDTO>();

            // 1. Lấy danh sách các phiếu chưa duyệt
            var listPhieuCho = listPhieuXuat.Where(px => px.TT == 2).ToList(); 

            // 2. Lấy chi tiết của từng phiếu
            foreach (var phieu in listPhieuCho)
            {
                var details = chiTietPhieuXuatDAO.selectAll(phieu.MPX.ToString());
                if (details != null)
                {
                    allDetails.AddRange(details);
                }
            }

            return allDetails;
        }
        public int getAutoIncrement()
        {
            return phieuXuatDAO.getAutoIncrement();
        }
    }
}
