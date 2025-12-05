using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhaSanXuatBUS
    {
        private readonly NhaSanXuatDAO nsxDAO = NhaSanXuatDAO.Instance;
        public List<NhaSanXuatDTO> listNSX = new List<NhaSanXuatDTO>();

        public NhaSanXuatBUS()
        {
            // Load danh sách nhà sản xuất từ DAO khi khởi tạo
            this.listNSX = nsxDAO.selectAll();
        }

        public List<NhaSanXuatDTO> GetAll()
        {
            return this.listNSX;
        }

        public NhaSanXuatDTO GetByIndex(int index)
        {
            return this.listNSX[index];
        }

        public bool Add(NhaSanXuatDTO nsx)
        {
            // Gọi DAO để insert vào DB
            bool check = nsxDAO.insert(nsx) != 0;
            if (check)
            {
                // Nếu thành công thì thêm vào list bộ nhớ
                this.listNSX.Add(nsx);
            }
            return check;
        }

        public bool Delete(NhaSanXuatDTO nsx, int index)
        {
            // Gọi DAO xóa (xóa mềm)
            bool check = nsxDAO.delete(nsx.MSX.ToString()) != 0;
            if (check)
            {
                this.listNSX.RemoveAt(index);
            }
            return check;
        }

        public bool Update(NhaSanXuatDTO nsx)
        {
            // Gọi DAO update
            bool check = nsxDAO.update(nsx) != 0;
            if (check)
            {
                // Cập nhật lại đối tượng trong list
                int index = GetIndexByMaNSX(nsx.MSX);
                if (index != -1)
                {
                    this.listNSX[index] = nsx;
                }
            }
            return check;
        }

        public int GetIndexByMaNSX(int mansx)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listNSX.Count && vitri == -1)
            {
                if (listNSX[i].MSX == mansx) // DTO C# dùng MSX
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

        public List<NhaSanXuatDTO> Search(string txt, string type)
        {
            List<NhaSanXuatDTO> result = new List<NhaSanXuatDTO>();
            txt = txt.ToLower();

            switch (type)
            {
                case "Tất cả":
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.MSX.ToString().Contains(txt) || 
                            i.TEN.ToLower().Contains(txt) || 
                            i.DIACHI.ToLower().Contains(txt) || 
                            i.EMAIL.ToLower().Contains(txt) || 
                            i.SDT.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã nhà sản xuất": // Đổi tên cho khớp ngữ cảnh
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.MSX.ToString().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Tên nhà sản xuất":
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.TEN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Địa chỉ":
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.DIACHI.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Số điện thoại":
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.SDT.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Email":
                    foreach (NhaSanXuatDTO i in listNSX)
                    {
                        if (i.EMAIL.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }
            return result;
        }

        public string[] GetArrTenNhaSanXuat()
        {
            int size = listNSX.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listNSX[i].TEN;
            }
            return result;
        }

        public string GetTenNhaSanXuat(int mansx)
        {
            int index = GetIndexByMaNSX(mansx);
            if (index != -1)
            {
                return this.listNSX[index].TEN;
            }
            return "";
        }

        public NhaSanXuatDTO FindCT(List<NhaSanXuatDTO> nsxList, string tennsx)
        {
            foreach (var nsx in nsxList)
            {
                if (nsx.TEN.Equals(tennsx, StringComparison.OrdinalIgnoreCase))
                {
                    return nsx;
                }
            }
            return null;
        }
    }
}