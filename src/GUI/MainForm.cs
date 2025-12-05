using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.GUI.Components;
using src.GUI.DanhMuc;
using src.GUI.NghiepVu;
using src.GUI.ThongKe;
using src.GUI.PhanQuyen;

namespace src.GUI.Auth
{
    public partial class MainForm : Form
    {
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel headerPanel;

        public MainForm()
        {
            InitializeComponent();
            LoadUserInfo();
            LoadDefaultContent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Size = new Size(1600, 900);
            this.Text = "Hệ thống Quản lý Kho hàng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1200, 600);
            this.WindowState = FormWindowState.Maximized;

            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label lblTitle = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ KHO HÀNG",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblTitle);

            Button btnLogout = new Button
            {
                Name = "btnLogout",
                Text = "Đăng xuất",
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            headerPanel.Controls.Add(btnLogout);

            Label lblUser = new Label
            {
                Name = "lblUser",
                Text = "Nhân viên: ...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            headerPanel.Controls.Add(lblUser);

            // Content Panel - Add đầu tiên để nó nằm dưới cùng
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 241),
                Padding = new Padding(0),
                AutoScroll = true
            };
            this.Controls.Add(contentPanel);

            // Sidebar Panel - Add thứ 2
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(44, 62, 80)
            };

            CreateMenuButtons();
            this.Controls.Add(sidebarPanel);

            // Header Panel - Add cuối cùng để nó nằm trên cùng
            // Position controls on right side
            this.Resize += (s, e) => PositionHeaderControls();
            PositionHeaderControls();
            this.Controls.Add(headerPanel);

            this.ResumeLayout(false);
        }

        private void PositionHeaderControls()
        {
            Button? btnLogout = headerPanel.Controls.Find("btnLogout", false).FirstOrDefault() as Button;
            Label? lblUser = headerPanel.Controls.Find("lblUser", false).FirstOrDefault() as Label;

            if (btnLogout != null && lblUser != null)
            {
                // Tính toán kích thước label trước
                using (Graphics g = Graphics.FromHwnd(lblUser.Handle))
                {
                    SizeF textSize = g.MeasureString(lblUser.Text, lblUser.Font);
                    lblUser.Size = new Size((int)Math.Ceiling(textSize.Width) + 5, (int)Math.Ceiling(textSize.Height));
                }
                
                // Đặt logout button cách mép phải headerPanel 20px
                int rightMargin = 20;
                btnLogout.Location = new Point(headerPanel.Width - btnLogout.Width - rightMargin, 15);
                
                // Đặt label user bên trái logout button, cách 15px
                lblUser.Location = new Point(btnLogout.Left - lblUser.Width - 15, 22);
            }
        }

        private void CreateMenuButtons()
        {
            int y = 20;
            int buttonHeight = 45;
            int spacing = 5;

            // Dashboard
            AddMenuButton("Dashboard", y, () => LoadDashboard());
            y += buttonHeight + spacing;

            // Quản lý Sản phẩm
            if (SessionManager.CanView("sanpham"))
            {
                AddMenuButton("Quản lý Sản phẩm", y, () => OpenForm(new QuanLySanPhamForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Khách hàng
            if (SessionManager.CanView("khachhang"))
            {
                AddMenuButton("Quản lý Khách hàng", y, () => OpenForm(new QuanLyKhachHangForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Nhà cung cấp
            if (SessionManager.CanView("nhacungcap"))
            {
                AddMenuButton("Quản lý Nhà cung cấp", y, () => OpenForm(new QuanLyNhaCungCapForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Nhà sản xuất
            if (SessionManager.CanView("nhasanxuat"))
            {
                AddMenuButton("Quản lý Nhà sản xuất", y, () => OpenForm(new QuanLyNhaSanXuatForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Khu vực kho
            if (SessionManager.CanView("khuvuckho"))
            {
                AddMenuButton("Quản lý Khu vực kho", y, () => OpenForm(new QuanLyKhuVucKhoForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Nhân viên
            if (SessionManager.CanView("nhanvien"))
            {
                AddMenuButton("Quản lý Nhân viên", y, () => OpenForm(new QuanLyNhanVienForm()));
                y += buttonHeight + spacing;
            }

            // Quản lý Tài khoản
            if (SessionManager.CanView("taikhoan"))
            {
                AddMenuButton("Quản lý Tài khoản", y, () => OpenForm(new QuanLyTaiKhoanForm()));
                y += buttonHeight + spacing;
            }

            // Phiếu nhập
            if (SessionManager.CanView("nhaphang"))
            {
                AddMenuButton("Phiếu Nhập hàng", y, () => OpenForm(new PhieuNhapForm()));
                y += buttonHeight + spacing;
            }

            // Phiếu xuất
            if (SessionManager.CanView("xuathang"))
            {
                AddMenuButton("Phiếu Xuất hàng", y, () => OpenForm(new PhieuXuatForm()));
                y += buttonHeight + spacing;
            }

            // Phiếu kiểm kê
            if (SessionManager.CanView("kiemke"))
            {
                AddMenuButton("Phiếu Kiểm kê", y, () => OpenForm(new PhieuKiemKeForm()));
                y += buttonHeight + spacing;
            }

            // Thống kê
            if (SessionManager.CanView("thongke"))
            {
                AddMenuButton("Thống kê & Báo cáo", y, () => OpenForm(new ThongKeForm()));
                y += buttonHeight + spacing;
            }

            // Phân quyền
            if (SessionManager.CanView("nhomquyen"))
            {
                AddMenuButton("Quản lý Phân quyền", y, () => OpenForm(new QuanLyNhomQuyenForm()));
                y += buttonHeight + spacing;
            }
        }

        private void AddMenuButton(string text, int y, Action onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(10, y),
                Size = new Size(230, 45),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();
            
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(41, 128, 185);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(52, 73, 94);

            sidebarPanel.Controls.Add(btn);
        }

        private void LoadUserInfo()
        {
            if (SessionManager.IsLoggedIn && SessionManager.CurrentEmployee != null)
            {
                Label? lblUser = headerPanel.Controls.Find("lblUser", false)[0] as Label;
                if (lblUser != null)
                {
                    lblUser.Text = $"Nhân viên: {SessionManager.CurrentEmployee.HOTEN} | " +
                                   $"Quyền: {SessionManager.CurrentRole?.Tennhomquyen ?? "N/A"}";
                }
            }
        }

        private void LoadDefaultContent()
        {
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            contentPanel.Controls.Clear();

            Label lblWelcome = new Label
            {
                Text = $"Chào mừng, {SessionManager.CurrentEmployee?.HOTEN}!",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(30, 30),
                AutoSize = true
            };
            contentPanel.Controls.Add(lblWelcome);

            Label lblInfo = new Label
            {
                Text = "Hệ thống quản lý kho hàng\nPhiên bản 1.0",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(30, 80),
                AutoSize = true
            };
            contentPanel.Controls.Add(lblInfo);
        }

        private void OpenForm(Form childForm)
        {
            contentPanel.Controls.Clear();
            
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Location = new Point(0, 0);
            childForm.Size = contentPanel.ClientSize;
            childForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            
            contentPanel.Controls.Add(childForm);
            childForm.Show();
            
            // Thêm event để resize child form khi contentPanel thay đổi kích thước
            contentPanel.Resize += (s, e) => 
            {
                if (contentPanel.Controls.Count > 0 && contentPanel.Controls[0] is Form form)
                {
                    form.Size = contentPanel.ClientSize;
                }
            };
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionManager.Logout();
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}
