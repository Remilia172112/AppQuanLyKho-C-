using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class PhieuNhapBUS
    {
        private readonly PhieuNhapDAO phieunhapDAO = PhieuNhapDAO.Instance;
        private readonly ChiTietPhieuNhapDAO ctPhieuNhapDAO = ChiTietPhieuNhapDAO.Instance;

        private readonly NhaCungCapBUS nccBUS = new NhaCungCapBUS();
        private readonly NhanVienBUS nvBUS = new NhanVienBUS();

        public List<PhieuNhapDTO> listPhieuNhap;

        public PhieuNhapBUS()
        {
            // Lấy danh sách phiếu nhập khi khởi tạo
            listPhieuNhap = phieunhapDAO.selectAll();
        }

        public List<PhieuNhapDTO> GetAll()
        {
            listPhieuNhap = phieunhapDAO.selectAll();
            return listPhieuNhap;
        }

        public List<PhieuNhapDTO> GetAllList()
        {
            return listPhieuNhap;
        }

        // Get single phieu by ID
        public PhieuNhapDTO GetById(int maphieu)
        {
            return phieunhapDAO.selectById(maphieu.ToString());
        }

        // Chuyển đổi Dictionary (Giỏ hàng) thành List sản phẩm
        public List<SanPhamDTO> ConvertHashMapToArray(Dictionary<int, List<SanPhamDTO>> chitietsanpham)
        {
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            foreach (var ctsp in chitietsanpham.Values)
            {
                result.AddRange(ctsp);
            }
            return result;
        }

        public List<ChiTietPhieuNhapDTO> GetChiTietPhieu(int maphieunhap)
        {
            return ctPhieuNhapDAO.selectAll(maphieunhap.ToString());
        }

        // Lấy chi tiết phiếu nhưng map sang DTO dùng chung (nếu cần)
        public List<ChiTietPhieuNhapDTO> GetChiTietPhieu_Type(int maphieunhap)
        {
            return ctPhieuNhapDAO.selectAll(maphieunhap.ToString());
        }

        // Lấy mã phiếu nhập lớn nhất (để tự tăng nếu không dùng AUTO_INCREMENT DB)
        public int GetMPMAX()
        {
            List<PhieuNhapDTO> list = phieunhapDAO.selectAll();
            int s = 1;
            foreach (PhieuNhapDTO i in list)
            {
                if (i.MPN > s) s = i.MPN;
            }
            return s;
        }

        // Thêm phiếu nhập và chi tiết
        // Lưu ý: Trong Java bạn truyền HashMap, nhưng ở đây mình khuyên dùng List<ChiTietPhieuNhapDTO> cho chuẩn
        public int Add(PhieuNhapDTO phieu, List<ChiTietPhieuNhapDTO> ctPhieu)
        {
            int newMPN = phieunhapDAO.insert(phieu);
            if (newMPN > 0)
            {
                phieu.MPN = newMPN;
                // Cập nhật mã phiếu cho chi tiết
                foreach (var item in ctPhieu)
                {
                    item.MPN = newMPN;
                }

                bool check = ctPhieuNhapDAO.insert(ctPhieu) != 0;
                
                if(check)
                {
                    listPhieuNhap.Add(phieu);
                    return newMPN;
                }
            }
            return 0;
        }

        // Update phiếu nhập và chi tiết
        public int Update(PhieuNhapDTO phieu, List<ChiTietPhieuNhapDTO> ctPhieu)
        {
            // Kiểm tra chỉ sửa khi TT=2
            if (!CanUpdate(phieu.MPN))
            {
                Console.WriteLine("Không thể sửa phiếu nhập đã duyệt (TT=1) hoặc đã xóa (TT=0)");
                return 0;
            }

            int result = phieunhapDAO.update(phieu);
            if (result > 0)
            {
                // Xóa chi tiết cũ
                ctPhieuNhapDAO.delete(phieu.MPN.ToString());
                
                // Thêm chi tiết mới
                foreach (var item in ctPhieu)
                {
                    item.MPN = phieu.MPN;
                }
                ctPhieuNhapDAO.insert(ctPhieu);

                // Cập nhật cache
                int index = listPhieuNhap.FindIndex(x => x.MPN == phieu.MPN);
                if (index >= 0)
                {
                    listPhieuNhap[index] = phieu;
                }
            }
            return result;
        }

        public ChiTietPhieuNhapDTO FindCT(List<ChiTietPhieuNhapDTO> ctphieu, int masp)
        {
            foreach (var item in ctphieu)
            {
                if (item.MSP == masp)
                {
                    return item;
                }
            }
            return null;
        }

        public long GetTIEN(List<ChiTietPhieuNhapDTO> ctphieu)
        {
            long result = 0;
            foreach (var item in ctphieu)
            {
                result += (long)item.TIENNHAP * item.SL;
            }
            return result;
        }

        // Hàm lọc phiếu nhập (Filter)
        public List<PhieuNhapDTO> FillerPhieuNhap(int type, string input, int mancc, int manv, DateTime timeStart, DateTime timeEnd, string priceMinStr, string priceMaxStr)
        {
            long priceMin = !string.IsNullOrEmpty(priceMinStr) ? long.Parse(priceMinStr) : 0;
            long priceMax = !string.IsNullOrEmpty(priceMaxStr) ? long.Parse(priceMaxStr) : long.MaxValue;

            // Set timeEnd về cuối ngày
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            List<PhieuNhapDTO> result = new List<PhieuNhapDTO>();
            input = input.ToLower();

            foreach (PhieuNhapDTO phieuNhap in GetAllList())
            {
                bool match = false;
                switch (type)
                {
                    case 0: // Tất cả
                        if (phieuNhap.MPN.ToString().Contains(input) || 
                            nccBUS.GetTenNhaCungCap(phieuNhap.MNCC).ToLower().Contains(input) || 
                            nvBUS.GetNameById(phieuNhap.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 1: // Mã phiếu
                        if (phieuNhap.MPN.ToString().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 2: // Nhà cung cấp
                        if (nccBUS.GetTenNhaCungCap(phieuNhap.MNCC).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 3: // Nhân viên
                        if (nvBUS.GetNameById(phieuNhap.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                }

                if (match &&
                    (manv == 0 || phieuNhap.MNV == manv) &&
                    (mancc == 0 || phieuNhap.MNCC == mancc) &&
                    (phieuNhap.TG >= timeStart) &&
                    (phieuNhap.TG <= timeEndFixed) &&
                    (phieuNhap.TIEN >= priceMin) &&
                    (phieuNhap.TIEN <= priceMax))
                {
                    result.Add(phieuNhap);
                }
            }
            return result;
        }

        public bool CheckSLPn(int maphieu)
        {
            return phieunhapDAO.checkSLPn(maphieu);
        }

        // Duyệt phiếu nhập (TT: 2->1) - Chỉ quản lý kho mới được duyệt
        public int DuyetPhieuNhap(int maphieu)
        {
            int result = phieunhapDAO.DuyetPhieuNhap(maphieu);
            if(result > 0)
            {
                // Cập nhật lại cache
                PhieuNhapDTO updatedPhieu = phieunhapDAO.selectById(maphieu.ToString());
                if (updatedPhieu != null)
                {
                    // Tìm và cập nhật trong list
                    int index = listPhieuNhap.FindIndex(x => x.MPN == maphieu);
                    if (index != -1)
                    {
                        listPhieuNhap[index] = updatedPhieu;
                    }
                }
            }
            return result;
        }

        // Kiểm tra có thể cập nhật phiếu không (Chỉ cập nhật khi TT=2)
        public bool CanUpdate(int maphieu)
        {
            PhieuNhapDTO phieu = phieunhapDAO.selectById(maphieu.ToString());
            return phieu != null && phieu.TT == 2;
        }

        // Kiểm tra có thể xóa phiếu không (Chỉ xóa khi TT=2)
        public bool CanDelete(int maphieu)
        {
            PhieuNhapDTO phieu = phieunhapDAO.selectById(maphieu.ToString());
            return phieu != null && phieu.TT == 2;
        }

        // Lọc phiếu nhập theo trạng thái (0: Đã xóa, 1: Đã duyệt, 2: Chờ duyệt)
        public List<PhieuNhapDTO> FillerPhieuNhapByStatus(int status)
        {
            List<PhieuNhapDTO> result = new List<PhieuNhapDTO>();
            foreach (PhieuNhapDTO phieu in GetAllList())
            {
                if (phieu.TT == status)
                {
                    result.Add(phieu);
                }
            }
            return result;
        }

        public int CancelPhieuNhap(int maphieu)
        {
            // Kiểm tra chỉ xóa khi TT=2
            if (!CanDelete(maphieu))
            {
                Console.WriteLine("Không thể xóa phiếu nhập đã duyệt (TT=1) hoặc đã xóa (TT=0)");
                return 0;
            }

            // Xóa chi tiết phiếu nhập
            ctPhieuNhapDAO.delete(maphieu.ToString());

            // Xóa phiếu nhập (UPDATE TT=0)
            int result = phieunhapDAO.delete(maphieu.ToString());
            
            if(result > 0)
            {
                // Xóa khỏi list cache
                listPhieuNhap.RemoveAll(x => x.MPN == maphieu);
            }
            return result;
        }
    }
}