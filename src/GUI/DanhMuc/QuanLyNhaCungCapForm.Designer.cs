using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyNhaCungCapForm
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

            this.Text = "Quản lý Nhà cung cấp";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvNhaCungCap = new DataGridView();
            txtMaNCC = new TextBox();
            txtTenNCC = new TextBox();
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

            ((ISupportInitialize)(dgvNhaCungCap)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ NHÀ CUNG CẤP";

            // 
            // dgvNhaCungCap
            // 
            dgvNhaCungCap.AllowUserToAddRows = false;
            dgvNhaCungCap.AutoGenerateColumns = false;
            dgvNhaCungCap.BackgroundColor = Color.White;
            dgvNhaCungCap.BorderStyle = BorderStyle.None;
            dgvNhaCungCap.ColumnHeadersHeight = 40;
            dgvNhaCungCap.EnableHeadersVisualStyles = false;
            dgvNhaCungCap.Location = new Point(30, 140);
            dgvNhaCungCap.ReadOnly = true;
            dgvNhaCungCap.RowHeadersVisible = false;
            dgvNhaCungCap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhaCungCap.Size = new Size(850, 500);
            dgvNhaCungCap.SelectionChanged += DgvNhaCungCap_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(900, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 660);

            // 
            // QuanLyNhaCungCapForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvNhaCungCap);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvNhaCungCap)).EndInit();
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
            cboTimKiem.Size = new Size(180, 25);
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã nhà cung cấp", "Tên nhà cung cấp", "Địa chỉ", "Số điện thoại", "Email" });
            cboTimKiem.SelectedIndex = 0;

            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.Location = new Point(200, 12);
            txtTimKiem.Size = new Size(400, 25);

            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(610, 10);
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
            btnRefresh.Location = new Point(720, 10);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(39, 174, 96);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(830, 10);
            btnExport.Size = new Size(110, 30);
            btnExport.Text = "📥 Xuất Excel";
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            panel.Controls.Add(cboTimKiem);
            panel.Controls.Add(txtTimKiem);
            panel.Controls.Add(btnTimKiem);
            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnExport);

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
                Text = "THÔNG TIN NHÀ CUNG CẤP",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NCC
            Label lblMaNCC = new Label
            {
                Text = "Mã NCC:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtMaNCC.BackColor = Color.FromArgb(236, 240, 241);
            txtMaNCC.Location = new Point(140, yPos);
            txtMaNCC.ReadOnly = true;
            txtMaNCC.Size = new Size(280, 25);
            panel.Controls.Add(lblMaNCC);
            panel.Controls.Add(txtMaNCC);
            yPos += 40;

            // Tên NCC
            Label lblTenNCC = new Label
            {
                Text = "Tên NCC: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtTenNCC.Font = new Font("Segoe UI", 10F);
            txtTenNCC.Location = new Point(140, yPos);
            txtTenNCC.Size = new Size(280, 25);
            panel.Controls.Add(lblTenNCC);
            panel.Controls.Add(txtTenNCC);
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
            btnLuu.BackColor = Color.FromArgb(52, 152, 219);
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

        private DataGridView dgvNhaCungCap;
        private TextBox txtMaNCC;
        private TextBox txtTenNCC;
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

        #endregion
    }
}
