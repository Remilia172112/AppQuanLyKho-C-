using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyNhanVienForm
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

            this.Text = "Quản lý Nhân viên";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvNhanVien = new DataGridView();
            txtMaNV = new TextBox();
            txtHoTen = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            txtTimKiem = new TextBox();
            cboGioiTinh = new ComboBox();
            cboTimKiem = new ComboBox();
            dtpNgaySinh = new DateTimePicker();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();
            btnExport = new Button();

            Label lblTitle = new Label();

            ((ISupportInitialize)(dgvNhanVien)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ NHÂN VIÊN";

            // 
            // dgvNhanVien
            // 
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.BackgroundColor = Color.White;
            dgvNhanVien.BorderStyle = BorderStyle.None;
            dgvNhanVien.ColumnHeadersHeight = 40;
            dgvNhanVien.EnableHeadersVisualStyles = false;
            dgvNhanVien.Location = new Point(30, 140);
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.RowHeadersVisible = false;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.Size = new Size(850, 500);
            dgvNhanVien.SelectionChanged += DgvNhanVien_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(900, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 660);

            // 
            // QuanLyNhanVienForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvNhanVien);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvNhanVien)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new System.Drawing.Size(1320, 50),
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(10)
            };

            // ComboBox
            cboTimKiem = new ComboBox
            {
                Location = new System.Drawing.Point(10, 12),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Họ tên", "Email", "Số điện thoại" });
            cboTimKiem.SelectedIndex = 0;

            // TextBox Tìm kiếm
            txtTimKiem = new TextBox
            {
                Location = new System.Drawing.Point(170, 12),
                Size = new System.Drawing.Size(250, 25), // Size chuẩn
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };

            // Button Tìm kiếm
            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new System.Drawing.Point(430, 10),
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            // Button Làm mới
            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new System.Drawing.Point(530, 10),
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.FromArgb(149, 165, 166),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            btnImport = new Button
            {
                Text = "📥 Nhập Excel",
                Location = new System.Drawing.Point(630, 10),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113), // Xanh lá
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.Click += BtnImport_Click;

            btnExport = new Button
            {
                Text = "📤 Xuất Excel",
                Location = new System.Drawing.Point(740, 10),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(39, 174, 96), // Xanh đậm
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            panel.Controls.AddRange(new Control[] { cboTimKiem, txtTimKiem, btnTimKiem, btnRefresh, btnImport, btnExport });

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(450, 500),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN NHÂN VIÊN",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NV
            Label lblMaNV = new Label
            {
                Text = "Mã NV:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtMaNV.BackColor = Color.FromArgb(236, 240, 241);
            txtMaNV.Location = new Point(140, yPos);
            txtMaNV.ReadOnly = true;
            txtMaNV.Size = new Size(280, 25);
            panel.Controls.Add(lblMaNV);
            panel.Controls.Add(txtMaNV);
            yPos += 40;

            // Họ tên
            Label lblHoTen = new Label
            {
                Text = "Họ tên: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtHoTen.Font = new Font("Segoe UI", 10F);
            txtHoTen.Location = new Point(140, yPos);
            txtHoTen.Size = new Size(280, 25);
            panel.Controls.Add(lblHoTen);
            panel.Controls.Add(txtHoTen);
            yPos += 40;

            // Giới tính
            Label lblGioiTinh = new Label
            {
                Text = "Giới tính: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.Font = new Font("Segoe UI", 10F);
            cboGioiTinh.Location = new Point(140, yPos);
            cboGioiTinh.Size = new Size(280, 25);
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;
            panel.Controls.Add(lblGioiTinh);
            panel.Controls.Add(cboGioiTinh);
            yPos += 40;

            // Ngày sinh
            Label lblNgaySinh = new Label
            {
                Text = "Ngày sinh: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            dtpNgaySinh.Font = new Font("Segoe UI", 10F);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(140, yPos);
            dtpNgaySinh.MaxDate = DateTime.Now.AddYears(-18);
            dtpNgaySinh.Size = new Size(280, 25);
            dtpNgaySinh.Value = DateTime.Now.AddYears(-20);
            panel.Controls.Add(lblNgaySinh);
            panel.Controls.Add(dtpNgaySinh);
            yPos += 40;

            // Số điện thoại
            Label lblSDT = new Label
            {
                Text = "Số ĐT: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtSDT.Font = new Font("Segoe UI", 10F);
            txtSDT.Location = new Point(140, yPos);
            txtSDT.Size = new Size(280, 25);
            panel.Controls.Add(lblSDT);
            panel.Controls.Add(txtSDT);
            yPos += 40;

            // Email
            Label lblEmail = new Label
            {
                Text = "Email: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(140, yPos);
            txtEmail.Size = new Size(280, 25);
            panel.Controls.Add(lblEmail);
            panel.Controls.Add(txtEmail);

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

            panel.Controls.Add(btnThem);
            panel.Controls.Add(btnSua);
            panel.Controls.Add(btnXoa);
            panel.Controls.Add(btnLuu);
            panel.Controls.Add(btnHuy);

            return panel;
        }

        #region Windows Form Designer generated code

        private DataGridView dgvNhanVien;
        private TextBox txtMaNV;
        private TextBox txtHoTen;
        private TextBox txtSDT;
        private TextBox txtEmail;
        private TextBox txtTimKiem;
        private ComboBox cboGioiTinh;
        private ComboBox cboTimKiem;
        private DateTimePicker dtpNgaySinh;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnImport;

        #endregion
    }
}
