using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAO NccDAO = NhaCungCapDAO.Instance;
        private List<NhaCungCapDTO> listNcc = new List<NhaCungCapDTO>();

        public NhaCungCapBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listNcc = NccDAO.selectAll() ?? new List<NhaCungCapDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData NhaCungCapBUS: {ex.Message}");
                listNcc = new List<NhaCungCapDTO>();
            }
        }

        public List<NhaCungCapDTO> GetAll() => listNcc;

        public NhaCungCapDTO GetByIndex(int index) => listNcc[index];

        // LINQ: Tìm index theo mã nhà cung cấp
        public int GetIndexByMaNCC(int mancc)
            => listNcc.FindIndex(ncc => ncc.MNCC == mancc);

        // LINQ: Lấy nhà cung cấp theo mã
        public NhaCungCapDTO? GetById(int mancc)
            => listNcc.FirstOrDefault(ncc => ncc.MNCC == mancc);

        public bool Add(NhaCungCapDTO ncc)
        {
            if (NccDAO.insert(ncc) != 0)
            {
                LoadData(); // Reload để đồng bộ với DB
                return true;
            }
            return false;
        }

        public bool Delete(NhaCungCapDTO ncc)
        {
            bool check = NccDAO.delete(ncc.MNCC.ToString()) != 0;
            if (check)
            {
                listNcc.Remove(ncc);
            }
            return check;
        }

        // Overload Delete với index (tương thích ngược)
        public bool Delete(NhaCungCapDTO ncc, int index)
        {
            bool check = NccDAO.delete(ncc.MNCC.ToString()) != 0;
            if (check)
            {
                listNcc.RemoveAt(index);
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
                    listNcc[index] = ncc;
                }
            }
            return check;
        }

        // LINQ: Search với nhiều tiêu chí
        public List<NhaCungCapDTO> Search(string txt, string type)
        {
            txt = txt.ToLower();
            IEnumerable<NhaCungCapDTO> query = listNcc;

            switch (type)
            {
                case "Mã nhà cung cấp":
                    query = query.Where(ncc => ncc.MNCC.ToString().Contains(txt));
                    break;
                case "Tên nhà cung cấp":
                    query = query.Where(ncc => ncc.TEN.ToLower().Contains(txt));
                    break;
                case "Địa chỉ":
                    query = query.Where(ncc => ncc.DIACHI.ToLower().Contains(txt));
                    break;
                case "Số điện thoại":
                    query = query.Where(ncc => ncc.SDT.ToLower().Contains(txt));
                    break;
                case "Email":
                    query = query.Where(ncc => ncc.EMAIL.ToLower().Contains(txt));
                    break;
                default: // Tất cả
                    query = query.Where(ncc =>
                        ncc.MNCC.ToString().Contains(txt) ||
                        ncc.TEN.ToLower().Contains(txt) ||
                        ncc.DIACHI.ToLower().Contains(txt) ||
                        ncc.EMAIL.ToLower().Contains(txt) ||
                        ncc.SDT.ToLower().Contains(txt));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Lấy mảng tên nhà cung cấp
        public string[] GetArrTenNhaCungCap()
            => listNcc.Select(ncc => ncc.TEN).ToArray();

        // LINQ: Lấy tên nhà cung cấp theo mã
        public string GetTenNhaCungCap(int mancc)
            => listNcc.FirstOrDefault(ncc => ncc.MNCC == mancc)?.TEN ?? "";

        // LINQ: Tìm nhà cung cấp theo tên trong list
        public NhaCungCapDTO? FindCT(List<NhaCungCapDTO> nccList, string tenncc)
            => nccList.FirstOrDefault(ncc => ncc.TEN.Equals(tenncc, StringComparison.OrdinalIgnoreCase));

        // LINQ: Tìm nhà cung cấp theo tên trong list hiện tại
        public NhaCungCapDTO? FindByTen(string tenncc)
            => listNcc.FirstOrDefault(ncc => ncc.TEN.Equals(tenncc, StringComparison.OrdinalIgnoreCase));
        public int AddMany(List<NhaCungCapDTO> listNCC)
        {
            return listNCC.Count(ncc => Add(ncc));
        }
    }
}
