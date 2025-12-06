using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;

namespace src.GUI.ThongKe
{
    // KẾ THỪA FORM (Thay vì UserControl)
    public class ThongKe : Form 
    {
        private ThongKeBUS thongkeBUS;
        private TabControl tabbedPane;
        private Color backgroundColor = Color.FromArgb(240, 247, 250);

        // Các màn hình con (Vẫn là UserControl để nhúng vào Tab)
        private ThongKeTongQuan tongquan;
        private ThongKeTonKho nhapxuat;
        private ThongKeKhachHang khachhang;
        private ThongKeNhaCungCap nhacungcap;
        private ThongKeDoanhThu doanhthu;

        public ThongKe()
        {
            this.thongkeBUS = new ThongKeBUS();
            InitComponent();
        }

        private void InitComponent()
        {
            // Cấu hình Form để nhúng được vào Panel
            this.TopLevel = false; // QUAN TRỌNG: Để nhúng vào Form khác
            this.FormBorderStyle = FormBorderStyle.None; // Bỏ viền
            this.Dock = DockStyle.Fill; // Lấp đầy Panel cha
            this.BackColor = backgroundColor;

            // Khởi tạo các UserControl con
            tongquan = new ThongKeTongQuan(thongkeBUS);
            nhapxuat = new ThongKeTonKho(thongkeBUS);
            khachhang = new ThongKeKhachHang(thongkeBUS);
            nhacungcap = new ThongKeNhaCungCap(thongkeBUS);
            doanhthu = new ThongKeDoanhThu(thongkeBUS);

            // Khởi tạo TabControl
            tabbedPane = new TabControl();
            tabbedPane.Dock = DockStyle.Fill;
            
            // Thêm các Tab
            AddTab("Tổng quan", tongquan);
            AddTab("Tồn kho", nhapxuat);
            AddTab("Doanh thu", doanhthu);
            AddTab("Nhà cung cấp", nhacungcap);
            AddTab("Khách hàng", khachhang);

            this.Controls.Add(tabbedPane);
        }

        // Hàm hỗ trợ thêm Tab
        private void AddTab(string title, UserControl content)
        {
            TabPage page = new TabPage(title);
            page.BackColor = Color.White;
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            tabbedPane.TabPages.Add(page);
        }
    }
}