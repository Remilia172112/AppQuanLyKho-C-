using src.GUI.Components;
using src.GUI.DanhMuc;
using src.GUI.NghiepVu;
using src.GUI.PhanQuyen;
using System.Drawing.Drawing2D;
using System.IO;
using Svg; // <--- QUAN TRỌNG: Thêm thư viện này

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
            SetupAppIcon();
            LoadUserInfo();
            LoadDefaultContent();
        }

        private void SetupAppIcon()
        {
            try
            {
                string path = FindIconFile("app.ico");
                if (!string.IsNullOrEmpty(path))
                {
                    this.Icon = new System.Drawing.Icon(path);
                }
            }
            catch { }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Size = new Size(1600, 1000);
            this.Text = "Hệ thống Quản lý Kho hàng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1200, 600);
            this.WindowState = FormWindowState.Normal;

            // Header
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

            // Content
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 241),
                Padding = new Padding(0),
                AutoScroll = true
            };
            this.Controls.Add(contentPanel);

            // Sidebar
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(44, 62, 80)
            };

            CreateMenuButtons();
            this.Controls.Add(sidebarPanel);

            this.Resize += (s, e) => PositionHeaderControls();
            this.Load += (s, e) => PositionHeaderControls();
            this.Shown += (s, e) => PositionHeaderControls();
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
                using (Graphics g = Graphics.FromHwnd(lblUser.Handle))
                {
                    SizeF textSize = g.MeasureString(lblUser.Text, lblUser.Font);
                    lblUser.Size = new Size((int)Math.Ceiling(textSize.Width) + 5, (int)Math.Ceiling(textSize.Height));
                }

                int rightMargin = 20;
                btnLogout.Location = new Point(headerPanel.Width - btnLogout.Width - rightMargin, 15);
                lblUser.Location = new Point(btnLogout.Left - lblUser.Width - 15, 22);
            }
        }

        private void CreateMenuButtons()
        {
            int y = 20;
            int buttonHeight = 45;
            int spacing = 5;

            // Dashboard
            AddMenuButton("Trang chủ", "home.svg", y, () => LoadDashboard());
            y += buttonHeight + spacing;

            if (SessionManager.CanView("sanpham"))
            {
                AddMenuButton("Sản phẩm", "book.svg", y, () => OpenForm(new QuanLySanPhamForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("loaisanpham"))
            {
                AddMenuButton("Loại sản phẩm", "book.svg", y, () => OpenForm(new LoaiSanPhamForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("khachhang"))
            {
                AddMenuButton("Khách hàng", "customer.svg", y, () => OpenForm(new QuanLyKhachHangForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("nhacungcap"))
            {
                AddMenuButton("Nhà cung cấp", "supplier.svg", y, () => OpenForm(new QuanLyNhaCungCapForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("nhasanxuat"))
            {
                AddMenuButton("Nhà sản xuất", "nhaxb.svg", y, () => OpenForm(new QuanLyNhaSanXuatForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("khuvuckho"))
            {
                AddMenuButton("Khu vực kho", "khu_vuc.svg", y, () => OpenForm(new QuanLyKhuVucKhoForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("nhanvien"))
            {
                AddMenuButton("Nhân viên", "staff_1.svg", y, () => OpenForm(new QuanLyNhanVienForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("taikhoan"))
            {
                AddMenuButton("Tài khoản", "account.svg", y, () => OpenForm(new QuanLyTaiKhoanForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("nhaphang"))
            {
                AddMenuButton("Phiếu nhập", "import.svg", y, () => OpenForm(new PhieuNhapForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("xuathang"))
            {
                AddMenuButton("Phiếu xuất", "export.svg", y, () => OpenForm(new PhieuXuatForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("kiemke"))
            {
                AddMenuButton("Phiếu kiểm kê", "inventory.svg", y, () => OpenForm(new PhieuKiemKeForm()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("thongke"))
            {
                AddMenuButton("Thống kê", "statistical_1.svg", y, () => OpenForm(new ThongKe.ThongKe()));
                y += buttonHeight + spacing;
            }

            if (SessionManager.CanView("nhomquyen"))
            {
                AddMenuButton("Phân quyền", "protect.svg", y, () => OpenForm(new QuanLyNhomQuyenForm()));
                y += buttonHeight + spacing;
            }
        }

        private void AddMenuButton(string text, string iconName, int y, Action onClick)
        {
            Button btn = new Button
            {
                Text = "  " + text,
                Location = new Point(10, y),
                Size = new Size(230, 45),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            // XỬ LÝ LOAD ICON
            if (!string.IsNullOrEmpty(iconName))
            {
                btn.Image = LoadIconFromSrc(iconName, 24, 24);
            }

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(41, 128, 185);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(52, 73, 94);

            sidebarPanel.Controls.Add(btn);
        }

        // --- TÌM FILE ICON (Hỗ trợ cả trong src/icon và bin) ---
        private string FindIconFile(string fileName)
        {
            // 1. Tìm trong thư mục output (bin/Debug/...)
            string pathInBin = Path.Combine(Application.StartupPath, "icon", fileName);
            if (File.Exists(pathInBin)) return pathInBin;

            // 2. Tìm trong thư mục src/icon (Khi chạy Debug từ VS)
            try
            {
                string projectRoot = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\.."));
                string pathInSrc = Path.Combine(projectRoot, "src", "icon", fileName);

                if (File.Exists(pathInSrc)) return pathInSrc;
            }
            catch { }

            return null; // Không tìm thấy
        }

        // --- HÀM LOAD ICON ĐÃ SỬA ĐỂ HỖ TRỢ SVG ---
        private Image? LoadIconFromSrc(string fileName, int w, int h)
        {
            try
            {
                string path = FindIconFile(fileName);

                if (!string.IsNullOrEmpty(path))
                {
                    // Nếu là file .svg -> Dùng thư viện Svg để render
                    if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                    {
                        var svgDocument = SvgDocument.Open(path);
                        if (svgDocument != null)
                        {
                            return svgDocument.Draw(w, h); // Render ra Bitmap
                        }
                    }
                    else
                    {
                        // Nếu là file ảnh thường (png, jpg, ico)
                        using (Image original = Image.FromFile(path))
                        {
                            Bitmap resized = new Bitmap(w, h);
                            using (Graphics g = Graphics.FromImage(resized))
                            {
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.DrawImage(original, 0, 0, w, h);
                            }
                            return resized;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In lỗi ra console để debug nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi load icon {fileName}: {ex.Message}");
            }
            return null;
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
            OpenForm(new DashboardForm());
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
                LoginPage loginForm = new LoginPage();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}