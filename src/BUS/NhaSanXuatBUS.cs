using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class NhaSanXuatBUS
    {
        private readonly NhaSanXuatDAO nsxDAO = NhaSanXuatDAO.Instance;
        private List<NhaSanXuatDTO> listNSX = new List<NhaSanXuatDTO>();

        public NhaSanXuatBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listNSX = nsxDAO.selectAll() ?? new List<NhaSanXuatDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData NhaSanXuatBUS: {ex.Message}");
                listNSX = new List<NhaSanXuatDTO>();
            }
        }

        public List<NhaSanXuatDTO> GetAll() => listNSX;

        public NhaSanXuatDTO GetByIndex(int index) => listNSX[index];

        // LINQ: Tìm index theo mã nhà sản xuất
        public int GetIndexByMaNSX(int mansx)
            => listNSX.FindIndex(nsx => nsx.MNSX == mansx);

        // LINQ: Lấy nhà sản xuất theo mã
        public NhaSanXuatDTO? GetById(int mansx)
            => listNSX.FirstOrDefault(nsx => nsx.MNSX == mansx);

        public bool Add(NhaSanXuatDTO nsx)
        {
            if (nsxDAO.insert(nsx) != 0)
            {
                LoadData(); // Reload để đồng bộ với DB
                return true;
            }
            return false;
        }

        public bool Delete(NhaSanXuatDTO nsx)
        {
            bool check = nsxDAO.delete(nsx.MNSX.ToString()) != 0;
            if (check)
            {
                listNSX.RemoveAll(x => x.MNSX == nsx.MNSX);
            }
            return check;
        }

        // Overload Delete với index (tương thích ngược)
        public bool Delete(NhaSanXuatDTO nsx, int index)
        {
            bool check = nsxDAO.delete(nsx.MNSX.ToString()) != 0;
            if (check)
            {
                listNSX.RemoveAt(index);
            }
            return check;
        }

        public bool Update(NhaSanXuatDTO nsx)
        {
            bool check = nsxDAO.update(nsx) != 0;
            if (check)
            {
                int index = GetIndexByMaNSX(nsx.MNSX);
                if (index != -1)
                {
                    listNSX[index] = nsx;
                }
            }
            return check;
        }

        // LINQ: Search với nhiều tiêu chí
        public List<NhaSanXuatDTO> Search(string txt, string type)
        {
            txt = txt.ToLower();
            IEnumerable<NhaSanXuatDTO> query = listNSX;

            switch (type)
            {
                case "Mã NSX":
                    query = query.Where(nsx => nsx.MNSX.ToString().Contains(txt));
                    break;
                case "Tên NSX":
                    query = query.Where(nsx => nsx.TEN.ToLower().Contains(txt));
                    break;
                case "Địa chỉ":
                    query = query.Where(nsx => nsx.DIACHI.ToLower().Contains(txt));
                    break;
                case "Số điện thoại":
                    query = query.Where(nsx => nsx.SDT.ToLower().Contains(txt));
                    break;
                case "Email":
                    query = query.Where(nsx => nsx.EMAIL.ToLower().Contains(txt));
                    break;
                default: // Tất cả
                    query = query.Where(nsx =>
                        nsx.MNSX.ToString().Contains(txt) ||
                        nsx.TEN.ToLower().Contains(txt) ||
                        nsx.DIACHI.ToLower().Contains(txt) ||
                        nsx.EMAIL.ToLower().Contains(txt) ||
                        nsx.SDT.ToLower().Contains(txt));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Lấy mảng tên nhà sản xuất
        public string[] GetArrTenNhaSanXuat()
            => listNSX.Select(nsx => nsx.TEN).ToArray();

        // LINQ: Lấy tên nhà sản xuất theo mã
        public string GetTenNhaSanXuat(int mansx)
            => listNSX.FirstOrDefault(nsx => nsx.MNSX == mansx)?.TEN ?? "";

        // LINQ: Tìm nhà sản xuất theo tên trong list
        public NhaSanXuatDTO? FindCT(List<NhaSanXuatDTO> nsxList, string tennsx)
            => nsxList.FirstOrDefault(nsx =>
                nsx.TEN.Equals(tennsx, StringComparison.OrdinalIgnoreCase));

        // LINQ: Tìm nhà sản xuất theo tên trong list hiện tại
        public NhaSanXuatDTO? FindByTen(string tennsx)
            => listNSX.FirstOrDefault(nsx =>
                nsx.TEN.Equals(tennsx, StringComparison.OrdinalIgnoreCase));

        public int AddMany(List<NhaSanXuatDTO> listNSX)
        {
            return listNSX.Count(nsx => Add(nsx));
        }
    }
}
