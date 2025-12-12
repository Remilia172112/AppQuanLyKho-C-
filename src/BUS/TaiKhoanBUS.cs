using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class TaiKhoanBUS
    {
        private List<TaiKhoanDTO> listTaiKhoan = new List<TaiKhoanDTO>();

        // Gọi DAO theo Singleton
        private readonly NhomQuyenDAO nhomQuyenDAO = NhomQuyenDAO.Instance;
        private readonly TaiKhoanDAO taiKhoanDAO = TaiKhoanDAO.Instance;

        public TaiKhoanBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listTaiKhoan = taiKhoanDAO.selectAll() ?? new List<TaiKhoanDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo TaiKhoanBUS: {ex.Message}");
                listTaiKhoan = new List<TaiKhoanDTO>();
            }
        }

        // Lấy danh sách tài khoản (có cập nhật từ DB)
        public List<TaiKhoanDTO> GetTaiKhoanAll()
        {
            LoadData();
            return listTaiKhoan;
        }

        public TaiKhoanDTO GetTaiKhoan(int index) => listTaiKhoan[index];

        // LINQ: Lấy vị trí (index) của tài khoản trong list dựa vào Mã Nhân Viên
        public int GetTaiKhoanByMaNV(int manv)
            => listTaiKhoan.FindIndex(tk => tk.MNV == manv);

        // Lấy thông tin nhóm quyền (để hiển thị tên quyền) - giữ nguyên vì gọi DAO
        public NhomQuyenDTO GetNhomQuyenDTO(int manhom)
            => nhomQuyenDAO.selectById(manhom.ToString());

        public void AddAcc(TaiKhoanDTO tk)
        {
            // Mã hóa mật khẩu bằng BCrypt trước khi lưu vào DB
            tk.MK = BCrypt.Net.BCrypt.HashPassword(tk.MK);

            taiKhoanDAO.insert(tk);
            // Sau khi thêm vào DB, nên thêm vào list cache để đồng bộ
            listTaiKhoan.Add(tk);
        }

        public void UpdateAcc(TaiKhoanDTO tk)
        {
            // Nếu mật khẩu không bắt đầu bằng $2 (BCrypt hash), nghĩa là mật khẩu mới cần mã hóa
            // Hash BCrypt luôn bắt đầu với $2a$, $2b$, $2x$, $2y$
            if (!tk.MK.StartsWith("$2"))
            {
                tk.MK = BCrypt.Net.BCrypt.HashPassword(tk.MK);
            }

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
            // LINQ: Xóa khỏi list cache
            listTaiKhoan.RemoveAll(tk => tk.MNV == manv);
        }

        // Kiểm tra Tên đăng nhập đã tồn tại chưa - giữ nguyên vì gọi DAO
        public bool CheckTDN(string TDN)
        {
            TaiKhoanDTO tk = taiKhoanDAO.selectByUser(TDN);
            return tk == null; // null = Chưa tồn tại -> OK
        }

        // LINQ: Tìm kiếm tài khoản
        public List<TaiKhoanDTO> Search(string txt, string type)
        {
            txt = txt.ToLower();
            IEnumerable<TaiKhoanDTO> query = listTaiKhoan;

            switch (type)
            {
                case "Mã nhân viên":
                    query = query.Where(tk => tk.MNV.ToString().Contains(txt));
                    break;
                case "Username": // Tên đăng nhập
                    query = query.Where(tk => tk.TDN.ToLower().Contains(txt));
                    break;
                default: // Tất cả
                    query = query.Where(tk =>
                        tk.MNV.ToString().Contains(txt) ||
                        tk.TDN.ToLower().Contains(txt));
                    break;
            }
            return query.ToList();
        }

        // Đăng nhập - xác thực tài khoản - giữ nguyên vì gọi DAO
        public TaiKhoanDTO? DangNhap(string tenDangNhap, string matKhau)
        {
            try
            {
                TaiKhoanDTO? tk = taiKhoanDAO.selectByUser(tenDangNhap);
                if (tk == null) return null;
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(matKhau, tk.MK);
                if (!isPasswordCorrect) return null;
                return tk;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
