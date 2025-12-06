using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyTaiKhoanForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            this.Text = "Quản lý Tài khoản";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvTaiKhoan = new DataGridView();
            cboMaNV = new ComboBox();
            cboNhomQuyen = new ComboBox();
            cboTimKiem = new ComboBox();
            txtTenNV = new TextBox();
            txtTenDangNhap = new TextBox();
            txtMatKhau = new TextBox();
            txtXacNhanMK = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();
            btnResetMK = new Button();

            Label lblTitle = new Label();

            ((ISupportInitialize)(dgvTaiKhoan)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";

            // 
            // dgvTaiKhoan
            // 
            dgvTaiKhoan.AllowUserToAddRows = false;
            dgvTaiKhoan.AutoGenerateColumns = false;
            dgvTaiKhoan.BackgroundColor = Color.White;
            dgvTaiKhoan.BorderStyle = BorderStyle.None;
            dgvTaiKhoan.ColumnHeadersHeight = 40;
            dgvTaiKhoan.EnableHeadersVisualStyles = false;
            dgvTaiKhoan.Location = new Point(30, 140);
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.RowHeadersVisible = false;
            dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiKhoan.Size = new Size(750, 500);
            dgvTaiKhoan.SelectionChanged += DgvTaiKhoan_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 660);

            // 
            // QuanLyTaiKhoanForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvTaiKhoan);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvTaiKhoan)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1320, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // 
            // cboTimKiem
            // 
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(10, 12);
            cboTimKiem.Size = new Size(150, 25);
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã nhân viên", "Username" });
            cboTimKiem.SelectedIndex = 0;

            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.Location = new Point(170, 12);
            txtTimKiem.Size = new Size(400, 25);

            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(580, 10);
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Text = "🔍 Tìm kiếm";
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(149, 165, 166);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(690, 10);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            panel.Controls.Add(cboTimKiem);
            panel.Controls.Add(txtTimKiem);
            panel.Controls.Add(btnTimKiem);
            panel.Controls.Add(btnRefresh);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(550, 500),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN TÀI KHOẢN",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Nhân viên
            Label lblMaNV = new Label
            {
                Text = "Nhân viên: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            cboMaNV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaNV.Font = new Font("Segoe UI", 10F);
            cboMaNV.Location = new Point(180, yPos);
            cboMaNV.Size = new Size(330, 25);
            cboMaNV.SelectedIndexChanged += CboMaNV_SelectedIndexChanged;
            panel.Controls.Add(lblMaNV);
            panel.Controls.Add(cboMaNV);
            yPos += 40;

            // Tên nhân viên
            Label lblTenNV = new Label
            {
                Text = "Tên nhân viên:",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtTenNV.BackColor = Color.FromArgb(236, 240, 241);
            txtTenNV.Font = new Font("Segoe UI", 10F);
            txtTenNV.Location = new Point(180, yPos);
            txtTenNV.ReadOnly = true;
            txtTenNV.Size = new Size(330, 25);
            panel.Controls.Add(lblTenNV);
            panel.Controls.Add(txtTenNV);
            yPos += 40;

            // Tên đăng nhập
            Label lblTenDangNhap = new Label
            {
                Text = "Tên đăng nhập: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtTenDangNhap.Font = new Font("Segoe UI", 10F);
            txtTenDangNhap.Location = new Point(180, yPos);
            txtTenDangNhap.Size = new Size(330, 25);
            panel.Controls.Add(lblTenDangNhap);
            panel.Controls.Add(txtTenDangNhap);
            yPos += 40;

            // Mật khẩu
            Label lblMatKhau = new Label
            {
                Text = "Mật khẩu: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtMatKhau.Font = new Font("Segoe UI", 10F);
            txtMatKhau.Location = new Point(180, yPos);
            txtMatKhau.PasswordChar = '●';
            txtMatKhau.Size = new Size(330, 25);
            panel.Controls.Add(lblMatKhau);
            panel.Controls.Add(txtMatKhau);
            yPos += 40;

            // Xác nhận mật khẩu
            Label lblXacNhanMK = new Label
            {
                Text = "Xác nhận MK: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtXacNhanMK.Font = new Font("Segoe UI", 10F);
            txtXacNhanMK.Location = new Point(180, yPos);
            txtXacNhanMK.PasswordChar = '●';
            txtXacNhanMK.Size = new Size(330, 25);
            panel.Controls.Add(lblXacNhanMK);
            panel.Controls.Add(txtXacNhanMK);
            yPos += 40;

            // Nhóm quyền
            Label lblNhomQuyen = new Label
            {
                Text = "Nhóm quyền: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            cboNhomQuyen.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhomQuyen.Font = new Font("Segoe UI", 10F);
            cboNhomQuyen.Location = new Point(180, yPos);
            cboNhomQuyen.Size = new Size(330, 25);
            panel.Controls.Add(lblNhomQuyen);
            panel.Controls.Add(cboNhomQuyen);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1320, 60),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(46, 204, 113);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(20, 15);
            btnThem.Size = new Size(100, 35);
            btnThem.Text = "➕ Thêm";
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;

            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(52, 152, 219);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(130, 15);
            btnSua.Size = new Size(100, 35);
            btnSua.Text = "✏️ Sửa";
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;

            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(231, 76, 60);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(240, 15);
            btnXoa.Size = new Size(100, 35);
            btnXoa.Text = "🗑️ Xóa";
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;

            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(155, 89, 182);
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(350, 15);
            btnLuu.Size = new Size(100, 35);
            btnLuu.Text = "💾 Lưu";
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;

            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(149, 165, 166);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(460, 15);
            btnHuy.Size = new Size(100, 35);
            btnHuy.Text = "❌ Hủy";
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;

            // 
            // btnResetMK
            // 
            btnResetMK.BackColor = Color.FromArgb(230, 126, 34);
            btnResetMK.FlatStyle = FlatStyle.Flat;
            btnResetMK.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnResetMK.ForeColor = Color.White;
            btnResetMK.Location = new Point(570, 15);
            btnResetMK.Size = new Size(120, 35);
            btnResetMK.Text = "🔑 Reset MK";
            btnResetMK.FlatAppearance.BorderSize = 0;
            btnResetMK.Click += BtnResetMK_Click;

            panel.Controls.Add(btnThem);
            panel.Controls.Add(btnSua);
            panel.Controls.Add(btnXoa);
            panel.Controls.Add(btnLuu);
            panel.Controls.Add(btnHuy);
            panel.Controls.Add(btnResetMK);

            return panel;
        }

        #region Windows Form Designer generated code

        private DataGridView dgvTaiKhoan;
        private ComboBox cboMaNV;
        private ComboBox cboNhomQuyen;
        private ComboBox cboTimKiem;
        private TextBox txtTenNV;
        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private TextBox txtXacNhanMK;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;
        private Button btnResetMK;

        #endregion
    }
}
