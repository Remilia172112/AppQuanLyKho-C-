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
        private readonly NhanVienBUS nvBUS = new NhanVienBUS();
        private List<PhieuKiemKeDTO> danhSachPhieu;

        public PhieuKiemKeBUS()
        {
            danhSachPhieu = phieuKiemKeDAO.selectAll();
        }

        // Lấy danh sách phiếu (Getter/Setter)
        public List<PhieuKiemKeDTO> DanhSachPhieu
        {
            get { return danhSachPhieu; }
            set { danhSachPhieu = value; }
        }

        public List<PhieuKiemKeDTO> SelectAll()
        {
            return phieuKiemKeDAO.selectAll();
        }

        public List<ChiTietPhieuKiemKeDTO> GetChiTietPhieu(int maphieukiemke)
        {
            return chiTietKiemKeDAO.selectAll(maphieukiemke.ToString());
        }

        public int GetAutoIncrement()
        {
            return phieuKiemKeDAO.getAutoIncrement();
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
                
                if (check)
                {
                    danhSachPhieu.Add(phieu);
                }
            }
            return check;
        }

        // Hủy phiếu kiểm kê
        public void Cancel(int index)
        {
            if (index >= 0 && index < danhSachPhieu.Count)
            {
                PhieuKiemKeDTO phieu = danhSachPhieu[index];
                
                // Xóa chi tiết trước
                chiTietKiemKeDAO.delete(phieu.MPKK.ToString());
                
                // Xóa phiếu
                phieuKiemKeDAO.delete(phieu.MPKK.ToString());
                
                // Xóa khỏi list
                danhSachPhieu.RemoveAt(index);
            }
        }

        public List<ChiTietPhieuKiemKeDTO> GetChitietTiemKe(int maphieu)
        {
            return chiTietKiemKeDAO.selectAll(maphieu.ToString());
        }

        // Lọc phiếu kiểm kê
        public List<PhieuKiemKeDTO> FilterPhieuKiemKe(int type, string input, int manv, DateTime timeStart, DateTime timeEnd)
        {
            List<PhieuKiemKeDTO> result = new List<PhieuKiemKeDTO>();
            
            // Chuyển input về chữ thường để so sánh
            input = input.ToLower();

            foreach (PhieuKiemKeDTO phieu in danhSachPhieu)
            {
                bool match = false;
                switch (type)
                {
                    case 0: // Tất cả
                        if (phieu.MPKK.ToString().Contains(input) || 
                            nvBUS.GetNameById(phieu.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 1: // Mã phiếu
                        if (phieu.MPKK.ToString().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 2: // Người tạo
                        if (nvBUS.GetNameById(phieu.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                }

                // Kiểm tra các điều kiện kết hợp: Match + Nhân viên + Thời gian
                if (match &&
                    (manv == 0 || phieu.MNV == manv) &&
                    (phieu.TG >= timeStart) && 
                    (phieu.TG <= timeEnd))
                {
                    result.Add(phieu);
                }
            }
            return result;
        }

        public ChiTietPhieuKiemKeDTO FindCT(List<ChiTietPhieuKiemKeDTO> ctphieu, int masp)
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

        // DUYỆT PHIẾU KIỂM KÊ - Chuyển TT từ 2 (chờ duyệt) -> 1 (đã duyệt) và điều chỉnh tồn kho
        public bool DuyetPhieuKiemKe(int mpkk)
        {
            int result = phieuKiemKeDAO.DuyetPhieuKiemKe(mpkk);
            if (result > 0)
            {
                // Refresh cache sau khi duyệt thành công
                this.danhSachPhieu = phieuKiemKeDAO.selectAll();
                return true;
            }
            return false;
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

        // Lọc phiếu kiểm kê theo trạng thái (TT)
        // status: 0 = đã xóa, 1 = đã duyệt, 2 = chờ duyệt
        public List<PhieuKiemKeDTO> FillerPhieuKiemKeByStatus(int status)
        {
            List<PhieuKiemKeDTO> result = new List<PhieuKiemKeDTO>();
            foreach (var phieu in this.danhSachPhieu)
            {
                if (phieu.TT == status)
                {
                    result.Add(phieu);
                }
            }
            return result;
        }
    }
}