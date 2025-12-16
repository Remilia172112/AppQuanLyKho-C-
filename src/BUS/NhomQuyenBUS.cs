using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhomQuyenBUS
    {
        private readonly NhomQuyenDAO nhomquyenDAO = NhomQuyenDAO.Instance;
        private readonly ChiTietQuyenDAO chitietquyenDAO = ChiTietQuyenDAO.Instance;
        private readonly DanhMucChucNangDAO danhMucChucNangDAO = DanhMucChucNangDAO.Instance;
        private List<NhomQuyenDTO> listNhomQuyen = new List<NhomQuyenDTO>();

        public NhomQuyenBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listNhomQuyen = nhomquyenDAO.selectAll() ?? new List<NhomQuyenDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData NhomQuyenBUS: {ex.Message}");
                listNhomQuyen = new List<NhomQuyenDTO>();
            }
        }

        public List<NhomQuyenDTO> GetAll() => listNhomQuyen;

        public NhomQuyenDTO GetByIndex(int index) => listNhomQuyen[index];

        // Thêm nhóm quyền mới và các chi tiết quyền kèm theo
        public bool Add(string tenNhomQuyen, List<ChiTietQuyenDTO> ctquyen)
        {
            int newId = nhomquyenDAO.getAutoIncrement();
            NhomQuyenDTO nq = new NhomQuyenDTO(newId, tenNhomQuyen);

            bool check = nhomquyenDAO.insert(nq) != 0;

            if (check)
            {
                listNhomQuyen.Add(nq);

                // LINQ: Cập nhật Mã nhóm quyền cho các chi tiết quyền
                ctquyen.ForEach(item => item.Manhomquyen = newId);

                AddChiTietQuyen(ctquyen);
            }
            return check;
        }

        // Cập nhật nhóm quyền và danh sách chi tiết quyền
        public bool Update(NhomQuyenDTO nhomquyen, List<ChiTietQuyenDTO> chitietquyen, int index)
        {
            bool check = nhomquyenDAO.update(nhomquyen) != 0;

            if (check)
            {
                RemoveChiTietQuyen(nhomquyen.Manhomquyen.ToString());

                // LINQ: Đảm bảo mã nhóm quyền khớp
                chitietquyen.ForEach(item => item.Manhomquyen = nhomquyen.Manhomquyen);
                AddChiTietQuyen(chitietquyen);

                listNhomQuyen[index] = nhomquyen;
            }
            return check;
        }

        public bool Delete(NhomQuyenDTO nqdto)
        {
            bool check = nhomquyenDAO.delete(nqdto.Manhomquyen.ToString()) != 0;

            if (check)
            {
                listNhomQuyen.Remove(nqdto);
            }
            return check;
        }

        // --- Xử lý Chi tiết quyền ---

        public List<ChiTietQuyenDTO> GetChiTietQuyen(string manhomquyen)
            => chitietquyenDAO.selectAll(manhomquyen);

        public List<DanhMucChucNangDTO> GetAllChucNang()
            => danhMucChucNangDAO.SelectAll();

        public NhomQuyenDTO? GetById(int id)
            => nhomquyenDAO.selectById(id.ToString());

        public List<ChiTietQuyenDTO> GetChiTietQuyen(int manhomquyen)
            => chitietquyenDAO.selectAll(manhomquyen.ToString());

        public bool AddChiTietQuyen(List<ChiTietQuyenDTO> listctquyen)
            => chitietquyenDAO.insert(listctquyen) != 0;

        public bool RemoveChiTietQuyen(string manhomquyen)
            => chitietquyenDAO.delete(manhomquyen) != 0;

        // LINQ: Kiểm tra quyền
        public bool CheckPermission(int maquyen, string chucnang, string hanhdong)
        {
            var ctquyen = GetChiTietQuyen(maquyen.ToString());
            return ctquyen.Any(item =>
                item.Machucnang.Equals(chucnang) &&
                item.Hanhdong.Equals(hanhdong));
        }

        // LINQ: Tìm kiếm
        public List<NhomQuyenDTO> Search(string text)
        {
            text = text.ToLower();
            return listNhomQuyen
                .Where(i => i.Manhomquyen.ToString().Contains(text) ||
                           i.Tennhomquyen.ToLower().Contains(text))
                .ToList();
        }
        public int getAutoIncrement()
        {
            return nhomquyenDAO.getAutoIncrement();
        }
    }
}
