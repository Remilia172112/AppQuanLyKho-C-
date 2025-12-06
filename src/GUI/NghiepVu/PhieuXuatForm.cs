using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.NghiepVu
{
    public partial class PhieuXuatForm : Form
    {
        private PhieuXuatBUS phieuXuatBUS = new PhieuXuatBUS();
        private KhachHangBUS khachHangBUS = new KhachHangBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();

        // UI Controls
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

        public PhieuXuatForm()
        {
            InitializeComponent();
            LoadData();
            LoadFilters();
            CheckPermissions();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Phiếu Xuất hàng";
            this.Size = new System.Drawing.Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel Title
            pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 60;
            pnlTop.BackColor = Color.FromArgb(220, 53, 69);

            lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ PHIẾU XUẤT HÀNG";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            pnlTop.Controls.Add(lblTitle);

            // Panel Filter
            pnlFilter = new Panel();
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Height = 120;
            pnlFilter.Padding = new Padding(10);

            // Row 1 - Search and ComboBoxes
            Label lblSearch = new Label();
            lblSearch.Text = "Tìm kiếm:";
            lblSearch.Location = new Point(15, 15);
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(100, 12);
            txtSearch.Size = new Size(200, 25);
            txtSearch.PlaceholderText = "Nhập mã phiếu...";

            Label lblKH = new Label();
            lblKH.Text = "Khách hàng:";
            lblKH.Location = new Point(320, 15);
            lblKH.AutoSize = true;

            cboKhachHang = new ComboBox();
            cboKhachHang.Location = new Point(425, 12);
            cboKhachHang.Size = new Size(200, 25);
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblNV = new Label();
            lblNV.Text = "Nhân viên:";
            lblNV.Location = new Point(645, 15);
            lblNV.AutoSize = true;

            cboNhanVien = new ComboBox();
            cboNhanVien.Location = new Point(730, 12);
            cboNhanVien.Size = new Size(200, 25);
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;

            // Row 2 - Date Range and Status
            Label lblTuNgay = new Label();
            lblTuNgay.Text = "Từ ngày:";
            lblTuNgay.Location = new Point(15, 55);
            lblTuNgay.AutoSize = true;

            dtpTuNgay = new DateTimePicker();
            dtpTuNgay.Location = new Point(100, 52);
            dtpTuNgay.Size = new Size(150, 25);
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd-MM-yyyy";
            dtpTuNgay.Enabled = false;

            Label lblDenNgay = new Label();
            lblDenNgay.Text = "Đến ngày:";
            lblDenNgay.Location = new Point(260, 55);
            lblDenNgay.AutoSize = true;

            dtpDenNgay = new DateTimePicker();
            dtpDenNgay.Location = new Point(345, 52);
            dtpDenNgay.Size = new Size(150, 25);
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd-MM-yyyy";
            dtpDenNgay.Enabled = false;

            chkLocTheoNgay = new CheckBox();
            chkLocTheoNgay.Text = "Lọc theo ngày";
            chkLocTheoNgay.Location = new Point(505, 54);
            chkLocTheoNgay.AutoSize = true;
            chkLocTheoNgay.CheckedChanged += (s, e) => {
                dtpTuNgay.Enabled = chkLocTheoNgay.Checked;
                dtpDenNgay.Enabled = chkLocTheoNgay.Checked;
            };

            Label lblTrangThai = new Label();
            lblTrangThai.Text = "Trạng thái:";
            lblTrangThai.Location = new Point(625, 55);
            lblTrangThai.AutoSize = true;

            rdoTatCa = new RadioButton();
            rdoTatCa.Text = "Tất cả";
            rdoTatCa.Location = new Point(710, 52);
            rdoTatCa.AutoSize = true;
            rdoTatCa.Checked = true;

            rdoChoDuyet = new RadioButton();
            rdoChoDuyet.Text = "Chờ duyệt";
            rdoChoDuyet.Location = new Point(800, 52);
            rdoChoDuyet.AutoSize = true;

            rdoDaDuyet = new RadioButton();
            rdoDaDuyet.Text = "Đã duyệt";
            rdoDaDuyet.Location = new Point(910, 52);
            rdoDaDuyet.AutoSize = true;

            btnLoc = new Button();
            btnLoc.Text = "Lọc";
            btnLoc.Location = new Point(1050, 12);
            btnLoc.Size = new Size(100, 35);
            btnLoc.BackColor = Color.FromArgb(8, 133, 204);
            btnLoc.ForeColor = Color.White;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Click += BtnLoc_Click;

            btnReset = new Button();
            btnReset.Text = "Đặt lại";
            btnReset.Location = new Point(1050, 52);
            btnReset.Size = new Size(100, 35);
            btnReset.BackColor = Color.Gray;
            btnReset.ForeColor = Color.White;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Click += BtnReset_Click;

            pnlFilter.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, lblKH, cboKhachHang, lblNV, cboNhanVien,
                lblTuNgay, dtpTuNgay, lblDenNgay, dtpDenNgay, chkLocTheoNgay, lblTrangThai,
                rdoTatCa, rdoChoDuyet, rdoDaDuyet, btnLoc, btnReset
            });

            // Panel Buttons
            pnlButtons = new Panel();
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 70;
            pnlButtons.BackColor = Color.WhiteSmoke;

            btnThem = new Button();
            btnThem.Text = "Thêm mới";
            btnThem.Location = new Point(20, 15);
            btnThem.Size = new Size(120, 40);
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.ForeColor = Color.White;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnThem.Click += BtnThem_Click;

            btnXem = new Button();
            btnXem.Text = "Xem chi tiết";
            btnXem.Location = new Point(160, 15);
            btnXem.Size = new Size(120, 40);
            btnXem.BackColor = Color.FromArgb(8, 133, 204);
            btnXem.ForeColor = Color.White;
            btnXem.FlatStyle = FlatStyle.Flat;
            btnXem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnXem.Click += BtnXem_Click;

            btnSua = new Button();
            btnSua.Text = "Sửa";
            btnSua.Location = new Point(300, 15);
            btnSua.Size = new Size(120, 40);
            btnSua.BackColor = Color.FromArgb(255, 193, 7);
            btnSua.ForeColor = Color.White;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSua.Click += BtnSua_Click;

            btnXoa = new Button();
            btnXoa.Text = "Xóa";
            btnXoa.Location = new Point(440, 15);
            btnXoa.Size = new Size(120, 40);
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.ForeColor = Color.White;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnXoa.Click += BtnXoa_Click;

            btnDuyet = new Button();
            btnDuyet.Text = "Duyệt phiếu";
            btnDuyet.Location = new Point(580, 15);
            btnDuyet.Size = new Size(120, 40);
            btnDuyet.BackColor = Color.FromArgb(23, 162, 184);
            btnDuyet.ForeColor = Color.White;
            btnDuyet.FlatStyle = FlatStyle.Flat;
            btnDuyet.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDuyet.Click += BtnDuyet_Click;

            btnExport = new Button();
            btnExport.Text = "Xuất Excel";
            btnExport.Location = new Point(720, 15);
            btnExport.Size = new Size(120, 40);
            btnExport.BackColor = Color.FromArgb(108, 117, 125);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExport.Click += BtnExport_Click;

            pnlButtons.Controls.AddRange(new Control[] {
                btnThem, btnXem, btnSua, btnXoa, btnDuyet, btnExport
            });

            // DataGridView
            dgvPhieuXuat = new DataGridView();
            dgvPhieuXuat.Dock = DockStyle.Fill;
            dgvPhieuXuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuXuat.MultiSelect = false;
            dgvPhieuXuat.AllowUserToAddRows = false;
            dgvPhieuXuat.AllowUserToDeleteRows = false;
            dgvPhieuXuat.ReadOnly = true;
            dgvPhieuXuat.BackgroundColor = Color.White;
            dgvPhieuXuat.RowHeadersVisible = false;
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPhieuXuat.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPhieuXuat.ColumnHeadersHeight = 40;
            dgvPhieuXuat.EnableHeadersVisualStyles = false;
            dgvPhieuXuat.CellDoubleClick += DgvPhieuXuat_CellDoubleClick;

            // Add columns
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "MPX", HeaderText = "Mã phiếu", DataPropertyName = "MPX" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKH", HeaderText = "Khách hàng", DataPropertyName = "TenKH" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Nhân viên", DataPropertyName = "TenNV" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TG", HeaderText = "Thời gian", DataPropertyName = "TG" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongTien", HeaderText = "Tổng tiền", DataPropertyName = "TongTien" });
            dgvPhieuXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", DataPropertyName = "TrangThai" });

            // Format currency column
            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvPhieuXuat.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Add controls to form
            this.Controls.Add(dgvPhieuXuat);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlTop);
            this.Controls.Add(pnlButtons);
        }

        private void LoadData()
        {
            try
            {
                var list = phieuXuatBUS.GetAll();
                var displayList = list.Select(p => new
                {
                    MPX = p.MPX,
                    TenKH = khachHangBUS.GetById(p.MKH)?.HOTEN ?? "",
                    TenNV = nhanVienBUS.GetById(p.MNV)?.HOTEN ?? "",
                    TG = p.TG.ToString("dd/MM/yyyy HH:mm"),
                    TongTien = p.TIEN,
                    TrangThai = p.TT == 1 ? "Đã duyệt" : (p.TT == 2 ? "Chờ duyệt" : "Đã xóa")
                }).ToList();

                dgvPhieuXuat.DataSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Load Khách hàng - CHỈ LẤY KHÁCH HÀNG ĐANG HOẠT ĐỘNG (TT = 1)
                var khList = khachHangBUS.GetAll().Where(k => k.TT == 1).ToList();
                var khListWithAll = new[] { new KhachHangDTO { MKH = 0, HOTEN = "-- Tất cả --" } }.Concat(khList).ToList();
                cboKhachHang.DataSource = khListWithAll;
                cboKhachHang.DisplayMember = "HOTEN";
                cboKhachHang.ValueMember = "MKH";

                // Load Nhân viên - CHỈ LẤY NHÂN VIÊN ĐANG HOẠT ĐỘNG (TT = 1)
                var nvList = nhanVienBUS.GetAll().Where(nv => nv.TT == 1).ToList();
                var nvListWithAll = new[] { new NhanVienDTO { MNV = 0, HOTEN = "-- Tất cả --" } }.Concat(nvList).ToList();
                cboNhanVien.DataSource = nvListWithAll;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bộ lọc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPermissions()
        {
            btnThem.Enabled = SessionManager.CanCreate("xuathang");
            btnSua.Enabled = SessionManager.CanUpdate("xuathang");
            btnXoa.Enabled = SessionManager.CanDelete("xuathang");
            btnDuyet.Enabled = SessionManager.CanApprove("xuathang");
            btnXem.Enabled = SessionManager.CanView("xuathang");
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                var list = phieuXuatBUS.GetAll();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    list = list.Where(p => p.MPX.ToString().Contains(txtSearch.Text)).ToList();
                }

                // Filter by Khách hàng
                if (cboKhachHang.SelectedValue != null && (int)cboKhachHang.SelectedValue > 0)
                {
                    int mkh = (int)cboKhachHang.SelectedValue;
                    list = list.Where(p => p.MKH == mkh).ToList();
                }

                // Filter by Nhân viên
                if (cboNhanVien.SelectedValue != null && (int)cboNhanVien.SelectedValue > 0)
                {
                    int mnv = (int)cboNhanVien.SelectedValue;
                    list = list.Where(p => p.MNV == mnv).ToList();
                }

                // Filter by date range (chỉ khi checkbox được chọn)
                if (chkLocTheoNgay.Checked)
                {
                    list = list.Where(p => p.TG.Date >= dtpTuNgay.Value.Date && p.TG.Date <= dtpDenNgay.Value.Date).ToList();
                }

                // Filter by status
                if (rdoChoDuyet.Checked)
                {
                    list = list.Where(p => p.TT == 2).ToList();
                }
                else if (rdoDaDuyet.Checked)
                {
                    list = list.Where(p => p.TT == 1).ToList();
                }

                var displayList = list.Select(p => new
                {
                    MPX = p.MPX,
                    TenKH = khachHangBUS.GetById(p.MKH)?.HOTEN ?? "",
                    TenNV = nhanVienBUS.GetById(p.MNV)?.HOTEN ?? "",
                    TG = p.TG.ToString("dd/MM/yyyy HH:mm"),
                    TongTien = p.TIEN,
                    TrangThai = p.TT == 1 ? "Đã duyệt" : (p.TT == 2 ? "Chờ duyệt" : "Đã xóa")
                }).ToList();

                dgvPhieuXuat.DataSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboKhachHang.SelectedIndex = 0;
            cboNhanVien.SelectedIndex = 0;
            chkLocTheoNgay.Checked = false;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;
            rdoTatCa.Checked = true;
            LoadData();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ChiTietPhieuXuatDialog dialog = new ChiTietPhieuXuatDialog(DialogMode.Add);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnXem_Click(object sender, EventArgs e)
        {
            if (dgvPhieuXuat.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất cần xem!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpx = Convert.ToInt32(dgvPhieuXuat.SelectedRows[0].Cells["MPX"].Value);
            ChiTietPhieuXuatDialog dialog = new ChiTietPhieuXuatDialog(DialogMode.View, mpx);
            dialog.ShowDialog();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhieuXuat.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpx = Convert.ToInt32(dgvPhieuXuat.SelectedRows[0].Cells["MPX"].Value);

            if (!phieuXuatBUS.CanUpdate(mpx))
            {
                MessageBox.Show("Chỉ có thể sửa phiếu xuất đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChiTietPhieuXuatDialog dialog = new ChiTietPhieuXuatDialog(DialogMode.Edit, mpx);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieuXuat.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpx = Convert.ToInt32(dgvPhieuXuat.SelectedRows[0].Cells["MPX"].Value);

            if (!phieuXuatBUS.CanDelete(mpx))
            {
                MessageBox.Show("Chỉ có thể xóa phiếu xuất đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phiếu xuất {mpx}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    int kq = phieuXuatBUS.CancelPhieuXuat(mpx);
                    if (kq > 0)
                    {
                        MessageBox.Show("Xóa phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDuyet_Click(object sender, EventArgs e)
        {
            if (dgvPhieuXuat.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất cần duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpx = Convert.ToInt32(dgvPhieuXuat.SelectedRows[0].Cells["MPX"].Value);
            PhieuXuatDTO phieu = phieuXuatBUS.GetById(mpx);

            if (phieu == null)
            {
                MessageBox.Show("Không tìm thấy phiếu xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (phieu.TT != 2)
            {
                MessageBox.Show("Chỉ có thể duyệt phiếu xuất đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra tồn kho trước khi duyệt
            if (!phieuXuatBUS.CheckSLPx(mpx))
            {
                MessageBox.Show("Không đủ hàng trong kho để xuất! Vui lòng kiểm tra lại số lượng tồn kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn duyệt phiếu xuất {mpx}?\n" +
                $"Hành động này sẽ giảm số lượng tồn kho và không thể hoàn tác!",
                "Xác nhận duyệt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = phieuXuatBUS.DuyetPhieuXuat(mpx);
                    if (success)
                    {
                        MessageBox.Show("Duyệt phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Duyệt phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi duyệt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng xuất Excel sẽ được triển khai sau", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvPhieuXuat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnXem_Click(sender, e);
            }
        }
    }
}
