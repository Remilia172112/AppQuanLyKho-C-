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

        private List<PhieuNhapDTO> listPhieuNhap = new List<PhieuNhapDTO>();

        public PhieuNhapBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listPhieuNhap = phieunhapDAO.selectAll() ?? new List<PhieuNhapDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData PhieuNhapBUS: {ex.Message}");
                listPhieuNhap = new List<PhieuNhapDTO>();
            }
        }

        public List<PhieuNhapDTO> GetAll()
        {
            LoadData();
            return listPhieuNhap;
        }

        public List<PhieuNhapDTO> GetAllList() => listPhieuNhap;

        // LINQ: Get single phieu by ID từ cache
        public PhieuNhapDTO? GetById(int maphieu)
            => listPhieuNhap.FirstOrDefault(p => p.MPN == maphieu);

        // LINQ: Chuyển đổi Dictionary thành List
        public List<SanPhamDTO> ConvertHashMapToArray(Dictionary<int, List<SanPhamDTO>> chitietsanpham)
            => chitietsanpham.Values.SelectMany(ctsp => ctsp).ToList();

        public List<ChiTietPhieuNhapDTO> GetChiTietPhieu(int maphieunhap)
            => ctPhieuNhapDAO.selectAll(maphieunhap.ToString());

        public List<ChiTietPhieuNhapDTO> GetChiTietPhieu_Type(int maphieunhap)
            => ctPhieuNhapDAO.selectAll(maphieunhap.ToString());

        // LINQ: Lấy mã phiếu nhập lớn nhất
        public int GetMPMAX()
        {
            var list = phieunhapDAO.selectAll();
            return list.Any() ? list.Max(p => p.MPN) : 1;
        }

        // Thêm phiếu nhập và chi tiết
        public int Add(PhieuNhapDTO phieu, List<ChiTietPhieuNhapDTO> ctPhieu)
        {
            int newMPN = phieunhapDAO.insert(phieu);
            if (newMPN > 0)
            {
                phieu.MPN = newMPN;
                // LINQ: Cập nhật mã phiếu cho chi tiết
                ctPhieu.ForEach(item => item.MPN = newMPN);

                bool check = ctPhieuNhapDAO.insert(ctPhieu) != 0;

                if (check)
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
            if (!CanUpdate(phieu.MPN))
            {
                Console.WriteLine("Không thể sửa phiếu nhập đã duyệt (TT=1) hoặc đã xóa (TT=0)");
                return 0;
            }

            int result = phieunhapDAO.update(phieu);
            if (result > 0)
            {
                ctPhieuNhapDAO.delete(phieu.MPN.ToString());

                // LINQ: Cập nhật mã phiếu cho chi tiết
                ctPhieu.ForEach(item => item.MPN = phieu.MPN);
                ctPhieuNhapDAO.insert(ctPhieu);

                // LINQ: Cập nhật cache
                int index = listPhieuNhap.FindIndex(x => x.MPN == phieu.MPN);
                if (index >= 0)
                {
                    listPhieuNhap[index] = phieu;
                }
            }
            return result;
        }

        // LINQ: Tìm chi tiết phiếu theo mã sản phẩm
        public ChiTietPhieuNhapDTO? FindCT(List<ChiTietPhieuNhapDTO> ctphieu, int masp)
            => ctphieu.FirstOrDefault(item => item.MSP == masp);

        // LINQ: Tính tổng tiền
        public long GetTIEN(List<ChiTietPhieuNhapDTO> ctphieu)
            => ctphieu.Sum(item => (long)item.TIENNHAP * item.SL);

        // LINQ: Hàm lọc phiếu nhập
        public List<PhieuNhapDTO> FillerPhieuNhap(int type, string input, int mancc, int manv, DateTime timeStart, DateTime timeEnd, string priceMinStr, string priceMaxStr)
        {
            long priceMin = !string.IsNullOrEmpty(priceMinStr) ? long.Parse(priceMinStr) : 0;
            long priceMax = !string.IsNullOrEmpty(priceMaxStr) ? long.Parse(priceMaxStr) : long.MaxValue;
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);
            input = input.ToLower();

            return GetAllList()
                .Where(phieu =>
                {
                    bool match = type switch
                    {
                        1 => phieu.MPN.ToString().Contains(input),
                        2 => nccBUS.GetTenNhaCungCap(phieu.MNCC).ToLower().Contains(input),
                        3 => nvBUS.GetNameById(phieu.MNV).ToLower().Contains(input),
                        _ => phieu.MPN.ToString().Contains(input) ||
                             nccBUS.GetTenNhaCungCap(phieu.MNCC).ToLower().Contains(input) ||
                             nvBUS.GetNameById(phieu.MNV).ToLower().Contains(input)
                    };

                    return match &&
                           (manv == 0 || phieu.MNV == manv) &&
                           (mancc == 0 || phieu.MNCC == mancc) &&
                           phieu.TG >= timeStart &&
                           phieu.TG <= timeEndFixed &&
                           phieu.TIEN >= priceMin &&
                           phieu.TIEN <= priceMax;
                })
                .ToList();
        }

        public bool CheckSLPn(int maphieu) => phieunhapDAO.checkSLPn(maphieu);

        // Duyệt phiếu nhập
        public int DuyetPhieuNhap(int maphieu)
        {
            int result = phieunhapDAO.DuyetPhieuNhap(maphieu);
            if (result > 0)
            {
                // LINQ: Cập nhật lại cache
                var updatedPhieu = phieunhapDAO.selectById(maphieu.ToString());
                if (updatedPhieu != null)
                {
                    int index = listPhieuNhap.FindIndex(x => x.MPN == maphieu);
                    if (index != -1)
                    {
                        listPhieuNhap[index] = updatedPhieu;
                    }
                }
            }
            return result;
        }

        // LINQ: Kiểm tra có thể cập nhật phiếu không
        public bool CanUpdate(int maphieu)
        {
            var phieu = listPhieuNhap.FirstOrDefault(p => p.MPN == maphieu);
            return phieu != null && phieu.TT == 2;
        }

        // LINQ: Kiểm tra có thể xóa phiếu không
        public bool CanDelete(int maphieu)
        {
            var phieu = listPhieuNhap.FirstOrDefault(p => p.MPN == maphieu);
            return phieu != null && phieu.TT == 2;
        }

        // LINQ: Lọc phiếu nhập theo trạng thái
        public List<PhieuNhapDTO> FillerPhieuNhapByStatus(int status)
            => GetAllList().Where(phieu => phieu.TT == status).ToList();

        public int CancelPhieuNhap(int maphieu)
        {
            if (!CanDelete(maphieu))
            {
                Console.WriteLine("Không thể xóa phiếu nhập đã duyệt (TT=1) hoặc đã xóa (TT=0)");
                return 0;
            }

            ctPhieuNhapDAO.delete(maphieu.ToString());
            int result = phieunhapDAO.delete(maphieu.ToString());

            if (result > 0)
            {
                // LINQ: Xóa khỏi list cache
                listPhieuNhap.RemoveAll(x => x.MPN == maphieu);
            }
            return result;
        }
    }
}
