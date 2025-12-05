using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhomQuyenBUS
    {
        private readonly NhomQuyenDAO nhomquyenDAO = NhomQuyenDAO.Instance;
        private readonly ChiTietQuyenDAO chitietquyenDAO = ChiTietQuyenDAO.Instance;
        private List<NhomQuyenDTO> listNhomQuyen = new List<NhomQuyenDTO>();

        public NhomQuyenBUS()
        {
            this.listNhomQuyen = nhomquyenDAO.selectAll();
        }

        public List<NhomQuyenDTO> GetAll()
        {
            return this.listNhomQuyen;
        }

        public NhomQuyenDTO GetByIndex(int index)
        {
            return this.listNhomQuyen[index];
        }

        // Thêm nhóm quyền mới và các chi tiết quyền kèm theo
        public bool Add(string tenNhomQuyen, List<ChiTietQuyenDTO> ctquyen)
        {
            // Lấy ID tự tăng tiếp theo từ DB
            int newId = nhomquyenDAO.getAutoIncrement();
            
            // Tạo đối tượng DTO
            NhomQuyenDTO nq = new NhomQuyenDTO(newId, tenNhomQuyen);
            
            // Insert nhóm quyền
            bool check = nhomquyenDAO.insert(nq) != 0;
            
            if (check)
            {
                // Thêm vào list cache
                this.listNhomQuyen.Add(nq);
                
                // Cập nhật lại Mã nhóm quyền cho các chi tiết quyền (để khớp với ID vừa tạo)
                foreach(var item in ctquyen)
                {
                    item.Manhomquyen = newId;
                }

                // Insert danh sách chi tiết quyền
                this.AddChiTietQuyen(ctquyen);
            }
            return check;
        }

        // Cập nhật nhóm quyền và danh sách chi tiết quyền
        public bool Update(NhomQuyenDTO nhomquyen, List<ChiTietQuyenDTO> chitietquyen, int index)
        {
            // Update tên nhóm quyền
            bool check = nhomquyenDAO.update(nhomquyen) != 0;
            
            if (check)
            {
                // Xóa hết quyền cũ
                this.RemoveChiTietQuyen(nhomquyen.Manhomquyen.ToString());
                
                // Thêm lại quyền mới
                // Đảm bảo mã nhóm quyền khớp
                foreach (var item in chitietquyen)
                {
                    item.Manhomquyen = nhomquyen.Manhomquyen;
                }
                this.AddChiTietQuyen(chitietquyen);
                
                // Cập nhật cache
                this.listNhomQuyen[index] = nhomquyen;
            }
            return check;
        }

        public bool Delete(NhomQuyenDTO nqdto)
        {
            // Xóa nhóm quyền (Xóa mềm trong DAO)
            bool check = nhomquyenDAO.delete(nqdto.Manhomquyen.ToString()) != 0;
            
            if (check)
            {
                this.listNhomQuyen.Remove(nqdto);
            }
            return check;
        }

        // --- Xử lý Chi tiết quyền ---

        public List<ChiTietQuyenDTO> GetChiTietQuyen(string manhomquyen)
        {
            return chitietquyenDAO.selectAll(manhomquyen);
        }

        public bool AddChiTietQuyen(List<ChiTietQuyenDTO> listctquyen)
        {
            return chitietquyenDAO.insert(listctquyen) != 0;
        }

        public bool RemoveChiTietQuyen(string manhomquyen)
        {
            return chitietquyenDAO.delete(manhomquyen) != 0;
        }

        // Kiểm tra xem 1 nhóm quyền (ID) có được phép làm hành động (Action) trên chức năng (MCN) không
        public bool CheckPermission(int maquyen, string chucnang, string hanhdong)
        {
            List<ChiTietQuyenDTO> ctquyen = this.GetChiTietQuyen(maquyen.ToString());
            
            foreach (var item in ctquyen)
            {
                if (item.Machucnang.Equals(chucnang) && item.Hanhdong.Equals(hanhdong))
                {
                    return true;
                }
            }
            return false;
        }

        public List<NhomQuyenDTO> Search(string text)
        {
            List<NhomQuyenDTO> result = new List<NhomQuyenDTO>();
            text = text.ToLower();

            foreach (NhomQuyenDTO i in this.listNhomQuyen)
            {
                if (i.Manhomquyen.ToString().Contains(text) || 
                    i.Tennhomquyen.ToLower().Contains(text))
                {
                    result.Add(i);
                }
            }
            return result;
        }
    }
}