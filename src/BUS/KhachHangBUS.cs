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
        private List<KhachHangDTO> listKhachHang = new List<KhachHangDTO>();

        public KhachHangBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listKhachHang = khDAO.selectAll() ?? new List<KhachHangDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo KhachHangBUS: {ex.Message}");
                listKhachHang = new List<KhachHangDTO>();
            }
        }

        public List<KhachHangDTO> GetAll() => listKhachHang;

        public KhachHangDTO GetByIndex(int index) => listKhachHang[index];

        // LINQ: Tìm index theo tên
        public int GetByTen(string ten)
            => listKhachHang.FindIndex(kh => kh.HOTEN.Equals(ten));

        // LINQ: Tìm index theo mã khách hàng
        public int GetIndexByMaKH(int makhachhang)
            => listKhachHang.FindIndex(kh => kh.MKH == makhachhang);

        // LINQ: Lấy khách hàng theo mã
        public KhachHangDTO? GetById(int makh)
            => listKhachHang.FirstOrDefault(kh => kh.MKH == makh);

        public bool Add(KhachHangDTO kh)
        {
            if (khDAO.insert(kh) != 0)
            {
                listKhachHang.Add(kh);
                return true;
            }
            return false;
        }

        public bool Delete(KhachHangDTO kh)
        {
            bool check = khDAO.delete(kh.MKH.ToString()) != 0;
            if (check)
            {
                listKhachHang.RemoveAll(x => x.MKH == kh.MKH);
            }
            return check;
        }

        public bool Update(KhachHangDTO kh)
        {
            bool check = khDAO.update(kh) != 0;
            if (check)
            {
                int index = GetIndexByMaKH(kh.MKH);
                if (index != -1)
                {
                    listKhachHang[index] = kh;
                }
            }
            return check;
        }

        // LINQ: Search với nhiều tiêu chí
        public List<KhachHangDTO> Search(string text, string type)
        {
            text = text.ToLower();
            IEnumerable<KhachHangDTO> query = listKhachHang;

            switch (type)
            {
                case "Mã KH":
                    query = query.Where(kh => kh.MKH.ToString().Contains(text));
                    break;
                case "Họ tên":
                    query = query.Where(kh => kh.HOTEN.ToLower().Contains(text));
                    break;
                case "Địa chỉ":
                    query = query.Where(kh => kh.DIACHI.ToLower().Contains(text));
                    break;
                case "Số điện thoại":
                    query = query.Where(kh => kh.SDT.ToLower().Contains(text));
                    break;
                default:
                    query = query.Where(kh =>
                        kh.MKH.ToString().Contains(text) ||
                        kh.HOTEN.ToLower().Contains(text) ||
                        kh.DIACHI.ToLower().Contains(text) ||
                        kh.SDT.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Lấy tên khách hàng theo mã
        public string GetTenKhachHang(int makh)
            => listKhachHang.FirstOrDefault(kh => kh.MKH == makh)?.HOTEN ?? "";

        // LINQ: Lấy mảng tên khách hàng
        public string[] GetArrTenKhachHang()
            => listKhachHang.Select(kh => kh.HOTEN).ToArray();

        public KhachHangDTO? SelectKh(int makh)
            => khDAO.selectById(makh.ToString());

        // LINQ: Lấy mã khách hàng lớn nhất
        public int GetMKHMAX()
        {
            var allList = khDAO.SelectAllFull();
            return allList.Any() ? allList.Max(kh => kh.MKH) : 1;
        }

        // LINQ: Tìm khách hàng theo số điện thoại
        public KhachHangDTO? FindByPhone(string phone)
            => listKhachHang.FirstOrDefault(kh => kh.SDT == phone);

        // LINQ: Tìm khách hàng theo email
        public KhachHangDTO? FindByEmail(string email)
            => listKhachHang.FirstOrDefault(kh =>
                kh.EMAIL.Equals(email, StringComparison.OrdinalIgnoreCase));

        public int AddMany(List<KhachHangDTO> listKH)
        {
            return listKH.Count(kh => Add(kh));
        }
    }

}
