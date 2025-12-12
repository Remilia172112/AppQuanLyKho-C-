using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class LoaiSanPhamForm
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
            this.SuspendLayout();
            
            this.Text = "Quản lý Loại Sản phẩm";
            this.Size = new Size(1200, 750); // Nhỏ hơn form SP chút vì ít cột hơn
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Init Controls
            dgvLoaiSanPham = new DataGridView();
            txtMaLSP = new TextBox();
            txtTenLSP = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();

            Label lblTitle = new Label();
            ((ISupportInitialize)(dgvLoaiSanPham)).BeginInit();

            // Title
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ LOẠI SẢN PHẨM";

            // GridView
            dgvLoaiSanPham.AllowUserToAddRows = false;
            dgvLoaiSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLoaiSanPham.BackgroundColor = Color.White;
            dgvLoaiSanPham.Location = new Point(30, 140);
            dgvLoaiSanPham.ReadOnly = true;
            dgvLoaiSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoaiSanPham.Size = new Size(700, 550); // Rộng hơn chút
            dgvLoaiSanPham.SelectionChanged += DgvLoaiSanPham_SelectionChanged;

            // Panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(750, 140); // Đặt bên phải Grid

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 710);

            // Add Controls
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvLoaiSanPham);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvLoaiSanPham)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1000, 50), // Tăng chiều rộng panel để chứa đủ nút
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // Text Tim Kiem
            txtTimKiem.Location = new Point(10, 12);
            txtTimKiem.Size = new Size(250, 25);
            txtTimKiem.PlaceholderText = "Nhập tên loại sản phẩm...";

            // Btn Tim Kiem
            btnTimKiem.BackColor = Color.FromArgb(41, 128, 185);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(270, 10);
            btnTimKiem.Size = new Size(90, 30);
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            // Btn Refresh
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(370, 10);
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.Text = "Làm mới";
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            // --- THÊM MỚI ---
            
            // Btn Import (Nhập Excel)
            btnImport = new Button();
            btnImport.BackColor = Color.FromArgb(46, 204, 113); // Màu xanh lá
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.ForeColor = Color.White;
            btnImport.Location = new Point(470, 10);
            btnImport.Size = new Size(100, 30);
            btnImport.Text = "📥 Nhập Excel";
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.Click += BtnImport_Click;

            // Btn Export (Xuất Excel)
            btnExport = new Button();
            btnExport.BackColor = Color.FromArgb(39, 174, 96); // Màu xanh đậm hơn
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(580, 10);
            btnExport.Size = new Size(100, 30);
            btnExport.Text = "📤 Xuất Excel";
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            panel.Controls.Add(txtTimKiem);
            panel.Controls.Add(btnTimKiem);
            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnImport); // Add Import
            panel.Controls.Add(btnExport); // Add Export

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(400, 550),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            int y = 20;

            // Mã LSP
            Label lblMa = new Label { Text = "Mã Loại:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtMaLSP.Location = new Point(120, y);
            txtMaLSP.Size = new Size(250, 25);
            txtMaLSP.ReadOnly = true;
            txtMaLSP.BackColor = SystemColors.Control;
            panel.Controls.Add(lblMa);
            panel.Controls.Add(txtMaLSP);
            y += 40;

            // Tên LSP
            Label lblTen = new Label { Text = "Tên Loại: *", Location = new Point(15, y), Size = new Size(100, 25) };
            txtTenLSP.Location = new Point(120, y);
            txtTenLSP.Size = new Size(250, 25);
            panel.Controls.Add(lblTen);
            panel.Controls.Add(txtTenLSP);
            y += 40;

            // Ghi chú
            Label lblGhiChu = new Label { Text = "Ghi chú:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtGhiChu.Location = new Point(120, y);
            txtGhiChu.Size = new Size(250, 100);
            txtGhiChu.Multiline = true;
            panel.Controls.Add(lblGhiChu);
            panel.Controls.Add(txtGhiChu);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(700, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            int x = 10;
            // Them
            btnThem = CreateButton("➕ Thêm", x, Color.FromArgb(46, 204, 113), BtnThem_Click);
            panel.Controls.Add(btnThem);
            x += 110;

            // Sua
            btnSua = CreateButton("✏️ Sửa", x, Color.FromArgb(52, 152, 219), BtnSua_Click);
            panel.Controls.Add(btnSua);
            x += 110;

            // Xoa
            btnXoa = CreateButton("🗑️ Xóa", x, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            panel.Controls.Add(btnXoa);
            x += 110;

            // Luu
            btnLuu = CreateButton("💾 Lưu", x, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            panel.Controls.Add(btnLuu);
            x += 110;

            // Huy
            btnHuy = CreateButton("❌ Hủy", x, Color.FromArgb(149, 165, 166), BtnHuy_Click);
            panel.Controls.Add(btnHuy);

            return panel;
        }

        private Button CreateButton(string text, int x, Color color, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 10),
                Size = new Size(100, 30),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += clickHandler;
            return btn;
        }

        #region Components
        private DataGridView dgvLoaiSanPham;
        private TextBox txtMaLSP;
        private TextBox txtTenLSP;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnImport;
        private Button btnExport;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;
        #endregion
    }
}