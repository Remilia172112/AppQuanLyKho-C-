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
            // Load dữ liệu ban đầu
            try
            {
                listSP = spDAO.selectAll();
            }
            catch (Exception ex)
            {
                listSP = new List<SanPhamDTO>();
            }
        }

        public List<SanPhamDTO> GetAll()
        {
            return this.listSP;
        }

        public SanPhamDTO GetByIndex(int index)
        {
            return this.listSP[index];
        }

        public SanPhamDTO GetByMaSP(int masp)
        {
            foreach (var sp in listSP)
            {
                if (sp.MSP == masp)
                {
                    return sp;
                }
            }
            return null;
        }

        public int GetIndexByMaSP(int masanpham)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listSP.Count && vitri == -1)
            {
                if (listSP[i].MSP == masanpham)
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

        public bool Add(SanPhamDTO sp)
        {
            bool check = spDAO.insert(sp) != 0;
            if (check)
            {
                this.listSP.Add(sp);
            }
            return check;
        }

        public bool Delete(SanPhamDTO sp)
        {
            // Xóa mềm bằng ID
            bool check = spDAO.delete(sp.MSP.ToString()) != 0;
            if (check)
            {
                this.listSP.Remove(sp);
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
                    this.listSP[index] = sp;
                }
            }
            return check;
        }

        // Lấy sản phẩm theo Mã khu vực kho (MKVK)
        public List<SanPhamDTO> GetByMaKhuVuc(int makvk)
        {
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            foreach (SanPhamDTO i in this.listSP)
            {
                // Sử dụng MKVK (theo SQL mới) thay vì MKVS (sách cũ)
                if (i.MKVK == makvk)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public List<SanPhamDTO> Search(string text, string type)
        {
            text = text.ToLower();
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            
            switch (type)
            {
                case "Tất cả":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        if (i.MSP.ToString().Contains(text) || 
                            i.TEN.ToLower().Contains(text) ||
                            i.DANHMUC.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã sản phẩm":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        if (i.MSP.ToString().Contains(text)) result.Add(i);
                    }
                    break;
                case "Tên sản phẩm":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        if (i.TEN.ToLower().Contains(text)) result.Add(i);
                    }
                    break;
                case "Danh mục":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        if (i.DANHMUC.ToLower().Contains(text)) result.Add(i);
                    }
                    break;
                case "Giá xuất":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        // Tìm kiếm tương đối: nhập "50" sẽ ra giá 500, 50000...
                        if (i.TIENX.ToString().Contains(text)) result.Add(i);
                    }
                    break;
                case "Số lượng":
                    foreach (SanPhamDTO i in this.listSP)
                    {
                        if (i.SL.ToString().Contains(text)) result.Add(i);
                    }
                    break;
                // ---------------------
            }
            return result;
        }
        // Overload hàm Search cho List tùy chỉnh
        public List<SanPhamDTO> Search(List<SanPhamDTO> listSource, string text, string type)
        {
            text = text.ToLower();
            List<SanPhamDTO> result = new List<SanPhamDTO>();
            
            switch (type)
            {
                case "Tất cả":
                    foreach (SanPhamDTO i in listSource)
                    {
                        if (i.MSP.ToString().Contains(text) || i.TEN.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã sản phẩm":
                    foreach (SanPhamDTO i in listSource)
                    {
                        if (i.MSP.ToString().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Tên sản phẩm":
                    foreach (SanPhamDTO i in listSource)
                    {
                        if (i.TEN.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }
            return result;
        }

        // Lấy sản phẩm theo danh mục (String)
        public List<SanPhamDTO> GetSpByDanhMuc(string danhmuc)
        {
            return spDAO.selectByDanhMuc(danhmuc);
        }

        public int GetQuantity()
        {
            int n = 0;
            foreach(SanPhamDTO i in this.listSP)
            {
                if (i.SL != 0)
                {
                    n += i.SL;
                }
            }
            return n;
        }
        public int AddMany(List<SanPhamDTO> listSP)
        {
            int successCount = 0;
            foreach (var sp in listSP)
            {
                // Có thể thêm logic kiểm tra trùng tên ở đây nếu muốn
                if (Add(sp))
                {
                    successCount++;
                }
            }
            return successCount;
        }
    }
}