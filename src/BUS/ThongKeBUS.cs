using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO.ThongKe;

namespace src.BUS
{
    public class ThongKeBUS
    {
        private readonly ThongKeDAO thongkeDAO = ThongKeDAO.Instance;
        private List<ThongKeKhachHangDTO> tkkh = new List<ThongKeKhachHangDTO>();
        private List<ThongKeNhaCungCapDTO> tkncc = new List<ThongKeNhaCungCapDTO>();
        private List<ThongKeTonKhoDTO> listTonKho = new List<ThongKeTonKhoDTO>();

        public ThongKeBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                // Lấy thống kê tồn kho từ đầu đến hiện tại
                listTonKho = thongkeDAO.GetThongKeTonKho("", DateTime.MinValue, DateTime.Now) ?? new List<ThongKeTonKhoDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo ThongKeBUS: {ex.Message}");
                listTonKho = new List<ThongKeTonKhoDTO>();
            }
        }

        public List<ThongKeKhachHangDTO> GetAllKhachHang()
        {
            tkkh = thongkeDAO.GetThongKeKhachHang("", DateTime.MinValue, DateTime.Now);
            return tkkh;
        }

        public List<ThongKeKhachHangDTO> FilterKhachHang(string text, DateTime start, DateTime end)
        {
            tkkh = thongkeDAO.GetThongKeKhachHang(text, start, end);
            return tkkh;
        }

        public List<ThongKeNhaCungCapDTO> GetAllNCC()
        {
            tkncc = thongkeDAO.GetThongKeNCC("", DateTime.MinValue, DateTime.Now);
            return tkncc;
        }

        public List<ThongKeNhaCungCapDTO> FilterNCC(string text, DateTime start, DateTime end)
        {
            tkncc = thongkeDAO.GetThongKeNCC(text, start, end);
            return tkncc;
        }

        public List<ThongKeTonKhoDTO> GetTonKho() => listTonKho;

        public List<ThongKeTonKhoDTO> FilterTonKho(string text, DateTime timeStart, DateTime timeEnd)
            => thongkeDAO.GetThongKeTonKho(text, timeStart, timeEnd);

        // LINQ: Tính tổng số lượng từ danh sách tồn kho
        public int[] GetSoluong(List<ThongKeTonKhoDTO> list)
        {
            return new int[]
            {
                list.Sum(item => item.Tondauky),
                list.Sum(item => item.Nhaptrongky),
                list.Sum(item => item.Xuattrongky),
                list.Sum(item => item.Toncuoiky)
            };
        }

        public List<ThongKeDoanhThuDTO> GetDoanhThuTheoTungNam(int yearStart, int yearEnd)
            => thongkeDAO.GetDoanhThuTheoTungNam(yearStart, yearEnd);

        public List<ThongKeTheoThangDTO> GetThongKeTheoThang(int nam)
            => thongkeDAO.GetThongKeTheoThang(nam);

        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTungNgayTrongThang(int thang, int nam)
            => thongkeDAO.GetThongKeTungNgayTrongThang(thang, nam);

        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTuNgayDenNgay(string start, string end)
            => thongkeDAO.GetThongKeTuNgayDenNgay(start, end);

        public List<ThongKeTungNgayTrongThangDTO> GetThongKe7NgayGanNhat()
            => thongkeDAO.GetThongKe7NgayGanNhat();
    }
}
