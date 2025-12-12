using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyNhaSanXuatForm
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

            this.Text = "Quản lý Nhà sản xuất";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvNhaSanXuat = new DataGridView();
            txtMaNSX = new TextBox();
            txtTenNSX = new TextBox();
            txtDiaChi = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            txtTimKiem = new TextBox();
            cboTimKiem = new ComboBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();
            btnExport = new Button();

            Label lblTitle = new Label();

            ((ISupportInitialize)(dgvNhaSanXuat)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ NHÀ SẢN XUẤT";

            // 
            // dgvNhaSanXuat
            // 
            dgvNhaSanXuat.AllowUserToAddRows = false;
            dgvNhaSanXuat.AutoGenerateColumns = false;
            dgvNhaSanXuat.BackgroundColor = Color.White;
            dgvNhaSanXuat.BorderStyle = BorderStyle.None;
            dgvNhaSanXuat.ColumnHeadersHeight = 40;
            dgvNhaSanXuat.EnableHeadersVisualStyles = false;
            dgvNhaSanXuat.Location = new Point(30, 140);
            dgvNhaSanXuat.ReadOnly = true;
            dgvNhaSanXuat.RowHeadersVisible = false;
            dgvNhaSanXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhaSanXuat.Size = new Size(850, 450);
            dgvNhaSanXuat.SelectionChanged += DgvNhaSanXuat_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(900, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 610);

            // 
            // QuanLyNhaSanXuatForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvNhaSanXuat);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvNhaSanXuat)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new System.Drawing.Size(1300, 50),
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
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã NSX", "Tên NSX", "Số điện thoại", "Email" });
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

            // --- Button Import (MỚI) ---
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

            // --- Button Export (Cập nhật vị trí) ---
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
                Size = new Size(450, 450),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN NHÀ SẢN XUẤT",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NSX
            Label lblMaNSX = new Label
            {
                Text = "Mã NSX:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtMaNSX.BackColor = Color.FromArgb(236, 240, 241);
            txtMaNSX.Location = new Point(140, yPos);
            txtMaNSX.ReadOnly = true;
            txtMaNSX.Size = new Size(280, 25);
            panel.Controls.Add(lblMaNSX);
            panel.Controls.Add(txtMaNSX);
            yPos += 40;

            // Tên NSX
            Label lblTenNSX = new Label
            {
                Text = "Tên NSX: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtTenNSX.Font = new Font("Segoe UI", 10F);
            txtTenNSX.Location = new Point(140, yPos);
            txtTenNSX.Size = new Size(280, 25);
            panel.Controls.Add(lblTenNSX);
            panel.Controls.Add(txtTenNSX);
            yPos += 40;

            // Địa chỉ
            Label lblDiaChi = new Label
            {
                Text = "Địa chỉ: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtDiaChi.Font = new Font("Segoe UI", 10F);
            txtDiaChi.Location = new Point(140, yPos);
            txtDiaChi.Size = new Size(280, 25);
            panel.Controls.Add(lblDiaChi);
            panel.Controls.Add(txtDiaChi);
            yPos += 40;

            // Số điện thoại
            Label lblSDT = new Label
            {
                Text = "Số điện thoại: *",
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
                Size = new Size(1300, 50),
                BackColor = Color.Transparent
            };

            int xPos = 0;
            int btnWidth = 100;
            int btnHeight = 35;
            int spacing = 15;

            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(39, 174, 96);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(xPos, 10);
            btnThem.Size = new Size(btnWidth, btnHeight);
            btnThem.Text = "➕ Thêm";
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            panel.Controls.Add(btnThem);
            xPos += btnWidth + spacing;

            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(241, 196, 15);
            btnSua.Cursor = Cursors.Hand;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(xPos, 10);
            btnSua.Size = new Size(btnWidth, btnHeight);
            btnSua.Text = "✏️ Sửa";
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            panel.Controls.Add(btnSua);
            xPos += btnWidth + spacing;

            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(231, 76, 60);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(xPos, 10);
            btnXoa.Size = new Size(btnWidth, btnHeight);
            btnXoa.Text = "🗑️ Xóa";
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            panel.Controls.Add(btnXoa);
            xPos += btnWidth + spacing;

            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(52, 152, 219);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(xPos, 10);
            btnLuu.Size = new Size(btnWidth, btnHeight);
            btnLuu.Text = "💾 Lưu";
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            panel.Controls.Add(btnLuu);
            xPos += btnWidth + spacing;

            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(149, 165, 166);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(xPos, 10);
            btnHuy.Size = new Size(btnWidth, btnHeight);
            btnHuy.Text = "❌ Hủy";
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;
            panel.Controls.Add(btnHuy);

            return panel;
        }

        #region Windows Form Designer generated code

        private DataGridView dgvNhaSanXuat;
        private TextBox txtMaNSX;
        private TextBox txtTenNSX;
        private TextBox txtDiaChi;
        private TextBox txtSDT;
        private TextBox txtEmail;
        private TextBox txtTimKiem;
        private ComboBox cboTimKiem;
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
