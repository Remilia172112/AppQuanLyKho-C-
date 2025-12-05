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
            return listLSP;
        }

        public void Refresh()
        {
            listLSP = lspDAO.selectAll();
        }
    }
}
