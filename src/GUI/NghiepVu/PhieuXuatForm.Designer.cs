using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class PhieuXuatForm
    {
        private IContainer components = null;

        // UI Controls
        private Panel pnlTop;
        private Label lblTitle;
        private Panel pnlFilter;
        private TextBox txtSearch;
        private ComboBox cboKhachHang; // Khác với PhieuNhap
        private ComboBox cboNhanVien;
        private DateTimePicker dtpTuNgay;
        private DateTimePicker dtpDenNgay;
        private CheckBox chkLocTheoNgay;
        private RadioButton rdoTatCa;
        private RadioButton rdoChoDuyet;
        private RadioButton rdoDaDuyet;
        private Button btnLoc;
        private Button btnReset;
        private Panel pnlButtons;
        private Button btnThem;
        private Button btnXem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnDuyet;
        private Button btnXuatPDF;
        private Button btnExport;
        private DataGridView dgvPhieuXuat;

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
            this.Text = "Quản lý Phiếu Xuất hàng";
            this.Size = new System.Drawing.Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- Khởi tạo ---
            pnlTop = new Panel();
            lblTitle = new Label();
            pnlFilter = new Panel();
            txtSearch = new TextBox();
            cboKhachHang = new ComboBox();
            cboNhanVien = new ComboBox();
            dtpTuNgay = new DateTimePicker();
            dtpDenNgay = new DateTimePicker();
            chkLocTheoNgay = new CheckBox();
            rdoTatCa = new RadioButton();
            rdoChoDuyet = new RadioButton();
            rdoDaDuyet = new RadioButton();
            btnLoc = new Button();
            btnReset = new Button();
            pnlButtons = new Panel();
            btnThem = new Button();
            btnXem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnDuyet = new Button();
            btnXuatPDF = new Button();
            btnExport = new Button();
            dgvPhieuXuat = new DataGridView();

            Label lblSearch = new Label();
            Label lblKH = new Label();
            Label lblNV = new Label();
            Label lblTuNgay = new Label();
            Label lblDenNgay = new Label();
            Label lblTrangThai = new Label();

            ((ISupportInitialize)(dgvPhieuXuat)).BeginInit();
            pnlTop.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();

            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(220, 53, 69); // Màu đỏ cho xuất hàng
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 60;

            // lblTitle
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "QUẢN LÝ PHIẾU XUẤT HÀNG";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // pnlFilter (Đã chỉnh tọa độ)
            // 
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Height = 120;
            pnlFilter.Padding = new Padding(10);
            pnlFilter.BackColor = Color.WhiteSmoke;

            // Row 1
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(15, 15);
            lblSearch.Text = "Tìm kiếm:";
            lblSearch.Font = new Font("Segoe UI", 10F);

            txtSearch.Location = new Point(100, 12);
            txtSearch.Size = new Size(200, 25);
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.PlaceholderText = "Nhập mã phiếu...";

            lblKH.AutoSize = true;
            lblKH.Location = new Point(330, 15); // Đẩy ra 330
            lblKH.Text = "Khách hàng:";
            lblKH.Font = new Font("Segoe UI", 10F);

            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhachHang.Location = new Point(440, 12); // Đẩy ra 440
            cboKhachHang.Size = new Size(200, 25);
            cboKhachHang.Font = new Font("Segoe UI", 10F);

            lblNV.AutoSize = true;
            lblNV.Location = new Point(670, 15); // Đẩy ra 670
            lblNV.Text = "Nhân viên:";
            lblNV.Font = new Font("Segoe UI", 10F);

            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Location = new Point(760, 12); // Đẩy ra 760
            cboNhanVien.Size = new Size(200, 25);
            cboNhanVien.Font = new Font("Segoe UI", 10F);

            // Row 2
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(15, 55);
            lblTuNgay.Text = "Từ ngày:";
            lblTuNgay.Font = new Font("Segoe UI", 10F);

            dtpTuNgay.CustomFormat = "dd-MM-yyyy";
            dtpTuNgay.Enabled = false;
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(100, 52);
            dtpTuNgay.Size = new Size(130, 25);
            dtpTuNgay.Font = new Font("Segoe UI", 10F);

            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(250, 55);
            lblDenNgay.Text = "Đến ngày:";
            lblDenNgay.Font = new Font("Segoe UI", 10F);

            dtpDenNgay.CustomFormat = "dd-MM-yyyy";
            dtpDenNgay.Enabled = false;
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(330, 52);
            dtpDenNgay.Size = new Size(130, 25);
            dtpDenNgay.Font = new Font("Segoe UI", 10F);

            chkLocTheoNgay.AutoSize = true;
            chkLocTheoNgay.Location = new Point(480, 54); // Đẩy ra 480
            chkLocTheoNgay.Text = "Lọc ngày";
            chkLocTheoNgay.Font = new Font("Segoe UI", 10F);
            chkLocTheoNgay.CheckedChanged += (s, e) =>
            {
                dtpTuNgay.Enabled = chkLocTheoNgay.Checked;
                dtpDenNgay.Enabled = chkLocTheoNgay.Checked;
            };

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(580, 55); // Đẩy ra 580
            lblTrangThai.Text = "Trạng thái:";
            lblTrangThai.Font = new Font("Segoe UI", 10F);

            rdoTatCa.AutoSize = true;
            rdoTatCa.Checked = true;
            rdoTatCa.Location = new Point(660, 54);
            rdoTatCa.Text = "Tất cả";
            rdoTatCa.Font = new Font("Segoe UI", 10F);

            rdoChoDuyet.AutoSize = true;
            rdoChoDuyet.Location = new Point(740, 54);
            rdoChoDuyet.Text = "Chờ duyệt";
            rdoChoDuyet.Font = new Font("Segoe UI", 10F);

            rdoDaDuyet.AutoSize = true;
            rdoDaDuyet.Location = new Point(840, 54);
            rdoDaDuyet.Text = "Đã duyệt";
            rdoDaDuyet.Font = new Font("Segoe UI", 10F);

            // Buttons Filter
            btnLoc.BackColor = Color.FromArgb(0, 123, 255);
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(1000, 12);
            btnLoc.Size = new Size(100, 35);
            btnLoc.Text = "🔍 Lọc";
            btnLoc.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLoc.Click += BtnLoc_Click;

            btnReset.BackColor = Color.Gray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(1000, 52);
            btnReset.Size = new Size(100, 35);
            btnReset.Text = "⟳ Đặt lại";
            btnReset.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReset.Click += BtnReset_Click;

            pnlFilter.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, lblKH, cboKhachHang, lblNV, cboNhanVien,
                lblTuNgay, dtpTuNgay, lblDenNgay, dtpDenNgay, chkLocTheoNgay,
                lblTrangThai, rdoTatCa, rdoChoDuyet, rdoDaDuyet,
                btnLoc, btnReset
            });

            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.WhiteSmoke;
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 70;
            pnlButtons.Padding = new Padding(10);

            Size btnSize = new Size(130, 40);
            int startX = 20;
            int spacing = 150;

            // 1. Thêm
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(startX, 15);
            btnThem.Size = btnSize;
            btnThem.Text = "➕ Thêm";
            btnThem.Click += BtnThem_Click;

            // 2. Xem
            btnXem.BackColor = Color.FromArgb(23, 162, 184);
            btnXem.FlatStyle = FlatStyle.Flat;
            btnXem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXem.ForeColor = Color.White;
            btnXem.Location = new Point(startX + spacing, 15);
            btnXem.Size = btnSize;
            btnXem.Text = "👁 Xem chi tiết";
            btnXem.Click += BtnXem_Click;

            // 3. Sửa
            btnSua.BackColor = Color.FromArgb(255, 193, 7);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(startX + spacing * 2, 15);
            btnSua.Size = btnSize;
            btnSua.Text = "✏️ Sửa";
            btnSua.Click += BtnSua_Click;

            // 4. Xóa
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(startX + spacing * 3, 15);
            btnXoa.Size = btnSize;
            btnXoa.Text = "🗑️ Xóa";
            btnXoa.Click += BtnXoa_Click;

            // 5. Duyệt
            btnDuyet.BackColor = Color.FromArgb(102, 16, 242);
            btnDuyet.FlatStyle = FlatStyle.Flat;
            btnDuyet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDuyet.ForeColor = Color.White;
            btnDuyet.Location = new Point(startX + spacing * 4, 15);
            btnDuyet.Size = btnSize;
            btnDuyet.Text = "✅ Duyệt phiếu";
            btnDuyet.Click += BtnDuyet_Click;

            // 6. PDF
            btnXuatPDF.BackColor = Color.FromArgb(200, 35, 51);
            btnXuatPDF.FlatStyle = FlatStyle.Flat;
            btnXuatPDF.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXuatPDF.ForeColor = Color.White;
            btnXuatPDF.Location = new Point(startX + spacing * 5, 15);
            btnXuatPDF.Size = btnSize;
            btnXuatPDF.Text = "📄 Xuất PDF";
            btnXuatPDF.Click += BtnXuatPDF_Click;

            // 7. Excel
            btnExport.BackColor = Color.FromArgb(25, 135, 84);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(startX + spacing * 6, 15);
            btnExport.Size = btnSize;
            btnExport.Text = "📊 Xuất Excel";
            btnExport.Click += BtnExport_Click;

            pnlButtons.Controls.AddRange(new Control[] {
                btnThem, btnXem, btnSua, btnXoa, btnDuyet, btnXuatPDF, btnExport
            });

            // 
            // dgvPhieuXuat
            // 
            dgvPhieuXuat.AllowUserToAddRows = false;
            dgvPhieuXuat.AllowUserToDeleteRows = false;
            dgvPhieuXuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuXuat.BackgroundColor = Color.White;
            dgvPhieuXuat.BorderStyle = BorderStyle.None;
            
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPhieuXuat.ColumnHeadersHeight = 45;
            
            dgvPhieuXuat.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvPhieuXuat.RowTemplate.Height = 35;
            
            dgvPhieuXuat.Dock = DockStyle.Fill;
            dgvPhieuXuat.EnableHeadersVisualStyles = false;
            dgvPhieuXuat.MultiSelect = false;
            dgvPhieuXuat.ReadOnly = true;
            dgvPhieuXuat.RowHeadersVisible = false;
            dgvPhieuXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuXuat.CellDoubleClick += DgvPhieuXuat_CellDoubleClick;

            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "MPX", HeaderText = "Mã phiếu", DataPropertyName = "MPX" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKH", HeaderText = "Khách hàng", DataPropertyName = "TenKH" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Nhân viên", DataPropertyName = "TenNV" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TG", HeaderText = "Thời gian", DataPropertyName = "TG" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongTien", HeaderText = "Tổng tiền", DataPropertyName = "TongTien" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", DataPropertyName = "TrangThai" });

            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Form
            Controls.Add(dgvPhieuXuat);
            Controls.Add(pnlFilter);
            Controls.Add(pnlTop);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvPhieuXuat)).EndInit();
            pnlTop.ResumeLayout(false);
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}