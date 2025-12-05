using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class TaiKhoanBUS
    {
        private List<TaiKhoanDTO> listTaiKhoan;
        
        // Gọi DAO theo Singleton
        private readonly NhomQuyenDAO nhomQuyenDAO = NhomQuyenDAO.Instance;
        private readonly TaiKhoanDAO taiKhoanDAO = TaiKhoanDAO.Instance;

        public TaiKhoanBUS()
        {
            this.listTaiKhoan = taiKhoanDAO.selectAll();
        }

        // Lấy danh sách tài khoản (có cập nhật từ DB)
        public List<TaiKhoanDTO> GetTaiKhoanAll()
        {
            this.listTaiKhoan = taiKhoanDAO.selectAll();
            return listTaiKhoan;
        }

        public TaiKhoanDTO GetTaiKhoan(int index)
        {
            return listTaiKhoan[index];
        }

        // Lấy vị trí (index) của tài khoản trong list dựa vào Mã Nhân Viên
        public int GetTaiKhoanByMaNV(int manv)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listTaiKhoan.Count && vitri == -1)
            {
                if (listTaiKhoan[i].MNV == manv)
                {
                    vitri = i;
                }
                else
                {
                    i++;
                }
            }
            return vitri;
        }

        // Lấy thông tin nhóm quyền (để hiển thị tên quyền)
        public NhomQuyenDTO GetNhomQuyenDTO(int manhom)
        {
            return nhomQuyenDAO.selectById(manhom.ToString());
        }

        public void AddAcc(TaiKhoanDTO tk)
        {
            taiKhoanDAO.insert(tk);
            // Sau khi thêm vào DB, nên thêm vào list cache để đồng bộ
            listTaiKhoan.Add(tk);
        }

        public void UpdateAcc(TaiKhoanDTO tk)
        {
            taiKhoanDAO.update(tk);
            // Cập nhật lại list cache
            int index = GetTaiKhoanByMaNV(tk.MNV);
            if (index != -1)
            {
                listTaiKhoan[index] = tk;
            }
        }

        public void DeleteAcc(int manv)
        {
            taiKhoanDAO.delete(manv.ToString());
            // Xóa khỏi list cache
            int index = GetTaiKhoanByMaNV(manv);
            if (index != -1)
            {
                listTaiKhoan.RemoveAt(index);
            }
        }

        // Kiểm tra Tên đăng nhập đã tồn tại chưa
        public bool CheckTDN(string TDN)
        {
            TaiKhoanDTO tk = taiKhoanDAO.selectByUser(TDN);
            if (tk != null) return false; // Đã tồn tại
            return true; // Chưa tồn tại -> OK
        }

        // Tìm kiếm tài khoản
        public List<TaiKhoanDTO> Search(string txt, string type)
        {
            List<TaiKhoanDTO> result = new List<TaiKhoanDTO>();
            txt = txt.ToLower();

            switch (type)
            {
                case "Tất cả":
                    foreach (TaiKhoanDTO i in listTaiKhoan)
                    {
                        if (i.MNV.ToString().Contains(txt) || i.TDN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã nhân viên":
                    foreach (TaiKhoanDTO i in listTaiKhoan)
                    {
                        if (i.MNV.ToString().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Username": // Tên đăng nhập
                    foreach (TaiKhoanDTO i in listTaiKhoan)
                    {
                        if (i.TDN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }
            return result;
        }
    }
}