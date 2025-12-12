using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAO khDAO = KhachHangDAO.Instance;
        public List<KhachHangDTO> listKhachHang = new List<KhachHangDTO>();

        public KhachHangBUS()
        {
            try
            {
                // Lấy danh sách khách hàng đang hoạt động (TT=1)
                var data = khDAO.selectAll();
                listKhachHang = data ?? new List<KhachHangDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo KhachHangBUS: {ex.Message}");
                listKhachHang = new List<KhachHangDTO>();
            }
        }

        public List<KhachHangDTO> GetAll()
        {
            return this.listKhachHang;
        }

        public KhachHangDTO GetByIndex(int index)
        {
            return this.listKhachHang[index];
        }

        public int GetByTen(string ten)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listKhachHang.Count && vitri == -1)
            {
                if (listKhachHang[i].HOTEN.Equals(ten))
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

        public int GetIndexByMaKH(int makhachhang)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listKhachHang.Count && vitri == -1)
            {
                if (listKhachHang[i].MKH == makhachhang)
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

        public bool Add(KhachHangDTO kh)
        {
            bool check = khDAO.insert(kh) != 0;
            if (check)
            {
                this.listKhachHang.Add(kh);
            }
            return check;
        }

    public bool Delete(KhachHangDTO kh)
    {
        bool check = khDAO.delete(kh.MKH.ToString()) != 0;
        if (check)
        {
            int index = GetIndexByMaKH(kh.MKH);
            if (index != -1)
            {
                this.listKhachHang.RemoveAt(index);
            }
        }
        return check;
    }

    public KhachHangDTO GetById(int makh)
    {
        int index = GetIndexByMaKH(makh);
        return index >= 0 ? listKhachHang[index] : null;
    }        public bool Update(KhachHangDTO kh)
        {
            bool check = khDAO.update(kh) != 0;
            if (check)
            {
                int index = GetIndexByMaKH(kh.MKH);
                if (index != -1)
                {
                    this.listKhachHang[index] = kh;
                }
            }
            return check;
        }

        public List<KhachHangDTO> Search(string text, string type)
        {
            List<KhachHangDTO> result = new List<KhachHangDTO>();
            text = text.ToLower();

            switch (type)
            {
                case "Tất cả":
                    foreach (KhachHangDTO i in this.listKhachHang)
                    {
                        if (i.MKH.ToString().ToLower().Contains(text) || 
                            i.HOTEN.ToLower().Contains(text) || 
                            i.DIACHI.ToLower().Contains(text) || 
                            i.SDT.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã khách hàng":
                    foreach (KhachHangDTO i in this.listKhachHang)
                    {
                        if (i.MKH.ToString().ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Tên khách hàng":
                    foreach (KhachHangDTO i in this.listKhachHang)
                    {
                        if (i.HOTEN.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Địa chỉ":
                    foreach (KhachHangDTO i in this.listKhachHang)
                    {
                        if (i.DIACHI.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Số điện thoại":
                    foreach (KhachHangDTO i in this.listKhachHang)
                    {
                        if (i.SDT.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }

            return result;
        }

        public string GetTenKhachHang(int makh)
        {
            string name = "";
            foreach (KhachHangDTO kh in listKhachHang)
            {
                if (kh.MKH == makh)
                {
                    name = kh.HOTEN;
                    break; // Tìm thấy thì thoát vòng lặp
                }
            }
            return name;
        }

        public string[] GetArrTenKhachHang()
        {
            int size = listKhachHang.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listKhachHang[i].HOTEN;
            }
            return result;
        }

        public KhachHangDTO SelectKh(int makh)
        {
            return khDAO.selectById(makh.ToString());
        }

        // Lấy mã khách hàng lớn nhất (Có thể dùng để tự tăng ID nếu cần)
        public int GetMKHMAX()
        {
            int s = 1;
            // Lấy danh sách full (kể cả đã xóa) để đảm bảo ID không trùng
            List<KhachHangDTO> allList = khDAO.SelectAllFull(); 
            foreach (KhachHangDTO i in allList)
            {
                if (i.MKH > s) s = i.MKH;
            }
            return s;
        }
    }
}