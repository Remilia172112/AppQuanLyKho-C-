using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLySanPhamForm
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

            this.Text = "Quản lý Sản phẩm";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvSanPham = new DataGridView();
            txtMaSP = new TextBox();
            txtTenSP = new TextBox();
            txtGiaNhap = new TextBox();
            txtGiaXuat = new TextBox();
            txtSoLuong = new TextBox();
            txtTimKiem = new TextBox();
            cboNhaSX = new ComboBox();
            cboKhuVuc = new ComboBox();
            cboTimKiem = new ComboBox();
            cboDanhMuc = new ComboBox();
            cboLoaiSP = new ComboBox();
            picHinhAnh = new PictureBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnChonAnh = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();

            Label lblTitle = new Label();

            ((ISupportInitialize)(dgvSanPham)).BeginInit();
            ((ISupportInitialize)(picHinhAnh)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ SẢN PHẨM";

            // 
            // dgvSanPham
            // 
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.BackgroundColor = Color.White;
            dgvSanPham.Location = new Point(30, 140);
            dgvSanPham.ReadOnly = true;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.Size = new Size(750, 550);
            dgvSanPham.SelectionChanged += DgvSanPham_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 710);

            // 
            // QuanLySanPhamForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvSanPham);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvSanPham)).EndInit();
            ((ISupportInitialize)(picHinhAnh)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1150, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // 
            // cboTimKiem
            // 
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(10, 12);
            cboTimKiem.Size = new Size(120, 25);
            cboTimKiem.Items.AddRange(new string[] { "Tất cả", "Tên SP" });
            cboTimKiem.SelectedIndex = 0;

            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(140, 12);
            txtTimKiem.Size = new Size(250, 25);

            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(41, 128, 185);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(400, 10);
            btnTimKiem.Size = new Size(90, 30);
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(500, 10);
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.Text = "Làm mới";
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

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
                Size = new Size(380, 550),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            int y = 15;

            // Mã SP
            Label lblMaSP = new Label { Text = "Mã SP:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtMaSP.Location = new Point(120, y);
            txtMaSP.Size = new Size(240, 25);
            txtMaSP.ReadOnly = true;
            txtMaSP.BackColor = SystemColors.Control;
            panel.Controls.Add(lblMaSP);
            panel.Controls.Add(txtMaSP);
            y += 35;

            // Tên SP
            Label lblTenSP = new Label { Text = "Tên SP: *", Location = new Point(15, y), Size = new Size(100, 25) };
            txtTenSP.Location = new Point(120, y);
            txtTenSP.Size = new Size(240, 25);
            panel.Controls.Add(lblTenSP);
            panel.Controls.Add(txtTenSP);
            y += 35;

            // Danh mục
            Label lblDanhMuc = new Label { Text = "Danh mục:", Location = new Point(15, y), Size = new Size(100, 25) };
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDown;
            cboDanhMuc.Location = new Point(120, y);
            cboDanhMuc.Size = new Size(240, 25);
            cboDanhMuc.Items.AddRange(new string[] { "Laptop", "Màn hình", "Bàn phím", "Chuột", "Tai nghe",
                "Phụ kiện", "Linh kiện", "Điện thoại", "Thiết bị mạng", "Máy in" });
            panel.Controls.Add(lblDanhMuc);
            panel.Controls.Add(cboDanhMuc);
            y += 35;

            // Loại sản phẩm
            Label lblLoaiSP = new Label { Text = "Loại SP: *", Location = new Point(15, y), Size = new Size(100, 25) };
            cboLoaiSP.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiSP.Location = new Point(120, y);
            cboLoaiSP.Size = new Size(240, 25);
            panel.Controls.Add(lblLoaiSP);
            panel.Controls.Add(cboLoaiSP);
            y += 35;

            // Nhà SX
            Label lblNhaSX = new Label { Text = "Nhà SX: *", Location = new Point(15, y), Size = new Size(100, 25) };
            cboNhaSX.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhaSX.Location = new Point(120, y);
            cboNhaSX.Size = new Size(240, 25);
            panel.Controls.Add(lblNhaSX);
            panel.Controls.Add(cboNhaSX);
            y += 35;

            // Khu vực
            Label lblKhuVuc = new Label { Text = "Khu vực: *", Location = new Point(15, y), Size = new Size(100, 25) };
            cboKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhuVuc.Location = new Point(120, y);
            cboKhuVuc.Size = new Size(240, 25);
            panel.Controls.Add(lblKhuVuc);
            panel.Controls.Add(cboKhuVuc);
            y += 35;

            // Giá nhập
            Label lblGiaNhap = new Label { Text = "Giá nhập:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtGiaNhap.Location = new Point(120, y);
            txtGiaNhap.Size = new Size(240, 25);
            txtGiaNhap.Text = "0";
            panel.Controls.Add(lblGiaNhap);
            panel.Controls.Add(txtGiaNhap);
            y += 35;

            // Giá xuất
            Label lblGiaXuat = new Label { Text = "Giá xuất:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtGiaXuat.Location = new Point(120, y);
            txtGiaXuat.Size = new Size(240, 25);
            txtGiaXuat.Text = "0";
            panel.Controls.Add(lblGiaXuat);
            panel.Controls.Add(txtGiaXuat);
            y += 35;

            // Số lượng
            Label lblSoLuong = new Label { Text = "Số lượng:", Location = new Point(15, y), Size = new Size(100, 25) };
            txtSoLuong.Location = new Point(120, y);
            txtSoLuong.Size = new Size(240, 25);
            txtSoLuong.Text = "0";
            panel.Controls.Add(lblSoLuong);
            panel.Controls.Add(txtSoLuong);
            y += 35;

            // Hình ảnh
            picHinhAnh.BorderStyle = BorderStyle.FixedSingle;
            picHinhAnh.Location = new Point(120, y);
            picHinhAnh.Size = new Size(150, 150);
            picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
            panel.Controls.Add(picHinhAnh);

            // 
            // btnChonAnh
            // 
            btnChonAnh.BackColor = Color.FromArgb(52, 152, 219);
            btnChonAnh.FlatStyle = FlatStyle.Flat;
            btnChonAnh.ForeColor = Color.White;
            btnChonAnh.Location = new Point(280, y);
            btnChonAnh.Size = new Size(80, 30);
            btnChonAnh.Text = "Chọn ảnh";
            btnChonAnh.FlatAppearance.BorderSize = 0;
            btnChonAnh.Click += BtnChonAnh_Click;
            panel.Controls.Add(btnChonAnh);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1150, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            int x = 10;

            // 
            // btnThem
            // 
            btnThem = CreateButton("Thêm", x, Color.FromArgb(46, 204, 113), BtnThem_Click);
            panel.Controls.Add(btnThem);
            x += 110;

            // 
            // btnSua
            // 
            btnSua = CreateButton("Sửa", x, Color.FromArgb(52, 152, 219), BtnSua_Click);
            panel.Controls.Add(btnSua);
            x += 110;

            // 
            // btnXoa
            // 
            btnXoa = CreateButton("Xóa", x, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            panel.Controls.Add(btnXoa);
            x += 110;

            // 
            // btnLuu
            // 
            btnLuu = CreateButton("Lưu", x, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            panel.Controls.Add(btnLuu);
            x += 110;

            // 
            // btnHuy
            // 
            btnHuy = CreateButton("Hủy", x, Color.FromArgb(149, 165, 166), BtnHuy_Click);
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

        #region Windows Form Designer generated code

        private DataGridView dgvSanPham;
        private TextBox txtMaSP;
        private TextBox txtTenSP;
        private TextBox txtGiaNhap;
        private TextBox txtGiaXuat;
        private TextBox txtSoLuong;
        private TextBox txtTimKiem;
        private ComboBox cboNhaSX;
        private ComboBox cboKhuVuc;
        private ComboBox cboTimKiem;
        private ComboBox cboDanhMuc;
        private ComboBox cboLoaiSP;
        private PictureBox picHinhAnh;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnChonAnh;
        private Button btnTimKiem;
        private Button btnRefresh;

        #endregion
    }
}
