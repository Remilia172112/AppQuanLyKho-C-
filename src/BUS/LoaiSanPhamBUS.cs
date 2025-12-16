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
            LoadData();
            return listLSP;
        }

        public LoaiSanPhamDTO? GetById(int mlsp)
            => listLSP.FirstOrDefault(x => x.MLSP == mlsp);

        public int GetIndexByMaLSP(int maloaisp)
            => listLSP.FindIndex(lsp => lsp.MLSP == maloaisp);

        // --- CẬP NHẬT THÊM THAM SỐ tlgx ---
        public bool Add(string ten, int tlgx, string ghichu)
        {
            LoaiSanPhamDTO lsp = new LoaiSanPhamDTO
            {
                TEN = ten,
                TLGX = tlgx, // <--- MỚI
                GHICHU = ghichu,
                TT = 1
            };

            if (lspDAO.insert(lsp) > 0)
            {
                LoadData(); 
                return true;
            }
            return false;
        }

        // --- CẬP NHẬT THÊM THAM SỐ tlgx ---
        public bool Update(int mlsp, string ten, int tlgx, string ghichu)
        {
            LoaiSanPhamDTO lsp = new LoaiSanPhamDTO
            {
                MLSP = mlsp,
                TEN = ten,
                TLGX = tlgx, // <--- MỚI
                GHICHU = ghichu,
                TT = 1
            };

            if (lspDAO.update(lsp) > 0)
            {
                LoadData(); 
                return true;
            }
            return false;
        }

        public bool Delete(int mlsp)
        {
            if (lspDAO.delete(mlsp) > 0)
            {
                LoadData(); 
                return true;
            }
            return false;
        }

        public int AddMany(List<LoaiSanPhamDTO> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                // Kiểm tra trùng tên nếu cần
                if (!IsTenExists(item.TEN))
                {
                    if (lspDAO.insert(item) > 0) count++;
                }
            }
            if (count > 0) LoadData();
            return count;
        }

        public List<LoaiSanPhamDTO> Search(string text, string type = "Tất cả")
        {
            text = text.ToLower();
            var query = listLSP.AsQueryable();

            switch (type)
            {
                case "Mã loại SP":
                    query = query.Where(lsp => lsp.MLSP.ToString().Contains(text));
                    break;
                case "Tên loại SP":
                    query = query.Where(lsp => lsp.TEN.ToLower().Contains(text));
                    break;
                default: 
                    query = query.Where(lsp =>
                        lsp.MLSP.ToString().Contains(text) ||
                        lsp.TEN.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        public LoaiSanPhamDTO? FindByTen(string ten)
            => listLSP.FirstOrDefault(lsp =>
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));

        public bool IsTenExists(string ten)
            => listLSP.Any(lsp =>
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));

        public bool IsTenExists(string ten, int excludeId)
            => listLSP.Any(lsp =>
                lsp.MLSP != excludeId &&
                lsp.TEN.Equals(ten, StringComparison.OrdinalIgnoreCase));
                
        public int getAutoIncrement()
        {
            return lspDAO.getAutoIncrement();
        }
    }
}