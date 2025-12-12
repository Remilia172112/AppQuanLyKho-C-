using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class LoaiSanPhamBUS
    {
        private readonly LoaiSanPhamDAO lspDAO = LoaiSanPhamDAO.Instance;
        private List<LoaiSanPhamDTO> listLSP = new List<LoaiSanPhamDTO>();

        public LoaiSanPhamBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listLSP = lspDAO.selectAll() ?? new List<LoaiSanPhamDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData LoaiSanPhamBUS: {ex.Message}");
                listLSP = new List<LoaiSanPhamDTO>();
            }
        }

        public List<LoaiSanPhamDTO> GetAll()
        {
            LoadData(); // Luôn lấy dữ liệu mới nhất
            return listLSP;
        }

        // LINQ: Lấy loại sản phẩm theo mã
        public LoaiSanPhamDTO? GetById(int mlsp)
            => listLSP.FirstOrDefault(x => x.MLSP == mlsp);

        // LINQ: Tìm index theo mã loại sản phẩm
        public int GetIndexByMaLSP(int maloaisp)
            => listLSP.FindIndex(lsp => lsp.MLSP == maloaisp);

        // LINQ: Lấy tên loại sản phẩm theo mã
        public string GetTenLoaiSP(int maloaisp)
            => listLSP.FirstOrDefault(lsp => lsp.MLSP == maloaisp)?.TEN ?? "";

        // LINQ: Lấy mảng tên loại sản phẩm
        public string[] GetArrTenLoaiSP()
            => listLSP.Select(lsp => lsp.TEN).ToArray();

        // Rút gọn với expression body
        public bool Add(LoaiSanPhamDTO lsp) => lspDAO.Insert(lsp) > 0;

        public bool Update(LoaiSanPhamDTO lsp) => lspDAO.Update(lsp) > 0;

        public bool Delete(int mlsp) => lspDAO.Delete(mlsp) > 0;

        // LINQ: Thêm nhiều với Count
        public int AddMany(List<LoaiSanPhamDTO> list)
            => list.Count(lsp => Add(lsp));

        // LINQ: Tìm kiếm theo tên
        public List<LoaiSanPhamDTO> Search(string text, string type)
        {
            text = text.ToLower();
            IEnumerable<LoaiSanPhamDTO> query = listLSP;

            switch (type)
            {
                case "Mã loại SP":
                    query = query.Where(lsp => lsp.MLSP.ToString().Contains(text));
                    break;
                case "Tên loại SP":
                    query = query.Where(lsp => lsp.TEN.ToLower().Contains(text));
                    break;
                default: // Tất cả
                    query = query.Where(lsp =>
                        lsp.MLSP.ToString().Contains(text) ||
                        lsp.TEN.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Tìm loại sản phẩm theo tên (exact match)
        public LoaiSanPhamDTO? FindByTen(string ten)
            => listLSP.FirstOrDefault(lsp =>
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));

        // LINQ: Kiểm tra tên đã tồn tại
        public bool IsTenExists(string ten)
            => listLSP.Any(lsp =>
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));

        // LINQ: Kiểm tra tên đã tồn tại (ngoại trừ ID hiện tại - dùng khi update)
        public bool IsTenExists(string ten, int excludeId)
            => listLSP.Any(lsp =>
                lsp.MLSP != excludeId &&
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));
    }
}
