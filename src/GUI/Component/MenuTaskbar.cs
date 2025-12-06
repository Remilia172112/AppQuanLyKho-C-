using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI;
using src.GUI.Panel;
using Svg; // Cần thư viện Svg

namespace src.GUI.Component
{
    public class MenuTaskbar : UserControl
    {
        private Main mainForm;
        private TaiKhoanDTO user;
        private NhanVienDTO nhanVienDTO;
        private NhomQuyenDTO nhomQuyenDTO;
        
        // BUS
        private NhanVienBUS nvBUS = new NhanVienBUS();
        private TaiKhoanBUS tkBUS = new TaiKhoanBUS();
        private NhomQuyenBUS nqBUS = new NhomQuyenBUS();

        // UI Components
        private System.Windows.Forms.Panel pnlTop, pnlBottom;
        private FlowLayoutPanel pnlCenter; // Thay cho JScrollPane
        private List<ItemTaskbar> listItems = new List<ItemTaskbar>();

        // Cấu hình Menu (Tên hiển thị, Tên icon, Mã quyền)
        private string[,] menuData = {
            {"Trang chủ", "home.svg", "trangchu"},
            {"Sản phẩm", "book.svg", "sanpham"},
            {"Khu vực kho", "khu_vuc.svg", "khuvucsach"},
            {"Nhân viên", "staff_1.svg", "nhanvien"},
            {"Khách hàng", "customer.svg", "khachhang"},
            {"Thống kê", "statistical_1.svg", "thongke"},
            {"Nhà cung cấp", "supplier.svg", "nhacungcap"},
            {"Nhà sản xuất", "nhaxb.svg", "nhaxuatban"},
            {"Phiếu nhập", "import.svg", "nhaphang"},
            {"Phiếu xuất", "export.svg", "xuathang"},
            {"Phiếu kiểm kê", "inventory.svg", "kiemke"},
            {"Phân quyền", "protect.svg", "nhomquyen"},
            {"Tài khoản", "account.svg", "taikhoan"},
            {"Đăng xuất", "log_out.svg", "dangxuat"}
        };

        // Constructor 1: Chỉ có Main (test)
        public MenuTaskbar(Main main)
        {
            this.mainForm = main;
            InitComponent();
        }

        // Constructor 2: Có User đăng nhập
        public MenuTaskbar(Main main, TaiKhoanDTO user)
        {
            this.mainForm = main;
            this.user = user;
            
            // Lấy thông tin chi tiết
            this.nhanVienDTO = nvBUS.GetByIndex(nvBUS.GetIndexById(user.MNV));
            this.nhomQuyenDTO = tkBUS.GetNhomQuyenDTO(user.MNQ);
            
            InitComponent();
        }

        private void InitComponent()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Left;
            this.Width = 250; // Chiều rộng cố định Menu

            // 1. Panel Top (User Info) - Giữ nguyên
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.Height = 80;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.BackColor = Color.White;
            pnlTop.Paint += (s, e) => {
                e.Graphics.DrawLine(new Pen(Color.LightGray), 0, 79, 250, 79);
            };
            InitUserInfo(pnlTop);
            this.Controls.Add(pnlTop);

            // 2. Panel Bottom (Đăng xuất) - Giữ nguyên
            pnlBottom = new System.Windows.Forms.Panel();
            pnlBottom.Height = 50;
            pnlBottom.Dock = DockStyle.Bottom;
            this.Controls.Add(pnlBottom);

            // 3. Panel Center (Menu Items) - SỬA LẠI CHO ĐẸP
            pnlCenter = new FlowLayoutPanel();
            pnlCenter.Dock = DockStyle.Fill;
            
            // QUAN TRỌNG: Xếp theo chiều dọc để thẳng hàng
            pnlCenter.FlowDirection = FlowDirection.TopDown; 
            pnlCenter.WrapContents = false; // Không cho xuống dòng ngang
            pnlCenter.AutoScroll = true; 
            pnlCenter.BackColor = Color.White;
            
            // Căn lề: item cách lề trái 10px, trên 10px
            pnlCenter.Padding = new Padding(10, 10, 0, 0); 
            
            this.Controls.Add(pnlCenter);
            this.Controls.SetChildIndex(pnlCenter, 0); 

            GenerateMenu();
        }

        private void InitUserInfo(System.Windows.Forms.Panel pnl)
        {
            if (nhanVienDTO == null) return;

            // Icon Avatar
            PictureBox pbAvatar = new PictureBox();
            pbAvatar.Size = new Size(50, 50);
            pbAvatar.Location = new Point(10, 15);
            pbAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            
            string iconName = (nhanVienDTO.GIOITINH == 1) ? "man_50px.svg" : "women_50px.svg";
            try 
            { 
                string path = $"img/icon/{iconName}";
                if (System.IO.File.Exists(path))
                {
                    var svgDoc = SvgDocument.Open(path);
                    pbAvatar.Image = svgDoc.Draw(50, 50);
                }
            } 
            catch {}
            
            pnl.Controls.Add(pbAvatar);

            // Label Tên
            Label lblName = new Label();
            lblName.Text = nhanVienDTO.HOTEN;
            lblName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblName.Location = new Point(70, 20);
            lblName.AutoSize = true;
            pnl.Controls.Add(lblName);

            // Label Quyền
            Label lblRole = new Label();
            lblRole.Text = nhomQuyenDTO != null ? nhomQuyenDTO.Tennhomquyen : "Chưa phân quyền";
            lblRole.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblRole.ForeColor = Color.Gray;
            lblRole.Location = new Point(70, 40);
            lblRole.AutoSize = true;
            pnl.Controls.Add(lblRole);

            // Sự kiện click avatar -> MyAccount (Tính sau)
            pbAvatar.Cursor = Cursors.Hand;
            pbAvatar.Click += (s, e) => { MessageBox.Show("Tính năng thông tin tài khoản đang phát triển."); };
        }

        private void GenerateMenu()
        {
            int rowCount = menuData.GetLength(0);

            for (int i = 0; i < rowCount; i++)
            {
                string name = menuData[i, 0];
                string icon = menuData[i, 1];
                string permissionCode = menuData[i, 2];

                // Item cuối cùng (Đăng xuất) bỏ vào Bottom
                if (i == rowCount - 1)
                {
                    ItemTaskbar item = new ItemTaskbar(icon, name);
                    item.Dock = DockStyle.Fill;
                    item.Click += (s, e) => Logout();
                    pnlBottom.Controls.Add(item);
                    continue;
                }

                // Kiểm tra quyền (Bỏ qua trang chủ - luôn hiện)
                if (i != 0 && user != null)
                {
                    // Hàm CheckPermission trong NhomQuyenBUS
                    if (!nqBUS.CheckPermission(user.MNQ, permissionCode, "view"))
                    {
                        continue; // Nếu không có quyền view thì không hiện menu
                    }
                }

                // Tạo Item và thêm vào Center
                ItemTaskbar menuItem = new ItemTaskbar(icon, name);
                
                // Sự kiện Click chuyển trang
                // Dùng biến cục bộ để capture trong lambda
                int index = i; 
                menuItem.Click += (s, e) => 
                {
                    ResetSelection();
                    menuItem.SetSelected(true);
                    SwitchPage(permissionCode);
                };

                listItems.Add(menuItem);
                pnlCenter.Controls.Add(menuItem);
            }

            // Mặc định chọn trang chủ
            if (listItems.Count > 0)
            {
                listItems[0].SetSelected(true);
            }
        }

        private void ResetSelection()
        {
            foreach (var item in listItems)
            {
                item.SetSelected(false);
            }
        }

        private void Logout()
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                mainForm.Hide();
                LoginPage login = new LoginPage();
                login.ShowDialog();
                mainForm.Close();
            }
        }

        // Hàm điều hướng (Thay thế switch case dài dòng)
        private void SwitchPage(string permissionCode)
        {
            UserControl content = null;

            switch (permissionCode)
            {
                case "trangchu":
                    content = new TrangChu();
                    break;
                case "sanpham":
                    // content = new SanPham(mainForm); // Uncomment khi tạo file SanPham.cs
                    MessageBox.Show("Chuyển đến Sản Phẩm");
                    break;
                case "khuvucsach":
                    // content = new KhuVucKho(mainForm);
                    MessageBox.Show("Chuyển đến Khu Vực Kho");
                    break;
                case "nhanvien":
                    // content = new NhanVien(mainForm);
                    MessageBox.Show("Chuyển đến Nhân Viên");
                    break;
                case "khachhang":
                    // content = new KhachHang(mainForm);
                    MessageBox.Show("Chuyển đến Khách Hàng");
                    break;
                case "taikhoan":
                    // content = new TaiKhoan(mainForm);
                    MessageBox.Show("Chuyển đến Tài Khoản");
                    break;
                // ... Thêm các case khác tương tự ...
                default:
                    MessageBox.Show("Chức năng đang phát triển: " + permissionCode);
                    break;
            }

            if (content != null)
            {
                mainForm.SetPanel(content);
            }
        }
    }
}