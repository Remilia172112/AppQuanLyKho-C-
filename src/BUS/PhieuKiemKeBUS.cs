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
    }
}