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
            this.Text = "Quản lý Sản phẩm";
            this.Size = new Size(1400, 800);
            this.MinimumSize = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized; 
            this.BackColor = Color.FromArgb(236, 240, 241);

            // --- 1. SETUP LAYOUT CHÍNH ---
            
            // Panel Header (Tiêu đề + Search)
            Panel pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 110; 
            pnlHeader.BackColor = Color.WhiteSmoke;
            pnlHeader.Padding = new Padding(10);

            // Panel Form nhập liệu (Bên phải)
            Panel pnlForm = CreateFormPanel();
            pnlForm.Dock = DockStyle.Right;
            pnlForm.Width = 360; 

            // Panel Nút chức năng (Dưới cùng)
            Panel pnlButtons = new Panel();
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 70;
            pnlButtons.BackColor = Color.White;

            // DataGridView (Ở giữa - Fill)
            dgvSanPham = new DataGridView();
            dgvSanPham.Dock = DockStyle.Fill;
            dgvSanPham.BackgroundColor = Color.White;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.SelectionChanged += DgvSanPham_SelectionChanged;

            // --- 2. XỬ LÝ HEADER (TITLE + SEARCH CENTER) ---
            
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Text = "QUẢN LÝ SẢN PHẨM";
            pnlHeader.Controls.Add(lblTitle);

            // TẠO CONTAINER RIÊNG CHO CỤM TÌM KIẾM ĐỂ DỄ CANH GIỮA
            Panel pnlSearchBox = new Panel();
            pnlSearchBox.Size = new Size(820, 40); // Chiều rộng đủ chứa các nút
            pnlSearchBox.BackColor = Color.Transparent;
            // Tạm thời để location 0,0, lát nữa sự kiện Resize sẽ chỉnh lại
            
            // 2.1 Các control tìm kiếm (Add vào pnlSearchBox)
            cboTimKiem = new ComboBox();
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(0, 8);
            cboTimKiem.Size = new Size(130, 25);
            cboTimKiem.Items.AddRange(new string[] { "Tất cả", "Mã SP", "Tên SP", "Danh mục" });
            cboTimKiem.SelectedIndex = 0;

            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(140, 8);
            txtTimKiem.Size = new Size(250, 25);
            txtTimKiem.PlaceholderText = "Nhập từ khóa...";

            btnTimKiem = CreateButtonSmall("🔍 Tìm", 400, Color.FromArgb(41, 128, 185), BtnTimKiem_Click);
            btnRefresh = CreateButtonSmall("⟳ Load", 500, Color.FromArgb(52, 152, 219), (s,e)=>LoadData());
            btnImport = CreateButtonSmall("📥 Import", 600, Color.FromArgb(46, 204, 113), BtnImport_Click);
            btnExport = CreateButtonSmall("📤 Export", 700, Color.FromArgb(39, 174, 96), BtnExport_Click);

            pnlSearchBox.Controls.AddRange(new Control[] { 
                cboTimKiem, txtTimKiem, btnTimKiem, btnRefresh, btnImport, btnExport 
            });

            pnlHeader.Controls.Add(pnlSearchBox);

            // --- 3. XỬ LÝ BUTTONS DƯỚI (CANH GIỮA LUÔN) ---
            
            Panel pnlActionBox = new Panel();
            pnlActionBox.Size = new Size(580, 50); // Đủ chứa 5 nút
            pnlActionBox.BackColor = Color.Transparent;
            
            // Tạo các nút chức năng
            int btnW = 100, gap = 20;
            btnThem = CreateBtnAction("➕ Thêm", 0, Color.FromArgb(46, 204, 113), BtnThem_Click);
            btnSua = CreateBtnAction("✏️ Sửa", 1, Color.FromArgb(52, 152, 219), BtnSua_Click);
            btnXoa = CreateBtnAction("🗑️ Xóa", 2, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            btnLuu = CreateBtnAction("💾 Lưu", 3, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            btnHuy = CreateBtnAction("❌ Hủy", 4, Color.FromArgb(149, 165, 166), BtnHuy_Click);
            
            pnlActionBox.Controls.AddRange(new Control[]{ btnThem, btnSua, btnXoa, btnLuu, btnHuy });
            pnlButtons.Controls.Add(pnlActionBox);

            // --- 4. SỰ KIỆN RESIZE ĐỂ CANH GIỮA (QUAN TRỌNG) ---
            
            // Khi pnlHeader thay đổi kích thước -> Tính lại vị trí pnlSearchBox
            pnlHeader.Resize += (s, e) => {
                pnlSearchBox.Location = new Point(
                    (pnlHeader.Width - pnlSearchBox.Width) / 2, // Canh giữa theo chiều ngang
                    60 // Y cố định
                );
            };

            // Khi pnlButtons thay đổi kích thước -> Tính lại vị trí pnlActionBox
            pnlButtons.Resize += (s, e) => {
                pnlActionBox.Location = new Point(
                    (pnlButtons.Width - pnlActionBox.Width) / 2, // Canh giữa theo chiều ngang
                    10 // Y cố định
                );
            };

            // --- 5. ADD CONTROLS VÀO FORM ---
            Controls.Add(dgvSanPham);
            Controls.Add(pnlForm);
            Controls.Add(pnlHeader);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvSanPham)).EndInit();
            ((ISupportInitialize)(picHinhAnh)).EndInit();
            ResumeLayout(false);
        }

        // --- CÁC HÀM HELPER GIÚP CODE GỌN HƠN ---

        private Button CreateButtonSmall(string text, int x, Color color, EventHandler click)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, 5);
            btn.Size = new Size(90, 30);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += click;
            return btn;
        }

        private Button CreateBtnAction(string text, int index, Color color, EventHandler click)
        {
            int btnW = 100, gap = 20;
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point((btnW + gap) * index, 10);
            btn.Size = new Size(btnW, 35);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += click;
            return btn;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            int y = 20;
            int labelW = 90;
            int inputW = 230;
            int startX = 15;

            // Hình ảnh
            picHinhAnh = new PictureBox();
            picHinhAnh.BorderStyle = BorderStyle.FixedSingle;
            picHinhAnh.Location = new Point(startX + 40, y);
            picHinhAnh.Size = new Size(200, 150);
            picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
            panel.Controls.Add(picHinhAnh);
            
            y += 160;
            btnChonAnh = new Button();
            btnChonAnh.BackColor = Color.FromArgb(52, 152, 219);
            btnChonAnh.FlatStyle = FlatStyle.Flat;
            btnChonAnh.ForeColor = Color.White;
            btnChonAnh.Location = new Point(startX + 95, y);
            btnChonAnh.Size = new Size(90, 30);
            btnChonAnh.Text = "Chọn ảnh";
            btnChonAnh.Click += BtnChonAnh_Click;
            panel.Controls.Add(btnChonAnh);

            y += 50;
            
            void AddInput(string label, Control control)
            {
                Label lbl = new Label { Text = label, Location = new Point(startX, y + 3), Size = new Size(labelW, 25) };
                control.Location = new Point(startX + labelW, y);
                control.Size = new Size(inputW, 25);
                panel.Controls.Add(lbl);
                panel.Controls.Add(control);
                y += 40;
            }

            txtMaSP = new TextBox { ReadOnly = true, BackColor = SystemColors.Control };
            AddInput("Mã SP:", txtMaSP);

            txtTenSP = new TextBox();
            AddInput("Tên SP: *", txtTenSP);

            cboDanhMuc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cboDanhMuc.Items.AddRange(new string[] { "Laptop", "Màn hình", "Bàn phím", "Chuột", "Tai nghe", "Phụ kiện", "Linh kiện", "Điện thoại" });
            AddInput("Danh mục:", cboDanhMuc);

            cboLoaiSP = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AddInput("Loại SP: *", cboLoaiSP);

            cboNhaSX = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AddInput("Nhà SX: *", cboNhaSX);

            cboKhuVuc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AddInput("Khu vực: *", cboKhuVuc);

            txtGiaNhap = new TextBox { Text = "0", TextAlign = HorizontalAlignment.Right };
            AddInput("Giá nhập:", txtGiaNhap);

            txtGiaXuat = new TextBox { Text = "0", TextAlign = HorizontalAlignment.Right };
            AddInput("Giá xuất:", txtGiaXuat);

            txtSoLuong = new TextBox { Text = "0", TextAlign = HorizontalAlignment.Center };
            AddInput("Số lượng:", txtSoLuong);

            return panel;
        }

        private void FormatDataGridView()
        {
            if (dgvSanPham.Columns.Count == 0) return;

            if (dgvSanPham.Columns.Contains("MSP")) 
            {
                dgvSanPham.Columns["MSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["MSP"].Width = 80;
                dgvSanPham.Columns["MSP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvSanPham.Columns.Contains("TEN")) 
            {
                dgvSanPham.Columns["TEN"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["TEN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvSanPham.Columns.Contains("DANHMUC")) 
            {
                dgvSanPham.Columns["DANHMUC"].HeaderText = "Danh mục";
                dgvSanPham.Columns["DANHMUC"].Width = 120;
            }
            if (dgvSanPham.Columns.Contains("TIENX")) 
            {
                dgvSanPham.Columns["TIENX"].HeaderText = "Giá xuất";
                dgvSanPham.Columns["TIENX"].DefaultCellStyle.Format = "N0"; 
                dgvSanPham.Columns["TIENX"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSanPham.Columns["TIENX"].Width = 120;
            }
            if (dgvSanPham.Columns.Contains("SL")) 
            {
                dgvSanPham.Columns["SL"].HeaderText = "Số lượng";
                dgvSanPham.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSanPham.Columns["SL"].Width = 80;
            }

            string[] hiddenColumns = { "TIENN", "HINHANH", "MNSX", "MKVK", "MLSP", "TT" };
            foreach (string colName in hiddenColumns)
            {
                if (dgvSanPham.Columns.Contains(colName)) dgvSanPham.Columns[colName].Visible = false;
            }
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
        private Button btnImport;
        private Button btnExport;
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