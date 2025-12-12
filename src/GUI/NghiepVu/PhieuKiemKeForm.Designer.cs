using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class PhieuKiemKeForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlFilter;
        private Label lblTitle;
        private TextBox txtSearch;
        private Button btnTimKiem;
        private Label lblNhanVien;
        private ComboBox cboNhanVien;
        
        // Nhóm Date Filter
        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;
        private CheckBox chkLocTheoNgay; // <--- Đã thêm mới

        private GroupBox grpTrangThai;
        private RadioButton rdoTatCa;
        private RadioButton rdoChoDuyet;
        private RadioButton rdoDaDuyet;
        private RadioButton rdoDaXoa;
        
        private Panel pnlButtons;
        private Button btnThem;
        private Button btnXem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnDuyet;
        private Button btnXuatPDF;
        private Button btnExport; 
        private DataGridView dgvPhieuKiemKe;

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
            this.pnlFilter = new Panel();
            this.lblTitle = new Label();
            this.txtSearch = new TextBox();
            this.btnTimKiem = new Button();
            this.lblNhanVien = new Label();
            this.cboNhanVien = new ComboBox();
            this.lblTuNgay = new Label();
            this.dtpTuNgay = new DateTimePicker();
            this.lblDenNgay = new Label();
            this.dtpDenNgay = new DateTimePicker();
            this.chkLocTheoNgay = new CheckBox(); // <--- Khởi tạo
            this.grpTrangThai = new GroupBox();
            this.rdoTatCa = new RadioButton();
            this.rdoChoDuyet = new RadioButton();
            this.rdoDaDuyet = new RadioButton();
            this.rdoDaXoa = new RadioButton();
            this.pnlButtons = new Panel();
            this.btnThem = new Button();
            this.btnXem = new Button();
            this.btnSua = new Button();
            this.btnXoa = new Button();
            this.btnDuyet = new Button();
            this.btnXuatPDF = new Button();
            this.btnExport = new Button(); 
            this.dgvPhieuKiemKe = new DataGridView();

            this.pnlFilter.SuspendLayout();
            this.grpTrangThai.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuKiemKe)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = Color.FromArgb(240, 240, 240);
            this.pnlFilter.Dock = DockStyle.Top;
            this.pnlFilter.Location = new Point(0, 0);
            this.pnlFilter.Size = new Size(1100, 180);
            this.pnlFilter.Controls.Add(this.lblTitle);
            this.pnlFilter.Controls.Add(this.txtSearch);
            this.pnlFilter.Controls.Add(this.btnTimKiem);
            this.pnlFilter.Controls.Add(this.lblNhanVien);
            this.pnlFilter.Controls.Add(this.cboNhanVien);
            this.pnlFilter.Controls.Add(this.lblTuNgay);
            this.pnlFilter.Controls.Add(this.dtpTuNgay);
            this.pnlFilter.Controls.Add(this.lblDenNgay);
            this.pnlFilter.Controls.Add(this.dtpDenNgay);
            this.pnlFilter.Controls.Add(this.chkLocTheoNgay); // <--- Add CheckBox
            this.pnlFilter.Controls.Add(this.grpTrangThai);

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Size = new Size(1060, 30);
            this.lblTitle.Text = "QUẢN LÝ PHIẾU KIỂM KÊ";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // txtSearch
            this.txtSearch.Font = new Font("Segoe UI", 10F);
            this.txtSearch.Location = new Point(20, 60);
            this.txtSearch.Size = new Size(250, 25);
            this.txtSearch.PlaceholderText = "Tìm theo mã phiếu...";

            // btnTimKiem
            this.btnTimKiem.BackColor = Color.FromArgb(0, 122, 204);
            this.btnTimKiem.FlatStyle = FlatStyle.Flat;
            this.btnTimKiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnTimKiem.ForeColor = Color.White;
            this.btnTimKiem.Location = new Point(280, 58);
            this.btnTimKiem.Size = new Size(100, 30);
            this.btnTimKiem.Text = "🔍 Tìm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += BtnTimKiem_Click;

            // lblNhanVien
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Font = new Font("Segoe UI", 9F);
            this.lblNhanVien.Location = new Point(20, 100);
            this.lblNhanVien.Text = "Nhân viên:";

            // cboNhanVien
            this.cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboNhanVien.Font = new Font("Segoe UI", 9F);
            this.cboNhanVien.Location = new Point(100, 97);
            this.cboNhanVien.Size = new Size(200, 25);

            // lblTuNgay
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Font = new Font("Segoe UI", 9F);
            this.lblTuNgay.Location = new Point(320, 100);
            this.lblTuNgay.Text = "Từ ngày:";

            // dtpTuNgay
            this.dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTuNgay.Font = new Font("Segoe UI", 9F);
            this.dtpTuNgay.Format = DateTimePickerFormat.Custom;
            this.dtpTuNgay.Location = new Point(380, 97);
            this.dtpTuNgay.Size = new Size(120, 25);
            this.dtpTuNgay.Enabled = false; // Mặc định tắt

            // lblDenNgay
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Font = new Font("Segoe UI", 9F);
            this.lblDenNgay.Location = new Point(510, 100);
            this.lblDenNgay.Text = "Đến ngày:";

            // dtpDenNgay
            this.dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpDenNgay.Font = new Font("Segoe UI", 9F);
            this.dtpDenNgay.Format = DateTimePickerFormat.Custom;
            this.dtpDenNgay.Location = new Point(580, 97);
            this.dtpDenNgay.Size = new Size(120, 25);
            this.dtpDenNgay.Enabled = false; // Mặc định tắt

            // chkLocTheoNgay (MỚI)
            this.chkLocTheoNgay.AutoSize = true;
            this.chkLocTheoNgay.Location = new Point(720, 100);
            this.chkLocTheoNgay.Text = "Lọc theo ngày";
            this.chkLocTheoNgay.Font = new Font("Segoe UI", 9F);
            // Sự kiện bật tắt datetime picker
            this.chkLocTheoNgay.CheckedChanged += (s, e) => {
                dtpTuNgay.Enabled = chkLocTheoNgay.Checked;
                dtpDenNgay.Enabled = chkLocTheoNgay.Checked;
            };

            // grpTrangThai
            this.grpTrangThai.Font = new Font("Segoe UI", 9F);
            this.grpTrangThai.Location = new Point(20, 135);
            this.grpTrangThai.Size = new Size(680, 35);
            this.grpTrangThai.Text = "Trạng thái";
            this.grpTrangThai.Controls.Add(this.rdoTatCa);
            this.grpTrangThai.Controls.Add(this.rdoChoDuyet);
            this.grpTrangThai.Controls.Add(this.rdoDaDuyet);
            this.grpTrangThai.Controls.Add(this.rdoDaXoa);

            // rdoTatCa
            this.rdoTatCa.AutoSize = true;
            this.rdoTatCa.Location = new Point(60, 15);
            this.rdoTatCa.Text = "Tất cả";
            this.rdoTatCa.Checked = true; // Mặc định chọn tất cả
            this.rdoTatCa.CheckedChanged += RadioButton_CheckedChanged;

            // rdoChoDuyet
            this.rdoChoDuyet.AutoSize = true;
            this.rdoChoDuyet.Location = new Point(160, 15);
            this.rdoChoDuyet.Text = "Chờ duyệt";
            this.rdoChoDuyet.CheckedChanged += RadioButton_CheckedChanged;

            // rdoDaDuyet
            this.rdoDaDuyet.AutoSize = true;
            this.rdoDaDuyet.Location = new Point(280, 15);
            this.rdoDaDuyet.Text = "Đã duyệt";
            this.rdoDaDuyet.CheckedChanged += RadioButton_CheckedChanged;

            // rdoDaXoa
            this.rdoDaXoa.AutoSize = true;
            this.rdoDaXoa.Location = new Point(390, 15);
            this.rdoDaXoa.Text = "Đã xóa";
            this.rdoDaXoa.CheckedChanged += RadioButton_CheckedChanged;

            // pnlButtons
            this.pnlButtons.BackColor = Color.FromArgb(240, 240, 240);
            this.pnlButtons.Dock = DockStyle.Bottom;
            this.pnlButtons.Location = new Point(0, 650);
            this.pnlButtons.Size = new Size(1100, 50);
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Controls.Add(this.btnXem);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnDuyet);
            this.pnlButtons.Controls.Add(this.btnXuatPDF);
            this.pnlButtons.Controls.Add(this.btnExport);

            // Buttons Config
            this.btnThem.Text = "+ Thêm";
            this.btnThem.Location = new Point(20, 10);
            this.btnThem.Size = new Size(100, 35);
            this.btnThem.BackColor = Color.FromArgb(46, 125, 50);
            this.btnThem.ForeColor = Color.White;
            this.btnThem.FlatStyle = FlatStyle.Flat;
            this.btnThem.Click += BtnThem_Click;

            this.btnXem.Text = "👁 Xem";
            this.btnXem.Location = new Point(130, 10);
            this.btnXem.Size = new Size(100, 35);
            this.btnXem.BackColor = Color.FromArgb(0, 122, 204);
            this.btnXem.ForeColor = Color.White;
            this.btnXem.FlatStyle = FlatStyle.Flat;
            this.btnXem.Click += BtnXem_Click;

            this.btnSua.Text = "✏ Sửa";
            this.btnSua.Location = new Point(240, 10);
            this.btnSua.Size = new Size(100, 35);
            this.btnSua.BackColor = Color.FromArgb(255, 152, 0);
            this.btnSua.ForeColor = Color.White;
            this.btnSua.FlatStyle = FlatStyle.Flat;
            this.btnSua.Click += BtnSua_Click;

            this.btnXoa.Text = "✗ Xóa";
            this.btnXoa.Location = new Point(350, 10);
            this.btnXoa.Size = new Size(100, 35);
            this.btnXoa.BackColor = Color.FromArgb(211, 47, 47);
            this.btnXoa.ForeColor = Color.White;
            this.btnXoa.FlatStyle = FlatStyle.Flat;
            this.btnXoa.Click += BtnXoa_Click;

            this.btnDuyet.Text = "✓ Duyệt";
            this.btnDuyet.Location = new Point(460, 10);
            this.btnDuyet.Size = new Size(100, 35);
            this.btnDuyet.BackColor = Color.FromArgb(76, 175, 80);
            this.btnDuyet.ForeColor = Color.White;
            this.btnDuyet.FlatStyle = FlatStyle.Flat;
            this.btnDuyet.Click += BtnDuyet_Click;

            this.btnXuatPDF.Text = "📄 PDF";
            this.btnXuatPDF.Location = new Point(570, 10);
            this.btnXuatPDF.Size = new Size(100, 35);
            this.btnXuatPDF.BackColor = Color.FromArgb(200, 35, 51);
            this.btnXuatPDF.ForeColor = Color.White;
            this.btnXuatPDF.FlatStyle = FlatStyle.Flat;
            this.btnXuatPDF.Click += BtnXuatPDF_Click;

            this.btnExport.Text = "📊 Excel";
            this.btnExport.Location = new Point(680, 10);
            this.btnExport.Size = new Size(100, 35);
            this.btnExport.BackColor = Color.FromArgb(25, 135, 84);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.Click += BtnExport_Click;

            // dgvPhieuKiemKe
            this.dgvPhieuKiemKe.AllowUserToAddRows = false;
            this.dgvPhieuKiemKe.AllowUserToDeleteRows = false;
            this.dgvPhieuKiemKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieuKiemKe.BackgroundColor = Color.White;
            this.dgvPhieuKiemKe.ColumnHeadersHeight = 40;
            this.dgvPhieuKiemKe.Dock = DockStyle.Fill;
            this.dgvPhieuKiemKe.Location = new Point(0, 180);
            this.dgvPhieuKiemKe.ReadOnly = true;
            this.dgvPhieuKiemKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhieuKiemKe.Size = new Size(1100, 470);

            // PhieuKiemKeForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1100, 700);
            this.Controls.Add(this.dgvPhieuKiemKe);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlFilter);
            this.Name = "PhieuKiemKeForm";
            this.Text = "Quản lý phiếu kiểm kê";
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.grpTrangThai.ResumeLayout(false);
            this.grpTrangThai.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuKiemKe)).EndInit();
            this.ResumeLayout(false);
        }
    }
}