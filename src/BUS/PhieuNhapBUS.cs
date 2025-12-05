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
        public bool Add(PhieuNhapDTO phieu, List<ChiTietPhieuNhapDTO> ctPhieu)
        {
            bool check = phieunhapDAO.insert(phieu) != 0;
            if (check)
            {
                // Cập nhật mã phiếu cho chi tiết (nếu ID tự tăng)
                // Tuy nhiên, logic Java cũ của bạn là lấy ID từ GetMPMAX rồi gán vào
                // Ở đây giả sử phieu.MPN đã có giá trị đúng
                foreach (var item in ctPhieu)
                {
                    item.MPN = phieu.MPN;
                }

                check = ctPhieuNhapDAO.insert(ctPhieu) != 0;
                
                if(check)
                {
                    listPhieuNhap.Add(phieu);
                }
            }
            return check;
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

        public int CancelPhieuNhap(int maphieu)
        {
            int result = phieunhapDAO.cancelPhieuNhap(maphieu);
            if(result > 0)
            {
                // Xóa khỏi list cache
                listPhieuNhap.RemoveAll(x => x.MPN == maphieu);
            }
            return result;
        }
    }
}