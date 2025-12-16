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

            // --- 2. XỬ LÝ HEADER ---
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Text = "QUẢN LÝ LOẠI SẢN PHẨM";
            pnlHeader.Controls.Add(lblTitle);

            Panel pnlSearchBox = new Panel();
            pnlSearchBox.Size = new Size(680, 40);
            pnlSearchBox.BackColor = Color.Transparent;

            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(0, 8);
            txtTimKiem.Size = new Size(300, 25);
            txtTimKiem.PlaceholderText = "Nhập từ khóa...";

            Button btnTimKiem = CreateButtonSmall("🔍 Tìm", 310, Color.FromArgb(41, 128, 185), BtnTimKiem_Click);
            Button btnRefresh = CreateButtonSmall("⟳ Load", 400, Color.FromArgb(52, 152, 219), BtnRefresh_Click);
            btnImport = CreateButtonSmall("📥 Import", 490, Color.FromArgb(46, 204, 113), BtnImport_Click);
            btnExport = CreateButtonSmall("📤 Export", 580, Color.FromArgb(39, 174, 96), BtnExport_Click);

            pnlSearchBox.Controls.AddRange(new Control[] { txtTimKiem, btnTimKiem, btnRefresh, btnImport, btnExport });
            pnlHeader.Controls.Add(pnlSearchBox);

            // --- 3. XỬ LÝ BUTTONS DƯỚI (CANH GIỮA) ---
            Panel pnlActionBox = new Panel();
            pnlActionBox.Size = new Size(340, 50); 
            pnlActionBox.BackColor = Color.Transparent;

            btnThem = CreateBtnAction("➕ Thêm", 0, Color.FromArgb(46, 204, 113), BtnThem_Click);
            btnSua = CreateBtnAction("✏️ Sửa", 1, Color.FromArgb(52, 152, 219), BtnSua_Click);
            btnXoa = CreateBtnAction("🗑️ Xóa", 2, Color.FromArgb(231, 76, 60), BtnXoa_Click);

            pnlActionBox.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa });
            pnlButtons.Controls.Add(pnlActionBox);

            // --- 4. SỰ KIỆN RESIZE ---
            pnlHeader.Resize += (s, e) => {
                pnlSearchBox.Location = new Point((pnlHeader.Width - pnlSearchBox.Width) / 2, 60);
            };
            pnlButtons.Resize += (s, e) => {
                pnlActionBox.Location = new Point((pnlButtons.Width - pnlActionBox.Width) / 2, 10);
            };

            // --- 5. ADD CONTROLS ---
            Controls.Add(dgvLoaiSanPham);
            Controls.Add(pnlForm);
            Controls.Add(pnlHeader);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvLoaiSanPham)).EndInit();
            this.ResumeLayout(false);
        }

        private Button CreateButtonSmall(string text, int x, Color color, EventHandler click)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, 5);
            btn.Size = new Size(85, 30);
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

            int y = 40; 
            int labelW = 90;
            int inputW = 230;
            int startX = 15;

            void AddInput(string labelText, Control control, int height = 25)
            {
                Label lbl = new Label { Text = labelText, Location = new Point(startX, y + 3), Size = new Size(labelW, 25) };
                control.Location = new Point(startX + labelW, y);
                control.Size = new Size(inputW, height);
                panel.Controls.Add(lbl);
                panel.Controls.Add(control);
                y += height + 20; 
            }

            // 1. Mã Loại
            txtMaLSP = new TextBox { ReadOnly = true, BackColor = SystemColors.Control };
            AddInput("Mã Loại:", txtMaLSP);

            // 2. Tên Loại
            txtTenLSP = new TextBox();
            AddInput("Tên Loại: *", txtTenLSP);

            // 3. Tỉ lệ giá xuất (MỚI)
            txtTLGX = new TextBox { Text = "0", TextAlign = HorizontalAlignment.Right };
            // Chỉ cho nhập số
            txtTLGX.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            AddInput("Tỉ lệ GX (%):", txtTLGX);

            // 4. Ghi Chú (Multiline)
            txtGhiChu = new TextBox { Multiline = true };
            AddInput("Ghi chú:", txtGhiChu, 150); 

            // --- NÚT LƯU & HỦY ---
            y += 10; 

            btnLuu = new Button();
            btnLuu.Text = "💾 Lưu";
            btnLuu.Size = new Size(110, 35);
            btnLuu.Location = new Point(startX + labelW, y);
            btnLuu.BackColor = Color.FromArgb(41, 128, 185);
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += BtnLuu_Click;
            btnLuu.Visible = false;

            btnHuy = new Button();
            btnHuy.Text = "❌ Hủy";
            btnHuy.Size = new Size(110, 35);
            btnHuy.Location = new Point(startX + labelW + 120, y);
            btnHuy.BackColor = Color.FromArgb(149, 165, 166);
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += BtnHuy_Click;
            btnHuy.Visible = false;

            panel.Controls.Add(btnLuu);
            panel.Controls.Add(btnHuy);

            return panel;
        }

        #region Components
        private DataGridView dgvLoaiSanPham;
        private TextBox txtMaLSP;
        private TextBox txtTenLSP;
        private TextBox txtTLGX; // <--- MỚI
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        
        private Button btnImport;
        private Button btnExport;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        #endregion
    }
}