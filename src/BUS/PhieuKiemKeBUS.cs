using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class PhieuKiemKeBUS
    {
        private readonly PhieuKiemKeDAO phieuKiemKeDAO = PhieuKiemKeDAO.Instance;
        private readonly ChiTietPhieuKiemKeDAO chiTietKiemKeDAO = ChiTietPhieuKiemKeDAO.Instance;
        private List<PhieuKiemKeDTO> listPhieuKiemKe = new List<PhieuKiemKeDTO>();

        public PhieuKiemKeBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listPhieuKiemKe = phieuKiemKeDAO.selectAll() ?? new List<PhieuKiemKeDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData PhieuKiemKeBUS: {ex.Message}");
                listPhieuKiemKe = new List<PhieuKiemKeDTO>();
            }
        }

        public List<PhieuKiemKeDTO> SelectAll() => listPhieuKiemKe;

        // Alias for SelectAll - for consistency with other BUS classes
        public List<PhieuKiemKeDTO> GetAll() => listPhieuKiemKe;

        // LINQ: Get phieu by ID từ cache
        public PhieuKiemKeDTO? GetById(int mpkk)
            => listPhieuKiemKe.FirstOrDefault(phieu => phieu.MPKK == mpkk);

        // Thêm phiếu kiểm kê và chi tiết
        public bool Add(PhieuKiemKeDTO phieu, List<ChiTietPhieuKiemKeDTO> dsPhieu)
        {
            bool check = phieuKiemKeDAO.insert(phieu) != 0;
            if (check)
            {
                // LINQ: Cập nhật Mã phiếu kiểm kê cho danh sách chi tiết
                dsPhieu.ForEach(item => item.MPKK = phieu.MPKK);

                check = chiTietKiemKeDAO.insert(dsPhieu) != 0;

                if (check) LoadData(); // Reload để đồng bộ
            }
            return check;
        }

        // Lấy chi tiết phiếu kiểm kê
        public List<ChiTietPhieuKiemKeDTO> GetChiTietPhieu(int mpkk)
        {
            var result = chiTietKiemKeDAO.selectAll(mpkk.ToString());
            return result ?? new List<ChiTietPhieuKiemKeDTO>();
        }

        // Cập nhật phiếu kiểm kê và chi tiết (chỉ cho phép sửa phiếu chờ duyệt TT=2)
        public bool Update(PhieuKiemKeDTO phieu, List<ChiTietPhieuKiemKeDTO> dsPhieu)
        {
            if (!CanUpdate(phieu.MPKK))
            {
                return false;
            }

            // Xóa chi tiết cũ
            chiTietKiemKeDAO.delete(phieu.MPKK.ToString());

            // LINQ: Cập nhật MPKK cho các chi tiết mới
            dsPhieu.ForEach(item => item.MPKK = phieu.MPKK);

            // Thêm chi tiết mới
            return chiTietKiemKeDAO.insert(dsPhieu) != 0;
        }

        // Hủy phiếu kiểm kê (xóa mềm - UPDATE TT=0)
        public bool Cancel(int mpkk)
        {
            // Xóa chi tiết trước
            chiTietKiemKeDAO.delete(mpkk.ToString());

            // Xóa phiếu (soft delete: UPDATE TT=0)
            bool result = phieuKiemKeDAO.delete(mpkk.ToString()) > 0;

            if (result) LoadData(); // Reload để đồng bộ

            return result;
        }

        // DUYỆT PHIẾU KIỂM KÊ
        public bool DuyetPhieuKiemKe(int mpkk)
        {
            bool result = phieuKiemKeDAO.DuyetPhieuKiemKe(mpkk) > 0;

            if (result) LoadData(); // Reload để đồng bộ

            return result;
        }

        // LINQ: Kiểm tra có thể sửa phiếu không (chỉ sửa được khi TT=2 - chờ duyệt)
        public bool CanUpdate(int mpkk)
        {
            var phieu = listPhieuKiemKe.FirstOrDefault(p => p.MPKK == mpkk);
            return phieu != null && phieu.TT == 2;
        }

        // LINQ: Kiểm tra có thể xóa phiếu không (chỉ xóa được khi TT=2 - chờ duyệt)
        public bool CanDelete(int mpkk)
        {
            var phieu = listPhieuKiemKe.FirstOrDefault(p => p.MPKK == mpkk);
            return phieu != null && phieu.TT == 2;
        }
    }
}
