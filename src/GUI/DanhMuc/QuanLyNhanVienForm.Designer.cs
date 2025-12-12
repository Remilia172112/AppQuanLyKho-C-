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
            this.Text = "Quản lý Nhân viên";
            this.Size = new Size(1300, 750);
            this.MinimumSize = new Size(1100, 650);
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
            dgvNhanVien = new DataGridView();
            dgvNhanVien.Dock = DockStyle.Fill;
            dgvNhanVien.BackgroundColor = Color.White;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.SelectionChanged += DgvNhanVien_SelectionChanged;

            // --- 2. XỬ LÝ HEADER (TITLE + SEARCH CENTER) ---

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Text = "QUẢN LÝ NHÂN VIÊN";
            pnlHeader.Controls.Add(lblTitle);

            // TẠO CONTAINER RIÊNG CHO CỤM TÌM KIẾM
            Panel pnlSearchBox = new Panel();
            pnlSearchBox.Size = new Size(820, 40);
            pnlSearchBox.BackColor = Color.Transparent;

            // Các control tìm kiếm
            cboTimKiem = new ComboBox();
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(0, 8);
            cboTimKiem.Size = new Size(130, 25);
            cboTimKiem.Items.AddRange(new string[] { "Tất cả", "Họ tên", "Email", "Số điện thoại" });
            cboTimKiem.SelectedIndex = 0;

            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(140, 8);
            txtTimKiem.Size = new Size(250, 25);
            txtTimKiem.PlaceholderText = "Nhập từ khóa...";

            btnTimKiem = CreateButtonSmall("🔍 Tìm", 400, Color.FromArgb(41, 128, 185), BtnTimKiem_Click);
            btnRefresh = CreateButtonSmall("⟳ Load", 500, Color.FromArgb(52, 152, 219), BtnRefresh_Click);
            btnImport = CreateButtonSmall("📥 Import", 600, Color.FromArgb(46, 204, 113), BtnImport_Click);
            btnExport = CreateButtonSmall("📤 Export", 700, Color.FromArgb(39, 174, 96), BtnExport_Click);

            pnlSearchBox.Controls.AddRange(new Control[] { 
                cboTimKiem, txtTimKiem, btnTimKiem, btnRefresh, btnImport, btnExport 
            });

            pnlHeader.Controls.Add(pnlSearchBox);

            // --- 3. XỬ LÝ BUTTONS DƯỚI (CANH GIỮA) ---

            Panel pnlActionBox = new Panel();
            pnlActionBox.Size = new Size(580, 50);
            pnlActionBox.BackColor = Color.Transparent;

            // Tạo các nút chức năng
            btnThem = CreateBtnAction("➕ Thêm", 0, Color.FromArgb(46, 204, 113), BtnThem_Click);
            btnSua = CreateBtnAction("✏️ Sửa", 1, Color.FromArgb(52, 152, 219), BtnSua_Click);
            btnXoa = CreateBtnAction("🗑️ Xóa", 2, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            btnLuu = CreateBtnAction("💾 Lưu", 3, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            btnHuy = CreateBtnAction("❌ Hủy", 4, Color.FromArgb(149, 165, 166), BtnHuy_Click);

            pnlActionBox.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLuu, btnHuy });
            pnlButtons.Controls.Add(pnlActionBox);

            // --- 4. SỰ KIỆN RESIZE ---

            // Canh giữa thanh tìm kiếm
            pnlHeader.Resize += (s, e) => {
                pnlSearchBox.Location = new Point(
                    (pnlHeader.Width - pnlSearchBox.Width) / 2,
                    60 
                );
            };

            // Canh giữa thanh nút bấm
            pnlButtons.Resize += (s, e) => {
                pnlActionBox.Location = new Point(
                    (pnlButtons.Width - pnlActionBox.Width) / 2,
                    10 
                );
            };

            // --- 5. ADD CONTROLS ---
            Controls.Add(dgvNhanVien);
            Controls.Add(pnlForm);
            Controls.Add(pnlHeader);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvNhanVien)).EndInit();
            this.ResumeLayout(false);
        }

        // --- HELPER METHODS ---

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

            int y = 40;
            int labelW = 90;
            int inputW = 230;
            int startX = 15;

            // Tiêu đề nhỏ
            Label lblInfo = new Label { 
                Text = "Thông tin Nhân viên", 
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(startX, 10),
                AutoSize = true
            };
            panel.Controls.Add(lblInfo);

            void AddInput(string labelText, Control control)
            {
                Label lbl = new Label { Text = labelText, Location = new Point(startX, y + 3), Size = new Size(labelW, 25) };
                control.Location = new Point(startX + labelW, y);
                control.Size = new Size(inputW, 25);
                panel.Controls.Add(lbl);
                panel.Controls.Add(control);
                y += 40;
            }

            // 1. Mã NV
            txtMaNV = new TextBox { ReadOnly = true, BackColor = SystemColors.Control };
            AddInput("Mã NV:", txtMaNV);

            // 2. Họ tên
            txtHoTen = new TextBox();
            AddInput("Họ tên: *", txtHoTen);

            // 3. Giới tính (ComboBox)
            cboGioiTinh = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cboGioiTinh.Items.AddRange(new string[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;
            AddInput("Giới tính: *", cboGioiTinh);

            // 4. Ngày sinh (DateTimePicker)
            dtpNgaySinh = new DateTimePicker { Format = DateTimePickerFormat.Short };
            dtpNgaySinh.MaxDate = DateTime.Now.AddYears(-18); // Ràng buộc tuổi 18+
            dtpNgaySinh.Value = DateTime.Now.AddYears(-22);
            AddInput("Ngày sinh: *", dtpNgaySinh);

            // 5. Số điện thoại
            txtSDT = new TextBox();
            AddInput("Số ĐT: *", txtSDT);

            // 6. Email
            txtEmail = new TextBox();
            AddInput("Email: *", txtEmail);

            return panel;
        }

        #region Components
        private DataGridView dgvNhanVien;

        // Input Fields
        private TextBox txtMaNV;
        private TextBox txtHoTen;
        private ComboBox cboGioiTinh;
        private DateTimePicker dtpNgaySinh;
        private TextBox txtSDT;
        private TextBox txtEmail;

        // Search Fields
        private TextBox txtTimKiem;
        private ComboBox cboTimKiem;

        // Buttons
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