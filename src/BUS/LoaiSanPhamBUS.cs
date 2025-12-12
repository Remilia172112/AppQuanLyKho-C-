using System.Collections.Generic;
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
            listLSP = lspDAO.selectAll();
        }

        public List<LoaiSanPhamDTO> GetAll()
        {
            // Luôn lấy dữ liệu mới nhất khi gọi GetAll để đảm bảo đồng bộ
            listLSP = lspDAO.selectAll();
            return listLSP;
        }

        public bool Add(LoaiSanPhamDTO lsp)
        {
            if (lspDAO.Insert(lsp) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Update(LoaiSanPhamDTO lsp)
        {
            if (lspDAO.Update(lsp) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(int mlsp)
        {
            if (lspDAO.Delete(mlsp) > 0)
            {
                return true;
            }
            return false;
        }
        
        public LoaiSanPhamDTO GetById(int mlsp)
        {
            return listLSP.Find(x => x.MLSP == mlsp);
        }
        // Thêm đoạn này vào trong class LoaiSanPhamBUS
        public int AddMany(List<LoaiSanPhamDTO> listLSP)
        {
            int successCount = 0;
            foreach (var lsp in listLSP)
            {
                // Kiểm tra trùng tên (nếu cần) trước khi add
                // var exists = listLSP.Find(x => x.TEN.Equals(lsp.TEN, StringComparison.OrdinalIgnoreCase));
                
                if (Add(lsp))
                {
                    successCount++;
                }
            }
            return successCount;
        }
    }
}