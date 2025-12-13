using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class SanPhamBUS
    {
        // Khởi tạo DAO theo mẫu Singleton
        public readonly SanPhamDAO spDAO = SanPhamDAO.Instance;
        private List<SanPhamDTO> listSP = new List<SanPhamDTO>();

        public SanPhamBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listSP = spDAO.selectAll() ?? new List<SanPhamDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo SanPhamBUS: {ex.Message}");
                listSP = new List<SanPhamDTO>();
            }
        }

        public List<SanPhamDTO> GetAll() => listSP;

        public SanPhamDTO GetByIndex(int index) => listSP[index];

        // LINQ: Lấy sản phẩm theo mã
        public SanPhamDTO? GetByMaSP(int masp)
            => listSP.FirstOrDefault(sp => sp.MSP == masp);

        // LINQ: Tìm index theo mã sản phẩm
        public int GetIndexByMaSP(int masanpham)
            => listSP.FindIndex(sp => sp.MSP == masanpham);

        public bool Add(SanPhamDTO sp)
        {
            if (spDAO.insert(sp) != 0)
            {
                LoadData();
                return true;
            }
            return false;
        }

        public bool Delete(SanPhamDTO sp)
        {
            // Xóa mềm bằng ID
            bool check = spDAO.delete(sp.MSP.ToString()) != 0;
            if (check)
            {
                listSP.RemoveAll(x => x.MSP == sp.MSP);
            }
            return check;
        }

        public bool Update(SanPhamDTO sp)
        {
            bool check = spDAO.update(sp) != 0;
            if (check)
            {
                int index = GetIndexByMaSP(sp.MSP);
                if (index != -1)
                {
                    listSP[index] = sp;
                }
            }
            return check;
        }

        // LINQ: Lấy sản phẩm theo Mã khu vực kho (MKVK)
        public List<SanPhamDTO> GetByMaKhuVuc(int makvk)
            => listSP.Where(sp => sp.MKVK == makvk).ToList();

        // LINQ: Search với nhiều tiêu chí
        public List<SanPhamDTO> Search(string text, string type)
        {
            text = text.ToLower();
            IEnumerable<SanPhamDTO> query = listSP;

            switch (type)
            {
                case "Mã SP":
                    query = query.Where(sp => sp.MSP.ToString().Contains(text));
                    break;
                case "Tên SP":
                    query = query.Where(sp => sp.TEN.ToLower().Contains(text));
                    break;
                case "Danh mục":
                    query = query.Where(sp => sp.DANHMUC.ToLower().Contains(text));
                    break;
                case "Giá xuất":
                    query = query.Where(sp => sp.TIENX.ToString().Contains(text));
                    break;
                case "Số lượng":
                    query = query.Where(sp => sp.SL.ToString().Contains(text));
                    break;
                default: // Tất cả
                    query = query.Where(sp =>
                        sp.MSP.ToString().Contains(text) ||
                        sp.TEN.ToLower().Contains(text) ||
                        sp.DANHMUC.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Overload hàm Search cho List tùy chỉnh
        public List<SanPhamDTO> Search(List<SanPhamDTO> listSource, string text, string type)
        {
            text = text.ToLower();
            IEnumerable<SanPhamDTO> query = listSource;

            switch (type)
            {
                case "Mã sản phẩm":
                    query = query.Where(sp => sp.MSP.ToString().Contains(text));
                    break;
                case "Tên sản phẩm":
                    query = query.Where(sp => sp.TEN.ToLower().Contains(text));
                    break;
                default: // Tất cả
                    query = query.Where(sp =>
                        sp.MSP.ToString().Contains(text) ||
                        sp.TEN.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        // Lấy sản phẩm theo danh mục (String) - giữ nguyên vì gọi DAO
        public List<SanPhamDTO> GetSpByDanhMuc(string danhmuc)
            => spDAO.selectByDanhMuc(danhmuc);

        // LINQ: Tính tổng số lượng sản phẩm
        public int GetQuantity()
            => listSP.Where(sp => sp.SL != 0).Sum(sp => sp.SL);

        // LINQ: Thêm nhiều sản phẩm
        public int AddMany(List<SanPhamDTO> listSP)
            => listSP.Count(sp => Add(sp));
    }
}
