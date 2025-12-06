using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;

namespace src.GUI.ThongKe
{
    public class ThongKeDoanhThu : UserControl
    {
        private ThongKeBUS thongkeBUS;
        private TabControl tabbedPane;
        private Color backgroundColor = Color.FromArgb(240, 247, 250);

        // Các màn hình con
        private ThongKeDoanhThuTrongThang thongketrongthang;
        private ThongKeDoanhThuTungNam thongketungnam;
        private ThongKeDoanhThuTungThang thongkedoanhthutungthang;
        private ThongKeDoanhThuTuNgayDenNgay thongkedoanhthutungaydenngay;

        public ThongKeDoanhThu(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            InitComponent();
        }

        private void InitComponent()
        {
            // Cấu hình Panel chính
            this.Dock = DockStyle.Fill;
            this.BackColor = backgroundColor;

            // Khởi tạo TabControl
            tabbedPane = new TabControl();
            tabbedPane.Dock = DockStyle.Fill;
            
            // Khởi tạo các Panel con
            // Lưu ý: Các class này phải tồn tại (xem phần code phụ bên dưới)
            thongketungnam = new ThongKeDoanhThuTungNam(thongkeBUS);
            thongkedoanhthutungthang = new ThongKeDoanhThuTungThang(thongkeBUS);
            thongketrongthang = new ThongKeDoanhThuTrongThang(thongkeBUS); // Class này bạn đã làm ở bước trước
            thongkedoanhthutungaydenngay = new ThongKeDoanhThuTuNgayDenNgay(thongkeBUS);

            // Thêm Tab 1: Thống kê theo năm
            AddTab("Thống kê theo năm", thongketungnam);

            // Thêm Tab 2: Thống kê từng tháng trong năm
            AddTab("Thống kê từng tháng trong năm", thongkedoanhthutungthang);

            // Thêm Tab 3: Thống kê từng ngày trong tháng
            AddTab("Thống kê từng ngày trong tháng", thongketrongthang);

            // Thêm Tab 4: Thống kê từ ngày đến ngày
            AddTab("Thống kê từ ngày đến ngày", thongkedoanhthutungaydenngay);

            this.Controls.Add(tabbedPane);
        }

        // Hàm hỗ trợ thêm Tab cho gọn code
        private void AddTab(string title, UserControl content)
        {
            TabPage page = new TabPage(title);
            page.BackColor = Color.White; // Màu nền tab con
            content.Dock = DockStyle.Fill; // Nội dung lấp đầy tab
            page.Controls.Add(content);
            tabbedPane.TabPages.Add(page);
        }
    }
}