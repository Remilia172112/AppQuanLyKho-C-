using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class PhieuKiemKeBUS
    {
        private readonly PhieuKiemKeDAO phieuKiemKeDAO = PhieuKiemKeDAO.Instance;
        private readonly ChiTietPhieuKiemKeDAO chiTietKiemKeDAO = ChiTietPhieuKiemKeDAO.Instance;

        public List<PhieuKiemKeDTO> SelectAll()
        {
            var result = phieuKiemKeDAO.selectAll();
            return result ?? new List<PhieuKiemKeDTO>();
        }

        // Alias for SelectAll - for consistency with other BUS classes
        public List<PhieuKiemKeDTO> GetAll()
        {
            return SelectAll();
        }

        // Get phieu by ID
        public PhieuKiemKeDTO GetById(int mpkk)
        {
            var list = phieuKiemKeDAO.selectAll();
            if (list == null) return null;
            
            foreach (var phieu in list)
            {
                if (phieu.MPKK == mpkk)
                {
                    return phieu;
                }
            }
            return null;
        }



        // Thêm phiếu kiểm kê và chi tiết
        public bool Add(PhieuKiemKeDTO phieu, List<ChiTietPhieuKiemKeDTO> dsPhieu)
        {
            bool check = phieuKiemKeDAO.insert(phieu) != 0;
            if (check)
            {
                // Cập nhật Mã phiếu kiểm kê cho danh sách chi tiết (để khớp khóa ngoại)
                // Lưu ý: Nếu ID tự tăng, bạn cần lấy ID vừa insert. 
                // Nhưng trong code Java cũ bạn truyền ID từ ngoài vào (getAutoIncrement trước đó).
                foreach (var item in dsPhieu)
                {
                    item.MPKK = phieu.MPKK;
                }

                check = chiTietKiemKeDAO.insert(dsPhieu) != 0;
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
            // Kiểm tra chỉ sửa khi TT=2
            if (!CanUpdate(phieu.MPKK))
            {
                return false;
            }

            // Xóa chi tiết cũ
            chiTietKiemKeDAO.delete(phieu.MPKK.ToString());
            
            // Cập nhật MPKK cho các chi tiết mới
            foreach (var item in dsPhieu)
            {
                item.MPKK = phieu.MPKK;
            }
            
            // Thêm chi tiết mới
            bool check = chiTietKiemKeDAO.insert(dsPhieu) != 0;
            
            return check;
        }

        // Hủy phiếu kiểm kê (xóa mềm - UPDATE TT=0)
        public bool Cancel(int mpkk)
        {
            // Xóa chi tiết trước
            chiTietKiemKeDAO.delete(mpkk.ToString());
            
            // Xóa phiếu (soft delete: UPDATE TT=0)
            int result = phieuKiemKeDAO.delete(mpkk.ToString());
            
            return result > 0;
        }

        // DUYỆT PHIẾU KIỂM KÊ - Chuyển TT từ 2 (chờ duyệt) -> 1 (đã duyệt) và điều chỉnh tồn kho
        public bool DuyetPhieuKiemKe(int mpkk)
        {
            int result = phieuKiemKeDAO.DuyetPhieuKiemKe(mpkk);
            return result > 0;
        }

        // Kiểm tra có thể sửa phiếu không (chỉ sửa được khi TT=2 - chờ duyệt)
        public bool CanUpdate(int mpkk)
        {
            var phieu = phieuKiemKeDAO.selectById(mpkk.ToString());
            return phieu != null && phieu.TT == 2;
        }

        // Kiểm tra có thể xóa phiếu không (chỉ xóa được khi TT=2 - chờ duyệt)
        public bool CanDelete(int mpkk)
        {
            var phieu = phieuKiemKeDAO.selectById(mpkk.ToString());
            return phieu != null && phieu.TT == 2;
        }
    }
}