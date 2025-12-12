using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyKhuVucKhoForm
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

            this.Text = "Quản lý Khu vực kho";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // --- 1. Tiêu đề ---
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ KHU VỰC KHO";

            // --- 2. Bảng Khu vực Kho (Thu nhỏ lại) ---
            dgvKhuVucKho = new DataGridView();
            dgvKhuVucKho.AllowUserToAddRows = false;
            dgvKhuVucKho.AutoGenerateColumns = false;
            dgvKhuVucKho.BackgroundColor = Color.White;
            dgvKhuVucKho.BorderStyle = BorderStyle.None;
            dgvKhuVucKho.ColumnHeadersHeight = 40;
            dgvKhuVucKho.EnableHeadersVisualStyles = false;
            dgvKhuVucKho.Location = new Point(30, 140);
            dgvKhuVucKho.ReadOnly = true;
            dgvKhuVucKho.RowHeadersVisible = false;
            dgvKhuVucKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // GIẢM CHIỀU CAO XUỐNG CÒN 250 ĐỂ NHƯỜNG CHỖ CHO BẢNG SẢN PHẨM
            dgvKhuVucKho.Size = new Size(750, 250); 
            dgvKhuVucKho.SelectionChanged += DgvKhuVucKho_SelectionChanged;
            ((ISupportInitialize)(dgvKhuVucKho)).BeginInit();

            // --- 3. Bảng Sản phẩm (MỚI) ---
            grpSanPham = new GroupBox();
            grpSanPham.Text = "Danh sách sản phẩm trong khu vực";
            grpSanPham.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpSanPham.ForeColor = Color.FromArgb(41, 128, 185);
            grpSanPham.Location = new Point(30, 410); // Đặt dưới bảng khu vực
            grpSanPham.Size = new Size(750, 200);

            dgvSanPham = new DataGridView();
            dgvSanPham.Dock = DockStyle.Fill;
            dgvSanPham.BackgroundColor = Color.White;
            dgvSanPham.BorderStyle = BorderStyle.None;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.RowHeadersVisible = false;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Style header cho bảng sản phẩm (Màu khác chút để phân biệt)
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvSanPham.EnableHeadersVisualStyles = false;
            
            grpSanPham.Controls.Add(dgvSanPham);
            ((ISupportInitialize)(dgvSanPham)).BeginInit();


            // --- 4. Các Panel chức năng (Giữ nguyên) ---
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 620); // Đẩy nút xuống thấp hơn chút

            // Add controls to form
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvKhuVucKho);
            Controls.Add(grpSanPham); // <--- Add GroupBox Sản phẩm
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvKhuVucKho)).EndInit();
            ((ISupportInitialize)(dgvSanPham)).EndInit();
            grpSanPham.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1300, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            cboTimKiem = new ComboBox();
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(10, 12);
            cboTimKiem.Size = new Size(150, 25);
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã KV", "Tên khu vực" });
            cboTimKiem.SelectedIndex = 0;

            txtTimKiem = new TextBox();
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.Location = new Point(170, 12);
            txtTimKiem.Size = new Size(300, 25);

            btnTimKiem = new Button();
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.Cursor = Cursors.Hand;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(480, 10);
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            btnRefresh = new Button();
            btnRefresh.BackColor = Color.FromArgb(149, 165, 166);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(590, 10);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Text = "Làm mới";
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
                Size = new Size(550, 450),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN KHU VỰC KHO",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            Label lblMaKV = new Label { Text = "Mã khu vực:", Location = new Point(20, yPos), Size = new Size(120, 25), Font = new Font("Segoe UI", 10F) };
            txtMaKV = new TextBox { BackColor = Color.FromArgb(236, 240, 241), Location = new Point(140, yPos), ReadOnly = true, Size = new Size(380, 25) };
            panel.Controls.Add(lblMaKV);
            panel.Controls.Add(txtMaKV);
            yPos += 40;

            Label lblTenKV = new Label { Text = "Tên khu vực: *", Location = new Point(20, yPos), Size = new Size(120, 25), Font = new Font("Segoe UI", 10F) };
            txtTenKV = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(140, yPos), Size = new Size(380, 25) };
            panel.Controls.Add(lblTenKV);
            panel.Controls.Add(txtTenKV);
            yPos += 40;

            Label lblGhiChu = new Label { Text = "Ghi chú:", Location = new Point(20, yPos), Size = new Size(120, 25), Font = new Font("Segoe UI", 10F) };
            txtGhiChu = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(140, yPos), Multiline = true, Size = new Size(380, 80) };
            panel.Controls.Add(lblGhiChu);
            panel.Controls.Add(txtGhiChu);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel { Size = new Size(1300, 50), BackColor = Color.Transparent };

            int xPos = 0;
            int btnWidth = 100;
            int btnHeight = 35;
            int spacing = 15;

            btnThem = CreateButton("➕ Thêm", xPos, Color.FromArgb(39, 174, 96), BtnThem_Click);
            panel.Controls.Add(btnThem);
            xPos += btnWidth + spacing;

            btnSua = CreateButton("✏️ Sửa", xPos, Color.FromArgb(241, 196, 15), BtnSua_Click);
            panel.Controls.Add(btnSua);
            xPos += btnWidth + spacing;

            btnXoa = CreateButton("🗑️ Xóa", xPos, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            panel.Controls.Add(btnXoa);
            xPos += btnWidth + spacing;

            btnLuu = CreateButton("💾 Lưu", xPos, Color.FromArgb(52, 152, 219), BtnLuu_Click);
            btnLuu.Enabled = false;
            panel.Controls.Add(btnLuu);
            xPos += btnWidth + spacing;

            btnHuy = CreateButton("❌ Hủy", xPos, Color.FromArgb(149, 165, 166), BtnHuy_Click);
            btnHuy.Enabled = false;
            panel.Controls.Add(btnHuy);

            return panel;
        }

        private Button CreateButton(string text, int x, Color color, EventHandler handler)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 10),
                Size = new Size(100, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += handler;
            return btn;
        }

        #region Windows Form Designer generated code

        private DataGridView dgvKhuVucKho;
        private GroupBox grpSanPham; // MỚI
        private DataGridView dgvSanPham; // MỚI
        private TextBox txtMaKV;
        private TextBox txtTenKV;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private ComboBox cboTimKiem;
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