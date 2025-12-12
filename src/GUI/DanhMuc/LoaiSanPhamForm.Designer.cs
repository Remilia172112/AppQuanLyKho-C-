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
            this.Text = "Quản lý Loại Sản phẩm";
            this.Size = new Size(1200, 750);
            this.MinimumSize = new Size(1000, 600);
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
            dgvLoaiSanPham = new DataGridView();
            dgvLoaiSanPham.Dock = DockStyle.Fill;
            dgvLoaiSanPham.BackgroundColor = Color.White;
            dgvLoaiSanPham.AllowUserToAddRows = false;
            dgvLoaiSanPham.ReadOnly = true;
            dgvLoaiSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoaiSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLoaiSanPham.SelectionChanged += DgvLoaiSanPham_SelectionChanged;

            // --- 2. XỬ LÝ HEADER (TITLE + SEARCH CENTER) ---

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Text = "QUẢN LÝ LOẠI SẢN PHẨM";
            pnlHeader.Controls.Add(lblTitle);

            // TẠO CONTAINER RIÊNG CHO CỤM TÌM KIẾM ĐỂ DỄ CANH GIỮA
            Panel pnlSearchBox = new Panel();
            pnlSearchBox.Size = new Size(680, 40); // Chiều rộng đủ chứa TextBox + 4 Nút
            pnlSearchBox.BackColor = Color.Transparent;
            
            // Các control tìm kiếm
            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(0, 8);
            txtTimKiem.Size = new Size(250, 25);
            txtTimKiem.PlaceholderText = "Nhập tên loại sản phẩm...";

            btnTimKiem = CreateButtonSmall("🔍 Tìm", 260, Color.FromArgb(41, 128, 185), BtnTimKiem_Click);
            btnRefresh = CreateButtonSmall("⟳ Load", 360, Color.FromArgb(52, 152, 219), (s, e) => LoadData());
            btnImport = CreateButtonSmall("📥 Import", 460, Color.FromArgb(46, 204, 113), BtnImport_Click);
            btnExport = CreateButtonSmall("📤 Export", 560, Color.FromArgb(39, 174, 96), BtnExport_Click);

            pnlSearchBox.Controls.AddRange(new Control[] { 
                txtTimKiem, btnTimKiem, btnRefresh, btnImport, btnExport 
            });

            pnlHeader.Controls.Add(pnlSearchBox);

            // --- 3. XỬ LÝ BUTTONS DƯỚI (CANH GIỮA) ---

            Panel pnlActionBox = new Panel();
            pnlActionBox.Size = new Size(580, 50); // Đủ chứa 5 nút
            pnlActionBox.BackColor = Color.Transparent;

            // Tạo các nút chức năng
            btnThem = CreateBtnAction("➕ Thêm", 0, Color.FromArgb(46, 204, 113), BtnThem_Click);
            btnSua = CreateBtnAction("✏️ Sửa", 1, Color.FromArgb(52, 152, 219), BtnSua_Click);
            btnXoa = CreateBtnAction("🗑️ Xóa", 2, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            btnLuu = CreateBtnAction("💾 Lưu", 3, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            btnHuy = CreateBtnAction("❌ Hủy", 4, Color.FromArgb(149, 165, 166), BtnHuy_Click);

            pnlActionBox.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLuu, btnHuy });
            pnlButtons.Controls.Add(pnlActionBox);

            // --- 4. SỰ KIỆN RESIZE ĐỂ CANH GIỮA ---

            // Khi pnlHeader resize -> Canh giữa pnlSearchBox
            pnlHeader.Resize += (s, e) => {
                pnlSearchBox.Location = new Point(
                    (pnlHeader.Width - pnlSearchBox.Width) / 2,
                    60 // Y cố định
                );
            };

            // Khi pnlButtons resize -> Canh giữa pnlActionBox
            pnlButtons.Resize += (s, e) => {
                pnlActionBox.Location = new Point(
                    (pnlButtons.Width - pnlActionBox.Width) / 2,
                    10 // Y cố định
                );
            };

            // --- 5. ADD CONTROLS VÀO FORM ---
            Controls.Add(dgvLoaiSanPham);
            Controls.Add(pnlForm);
            Controls.Add(pnlHeader);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvLoaiSanPham)).EndInit();
            this.ResumeLayout(false);
        }

        // --- CÁC HÀM HELPER ---

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

            int y = 40; // Start Y cao hơn xíu vì Form này ít trường hơn
            int labelW = 90;
            int inputW = 230;
            int startX = 15;

            // Helper cục bộ để thêm Input
            void AddInput(string labelText, Control control, int height = 25)
            {
                Label lbl = new Label { Text = labelText, Location = new Point(startX, y + 3), Size = new Size(labelW, 25) };
                control.Location = new Point(startX + labelW, y);
                control.Size = new Size(inputW, height);
                panel.Controls.Add(lbl);
                panel.Controls.Add(control);
                y += height + 20; // Khoảng cách giữa các dòng
            }

            // 1. Mã Loại
            txtMaLSP = new TextBox { ReadOnly = true, BackColor = SystemColors.Control };
            AddInput("Mã Loại:", txtMaLSP);

            // 2. Tên Loại
            txtTenLSP = new TextBox();
            AddInput("Tên Loại: *", txtTenLSP);

            // 3. Ghi Chú (Multiline)
            txtGhiChu = new TextBox { Multiline = true };
            AddInput("Ghi chú:", txtGhiChu, 150); // Chiều cao 150 cho ghi chú

            return panel;
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