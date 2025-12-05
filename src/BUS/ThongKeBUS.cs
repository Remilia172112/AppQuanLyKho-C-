using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO.ThongKe;

namespace src.BUS
{
    public class ThongKeBUS
    {
        private readonly ThongKeDAO thongkeDAO = ThongKeDAO.Instance;
        private List<ThongKeKhachHangDTO> tkkh;
        private List<ThongKeNhaCungCapDTO> tkncc;
        private List<ThongKeTonKhoDTO> listTonKho;

        public ThongKeBUS()
        {
            // Lấy thống kê tồn kho từ đầu đến hiện tại
            listTonKho = thongkeDAO.GetThongKeTonKho("", DateTime.MinValue, DateTime.Now);
        }

        public List<ThongKeKhachHangDTO> GetAllKhachHang()
        {
            this.tkkh = thongkeDAO.GetThongKeKhachHang("", DateTime.MinValue, DateTime.Now);
            return this.tkkh;
        }

        public List<ThongKeKhachHangDTO> FilterKhachHang(string text, DateTime start, DateTime end)
        {
            this.tkkh = thongkeDAO.GetThongKeKhachHang(text, start, end);
            return this.tkkh;
        }

        public List<ThongKeNhaCungCapDTO> GetAllNCC()
        {
            this.tkncc = thongkeDAO.GetThongKeNCC("", DateTime.MinValue, DateTime.Now);
            return this.tkncc;
        }

        public List<ThongKeNhaCungCapDTO> FilterNCC(string text, DateTime start, DateTime end)
        {
            this.tkncc = thongkeDAO.GetThongKeNCC(text, start, end);
            return this.tkncc;
        }

        public List<ThongKeTonKhoDTO> GetTonKho()
        {
            return this.listTonKho;
        }

        public List<ThongKeTonKhoDTO> FilterTonKho(string text, DateTime timeStart, DateTime timeEnd)
        {
            List<ThongKeTonKhoDTO> result = thongkeDAO.GetThongKeTonKho(text, timeStart, timeEnd);
            return result;
        }

        public int[] GetSoluong(List<ThongKeTonKhoDTO> list)
        {
            int[] result = { 0, 0, 0, 0 };
            foreach (var item in list)
            {
                result[0] += item.Tondauky;
                result[1] += item.Nhaptrongky;
                result[2] += item.Xuattrongky;
                result[3] += item.Toncuoiky;
            }
            return result;
        }

        public List<ThongKeDoanhThuDTO> GetDoanhThuTheoTungNam(int yearStart, int yearEnd)
        {
            return this.thongkeDAO.GetDoanhThuTheoTungNam(yearStart, yearEnd);
        }

        public List<ThongKeTheoThangDTO> GetThongKeTheoThang(int nam)
        {
            return thongkeDAO.GetThongKeTheoThang(nam);
        }

        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTungNgayTrongThang(int thang, int nam)
        {
            return thongkeDAO.GetThongKeTungNgayTrongThang(thang, nam);
        }

        public List<ThongKeTungNgayTrongThangDTO> GetThongKeTuNgayDenNgay(string start, string end)
        {
            return thongkeDAO.GetThongKeTuNgayDenNgay(start, end);
        }

        public List<ThongKeTungNgayTrongThangDTO> GetThongKe7NgayGanNhat()
        {
            return thongkeDAO.GetThongKe7NgayGanNhat();
        }
    }
}