using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class PhieuXuatForm
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
            this.Text = "Quản lý Phiếu Xuất hàng";
            this.Size = new System.Drawing.Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize controls
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
            pnlTop.BackColor = Color.FromArgb(220, 53, 69);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 60;

            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "QUẢN LÝ PHIẾU XUẤT HÀNG";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // pnlFilter
            // 
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Height = 120;
            pnlFilter.Padding = new Padding(10);

            // Row 1 - Search and ComboBoxes
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(15, 15);
            lblSearch.Text = "Tìm kiếm:";

            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(100, 12);
            txtSearch.Size = new Size(200, 25);
            txtSearch.PlaceholderText = "Nhập mã phiếu...";

            lblKH.AutoSize = true;
            lblKH.Location = new Point(320, 15);
            lblKH.Text = "Khách hàng:";

            // 
            // cboKhachHang
            // 
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhachHang.Location = new Point(425, 12);
            cboKhachHang.Size = new Size(200, 25);

            lblNV.AutoSize = true;
            lblNV.Location = new Point(645, 15);
            lblNV.Text = "Nhân viên:";

            // 
            // cboNhanVien
            // 
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Location = new Point(730, 12);
            cboNhanVien.Size = new Size(200, 25);

            // Row 2 - Date Range and Status
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(15, 55);
            lblTuNgay.Text = "Từ ngày:";

            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd-MM-yyyy";
            dtpTuNgay.Enabled = false;
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(100, 52);
            dtpTuNgay.Size = new Size(150, 25);

            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(260, 55);
            lblDenNgay.Text = "Đến ngày:";

            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd-MM-yyyy";
            dtpDenNgay.Enabled = false;
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(345, 52);
            dtpDenNgay.Size = new Size(150, 25);

            // 
            // chkLocTheoNgay
            // 
            chkLocTheoNgay.AutoSize = true;
            chkLocTheoNgay.Location = new Point(505, 54);
            chkLocTheoNgay.Text = "Lọc theo ngày";
            chkLocTheoNgay.CheckedChanged += (s, e) =>
            {
                dtpTuNgay.Enabled = chkLocTheoNgay.Checked;
                dtpDenNgay.Enabled = chkLocTheoNgay.Checked;
            };

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(625, 55);
            lblTrangThai.Text = "Trạng thái:";

            // 
            // rdoTatCa
            // 
            rdoTatCa.AutoSize = true;
            rdoTatCa.Checked = true;
            rdoTatCa.Location = new Point(710, 52);
            rdoTatCa.Text = "Tất cả";

            // 
            // rdoChoDuyet
            // 
            rdoChoDuyet.AutoSize = true;
            rdoChoDuyet.Location = new Point(800, 52);
            rdoChoDuyet.Text = "Chờ duyệt";

            // 
            // rdoDaDuyet
            // 
            rdoDaDuyet.AutoSize = true;
            rdoDaDuyet.Location = new Point(910, 52);
            rdoDaDuyet.Text = "Đã duyệt";

            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(8, 133, 204);
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(1050, 12);
            btnLoc.Size = new Size(100, 35);
            btnLoc.Text = "Lọc";
            btnLoc.Click += BtnLoc_Click;

            // 
            // btnReset
            // 
            btnReset.BackColor = Color.Gray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(1050, 52);
            btnReset.Size = new Size(100, 35);
            btnReset.Text = "Đặt lại";
            btnReset.Click += BtnReset_Click;

            pnlFilter.Controls.Add(lblSearch);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(lblKH);
            pnlFilter.Controls.Add(cboKhachHang);
            pnlFilter.Controls.Add(lblNV);
            pnlFilter.Controls.Add(cboNhanVien);
            pnlFilter.Controls.Add(lblTuNgay);
            pnlFilter.Controls.Add(dtpTuNgay);
            pnlFilter.Controls.Add(lblDenNgay);
            pnlFilter.Controls.Add(dtpDenNgay);
            pnlFilter.Controls.Add(chkLocTheoNgay);
            pnlFilter.Controls.Add(lblTrangThai);
            pnlFilter.Controls.Add(rdoTatCa);
            pnlFilter.Controls.Add(rdoChoDuyet);
            pnlFilter.Controls.Add(rdoDaDuyet);
            pnlFilter.Controls.Add(btnLoc);
            pnlFilter.Controls.Add(btnReset);

            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.WhiteSmoke;
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 70;

            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(20, 15);
            btnThem.Size = new Size(120, 40);
            btnThem.Text = "Thêm mới";
            btnThem.Click += BtnThem_Click;

            // 
            // btnXem
            // 
            btnXem.BackColor = Color.FromArgb(8, 133, 204);
            btnXem.FlatStyle = FlatStyle.Flat;
            btnXem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXem.ForeColor = Color.White;
            btnXem.Location = new Point(160, 15);
            btnXem.Size = new Size(120, 40);
            btnXem.Text = "Xem chi tiết";
            btnXem.Click += BtnXem_Click;

            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(255, 193, 7);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(300, 15);
            btnSua.Size = new Size(120, 40);
            btnSua.Text = "Sửa";
            btnSua.Click += BtnSua_Click;

            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(440, 15);
            btnXoa.Size = new Size(120, 40);
            btnXoa.Text = "Xóa";
            btnXoa.Click += BtnXoa_Click;

            // 
            // btnDuyet
            // 
            btnDuyet.BackColor = Color.FromArgb(23, 162, 184);
            btnDuyet.FlatStyle = FlatStyle.Flat;
            btnDuyet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDuyet.ForeColor = Color.White;
            btnDuyet.Location = new Point(580, 15);
            btnDuyet.Size = new Size(120, 40);
            btnDuyet.Text = "Duyệt phiếu";
            btnDuyet.Click += BtnDuyet_Click;

            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(108, 117, 125);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(720, 15);
            btnExport.Size = new Size(120, 40);
            btnExport.Text = "Xuất Excel";
            btnExport.Click += BtnExport_Click;

            pnlButtons.Controls.Add(btnThem);
            pnlButtons.Controls.Add(btnXem);
            pnlButtons.Controls.Add(btnSua);
            pnlButtons.Controls.Add(btnXoa);
            pnlButtons.Controls.Add(btnDuyet);
            pnlButtons.Controls.Add(btnExport);

            // 
            // dgvPhieuXuat
            // 
            dgvPhieuXuat.AllowUserToAddRows = false;
            dgvPhieuXuat.AllowUserToDeleteRows = false;
            dgvPhieuXuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuXuat.BackgroundColor = Color.White;
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPhieuXuat.ColumnHeadersHeight = 40;
            dgvPhieuXuat.Dock = DockStyle.Fill;
            dgvPhieuXuat.EnableHeadersVisualStyles = false;
            dgvPhieuXuat.MultiSelect = false;
            dgvPhieuXuat.ReadOnly = true;
            dgvPhieuXuat.RowHeadersVisible = false;
            dgvPhieuXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuXuat.CellDoubleClick += DgvPhieuXuat_CellDoubleClick;

            // Add columns to dgvPhieuXuat
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MPX",
                HeaderText = "Mã phiếu",
                DataPropertyName = "MPX"
            });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKH",
                HeaderText = "Khách hàng",
                DataPropertyName = "TenKH"
            });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNV",
                HeaderText = "Nhân viên",
                DataPropertyName = "TenNV"
            });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TG",
                HeaderText = "Thời gian",
                DataPropertyName = "TG"
            });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TongTien",
                HeaderText = "Tổng tiền",
                DataPropertyName = "TongTien"
            });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai"
            });

            // Format currency column
            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 
            // PhieuXuatForm
            // 
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

        #region Windows Form Designer generated code

        private Panel pnlTop;
        private Label lblTitle;
        private Panel pnlFilter;
        private TextBox txtSearch;
        private ComboBox cboKhachHang;
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
        private Button btnExport;
        private DataGridView dgvPhieuXuat;

        #endregion
    }
}
