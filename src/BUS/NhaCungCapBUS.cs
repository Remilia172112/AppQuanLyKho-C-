using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAO NccDAO = NhaCungCapDAO.Instance;
        public List<NhaCungCapDTO> listNcc = new List<NhaCungCapDTO>();

        public NhaCungCapBUS()
        {
            this.listNcc = NccDAO.selectAll();
        }

        public List<NhaCungCapDTO> GetAll()
        {
            return this.listNcc;
        }

        public NhaCungCapDTO GetByIndex(int index)
        {
            return this.listNcc[index];
        }

        public bool Add(NhaCungCapDTO ncc)
        {
            // Kiểm tra trùng lặp nếu cần
            bool check = NccDAO.insert(ncc) != 0;
            if (check)
            {
                this.listNcc.Add(ncc);
            }
            return check;
        }

        public bool Delete(NhaCungCapDTO ncc, int index)
        {
            // Xóa mềm trong CSDL
            bool check = NccDAO.delete(ncc.MNCC.ToString()) != 0;
            if (check)
            {
                // Xóa khỏi danh sách bộ nhớ
                this.listNcc.RemoveAt(index);
            }
            return check;
        }

        public bool Update(NhaCungCapDTO ncc)
        {
            bool check = NccDAO.update(ncc) != 0;
            if (check)
            {
                int index = GetIndexByMaNCC(ncc.MNCC);
                if (index != -1)
                {
                    this.listNcc[index] = ncc;
                }
            }
            return check;
        }

        public int GetIndexByMaNCC(int mancc)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listNcc.Count && vitri == -1)
            {
                if (listNcc[i].MNCC == mancc) // Sử dụng Property MNCC
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

        public NhaCungCapDTO GetById(int mancc)
        {
            int index = GetIndexByMaNCC(mancc);
            if (index != -1)
            {
                return this.listNcc[index];
            }
            return null;
        }

        public List<NhaCungCapDTO> Search(string txt, string type)
        {
            List<NhaCungCapDTO> result = new List<NhaCungCapDTO>();
            txt = txt.ToLower();

            switch (type)
            {
                case "Tất cả":
                    foreach (NhaCungCapDTO i in listNcc)
                    {
                        if (i.MNCC.ToString().Contains(txt) || 
                            i.TEN.ToLower().Contains(txt) || 
                            i.DIACHI.ToLower().Contains(txt) || 
                            i.EMAIL.ToLower().Contains(txt) || 
                            i.SDT.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã nhà cung cấp":
                    foreach (NhaCungCapDTO i in listNcc)
                    {
                        if (i.MNCC.ToString().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Tên nhà cung cấp":
                    foreach (NhaCungCapDTO i in listNcc)
                    {
                        if (i.TEN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Địa chỉ":
                    foreach (NhaCungCapDTO i in listNcc)
                    {
                        if (i.DIACHI.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Số điện thoại":
                    foreach (NhaCungCapDTO i in listNcc)
                    {
                        if (i.SDT.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Email":
                    foreach (NhaCungCapDTO i in listNcc)
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

        public string[] GetArrTenNhaCungCap()
        {
            int size = listNcc.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listNcc[i].TEN;
            }
            return result;
        }

        public string GetTenNhaCungCap(int mancc)
        {
            int index = GetIndexByMaNCC(mancc);
            if (index != -1)
            {
                return this.listNcc[index].TEN;
            }
            return "";
        }

        public NhaCungCapDTO FindCT(List<NhaCungCapDTO> nccList, string tenncc)
        {
            foreach (var ncc in nccList)
            {
                if (ncc.TEN.Equals(tenncc, StringComparison.OrdinalIgnoreCase))
                {
                    return ncc;
                }
            }
            return null;
        }
    }
}