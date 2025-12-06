using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.DAO;
using src.GUI.Components;

namespace src.GUI.NghiepVu
{
    public partial class ChiTietPhieuXuatDialog : Form
    {
        private DialogMode mode;
        private int? maphieu;
        private PhieuXuatBUS phieuXuatBUS = new PhieuXuatBUS();
        private KhachHangBUS khachHangBUS = new KhachHangBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();
        private ChiTietPhieuXuatDAO chiTietDAO = ChiTietPhieuXuatDAO.Instance;
        private List<ChiTietPhieuXuatDTO> danhSachChiTiet = new List<ChiTietPhieuXuatDTO>();

        // UI Controls
        private Label lblTitle;
        private Panel pnlInfo;
        private Label lblMaPhieu;
        private TextBox txtMaPhieu;
        private Label lblKhachHang;
        private ComboBox cboKhachHang;
        private Label lblNhanVien;
        private ComboBox cboNhanVien;
        private Label lblThoiGian;
        private DateTimePicker dtpThoiGian;
        private Label lblTrangThai;
        private TextBox txtTrangThai;
        private Panel pnlChiTiet;
        private Label lblChiTiet;
        private DataGridView dgvChiTiet;
        private Button btnThemSP;
        private Button btnXoaSP;
        private Button btnSuaSP;
        private Panel pnlTongTien;
        private Label lblTongTien;
        private TextBox txtTongTien;
        private Panel pnlButtons;
        private Button btnLuu;
        private Button btnHuy;

        public ChiTietPhieuXuatDialog(DialogMode mode, int? maphieu = null)
        {
            this.mode = mode;
            this.maphieu = maphieu;
            InitializeComponent();
            InitializeData();
            SetupUIByMode();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi tiết Phiếu Xuất";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            lblTitle = new Label();
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 53, 69);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(960, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Panel Info
            pnlInfo = new Panel();
            pnlInfo.Location = new Point(20, 70);
            pnlInfo.Size = new Size(960, 120);
            pnlInfo.BorderStyle = BorderStyle.FixedSingle;

            // Row 1
            lblMaPhieu = new Label();
            lblMaPhieu.Text = "Mã phiếu:";
            lblMaPhieu.Location = new Point(20, 20);
            lblMaPhieu.AutoSize = true;

            txtMaPhieu = new TextBox();
            txtMaPhieu.Location = new Point(150, 17);
            txtMaPhieu.Size = new Size(150, 25);
            txtMaPhieu.ReadOnly = true;
            txtMaPhieu.BackColor = Color.LightGray;

            lblKhachHang = new Label();
            lblKhachHang.Text = "Khách hàng:";
            lblKhachHang.Location = new Point(330, 20);
            lblKhachHang.AutoSize = true;

            cboKhachHang = new ComboBox();
            cboKhachHang.Location = new Point(460, 17);
            cboKhachHang.Size = new Size(250, 25);
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;

            // Row 2
            lblNhanVien = new Label();
            lblNhanVien.Text = "Nhân viên:";
            lblNhanVien.Location = new Point(20, 60);
            lblNhanVien.AutoSize = true;

            cboNhanVien = new ComboBox();
            cboNhanVien.Location = new Point(150, 57);
            cboNhanVien.Size = new Size(250, 25);
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;

            lblThoiGian = new Label();
            lblThoiGian.Text = "Thời gian:";
            lblThoiGian.Location = new Point(430, 60);
            lblThoiGian.AutoSize = true;

            dtpThoiGian = new DateTimePicker();
            dtpThoiGian.Location = new Point(530, 57);
            dtpThoiGian.Size = new Size(180, 25);
            dtpThoiGian.Format = DateTimePickerFormat.Custom;
            dtpThoiGian.CustomFormat = "dd/MM/yyyy HH:mm";

            lblTrangThai = new Label();
            lblTrangThai.Text = "Trạng thái:";
            lblTrangThai.Location = new Point(730, 60);
            lblTrangThai.AutoSize = true;

            txtTrangThai = new TextBox();
            txtTrangThai.Location = new Point(830, 57);
            txtTrangThai.Size = new Size(100, 25);
            txtTrangThai.ReadOnly = true;
            txtTrangThai.BackColor = Color.LightGray;

            pnlInfo.Controls.AddRange(new Control[] {
                lblMaPhieu, txtMaPhieu, lblKhachHang, cboKhachHang,
                lblNhanVien, cboNhanVien, lblThoiGian, dtpThoiGian,
                lblTrangThai, txtTrangThai
            });

            // Panel Chi tiet
            pnlChiTiet = new Panel();
            pnlChiTiet.Location = new Point(20, 200);
            pnlChiTiet.Size = new Size(960, 350);
            pnlChiTiet.BorderStyle = BorderStyle.FixedSingle;

            lblChiTiet = new Label();
            lblChiTiet.Text = "Danh sách sản phẩm";
            lblChiTiet.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblChiTiet.Location = new Point(10, 10);
            lblChiTiet.AutoSize = true;

            dgvChiTiet = new DataGridView();
            dgvChiTiet.Location = new Point(10, 40);
            dgvChiTiet.Size = new Size(940, 250);
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvChiTiet.EnableHeadersVisualStyles = false;

            // Add columns
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "MSP", HeaderText = "Mã SP", DataPropertyName = "MSP" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenSP", HeaderText = "Tên sản phẩm", DataPropertyName = "TenSP" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SL", HeaderText = "Số lượng", DataPropertyName = "SL" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GIA", HeaderText = "Đơn giá", DataPropertyName = "GIA" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành tiền", DataPropertyName = "ThanhTien" });

            dgvChiTiet.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["GIA"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["GIA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            btnThemSP = new Button();
            btnThemSP.Text = "Thêm sản phẩm";
            btnThemSP.Location = new Point(10, 300);
            btnThemSP.Size = new Size(130, 35);
            btnThemSP.BackColor = Color.FromArgb(40, 167, 69);
            btnThemSP.ForeColor = Color.White;
            btnThemSP.FlatStyle = FlatStyle.Flat;
            btnThemSP.Click += BtnThemSP_Click;

            btnSuaSP = new Button();
            btnSuaSP.Text = "Sửa";
            btnSuaSP.Location = new Point(150, 300);
            btnSuaSP.Size = new Size(100, 35);
            btnSuaSP.BackColor = Color.FromArgb(255, 193, 7);
            btnSuaSP.ForeColor = Color.White;
            btnSuaSP.FlatStyle = FlatStyle.Flat;
            btnSuaSP.Click += BtnSuaSP_Click;

            btnXoaSP = new Button();
            btnXoaSP.Text = "Xóa";
            btnXoaSP.Location = new Point(260, 300);
            btnXoaSP.Size = new Size(100, 35);
            btnXoaSP.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaSP.ForeColor = Color.White;
            btnXoaSP.FlatStyle = FlatStyle.Flat;
            btnXoaSP.Click += BtnXoaSP_Click;

            pnlChiTiet.Controls.AddRange(new Control[] {
                lblChiTiet, dgvChiTiet, btnThemSP, btnSuaSP, btnXoaSP
            });

            // Panel Tong tien
            pnlTongTien = new Panel();
            pnlTongTien.Location = new Point(20, 560);
            pnlTongTien.Size = new Size(960, 50);

            lblTongTien = new Label();
            lblTongTien.Text = "TỔNG TIỀN:";
            lblTongTien.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTongTien.Location = new Point(600, 12);
            lblTongTien.AutoSize = true;

            txtTongTien = new TextBox();
            txtTongTien.Location = new Point(740, 10);
            txtTongTien.Size = new Size(200, 30);
            txtTongTien.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            txtTongTien.ForeColor = Color.Red;
            txtTongTien.ReadOnly = true;
            txtTongTien.BackColor = Color.White;
            txtTongTien.TextAlign = HorizontalAlignment.Right;

            pnlTongTien.Controls.AddRange(new Control[] { lblTongTien, txtTongTien });

            // Panel Buttons
            pnlButtons = new Panel();
            pnlButtons.Location = new Point(20, 620);
            pnlButtons.Size = new Size(960, 50);

            btnLuu = new Button();
            btnLuu.Text = "Lưu";
            btnLuu.Location = new Point(730, 5);
            btnLuu.Size = new Size(100, 40);
            btnLuu.BackColor = Color.FromArgb(40, 167, 69);
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLuu.Click += BtnLuu_Click;

            btnHuy = new Button();
            btnHuy.Text = "Hủy";
            btnHuy.Location = new Point(840, 5);
            btnHuy.Size = new Size(100, 40);
            btnHuy.BackColor = Color.Gray;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnHuy.Click += BtnHuy_Click;

            pnlButtons.Controls.AddRange(new Control[] { btnLuu, btnHuy });

            // Add to form
            this.Controls.AddRange(new Control[] {
                lblTitle, pnlInfo, pnlChiTiet, pnlTongTien, pnlButtons
            });
        }

        private void SetupUIByMode()
        {
            switch (mode)
            {
                case DialogMode.View:
                    lblTitle.Text = "XEM CHI TIẾT PHIẾU XUẤT";
                    cboKhachHang.Enabled = false;
                    cboNhanVien.Enabled = false;
                    dtpThoiGian.Enabled = false;
                    btnThemSP.Visible = false;
                    btnSuaSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnLuu.Visible = false;
                    btnHuy.Text = "Đóng";
                    break;

                case DialogMode.Add:
                    lblTitle.Text = "THÊM PHIẾU XUẤT MỚI";
                    txtMaPhieu.Text = "(Tự động)";
                    txtTrangThai.Text = "Chờ duyệt";
                    dtpThoiGian.Value = DateTime.Now;
                    // Set current user as default
                    if (SessionManager.CurrentEmployee != null)
                    {
                        cboNhanVien.SelectedValue = SessionManager.CurrentEmployee.MNV;
                    }
                    cboNhanVien.Enabled = false;
                    break;

                case DialogMode.Edit:
                    lblTitle.Text = "SỬA PHIẾU XUẤT";
                    cboNhanVien.Enabled = false; // Cannot change employee
                    break;
            }
        }

        private void InitializeData()
        {
            try
            {
                // Load Khách hàng - CHỈ LẤY KHÁCH HÀNG ĐANG HOẠT ĐỘNG (TT = 1)
                var khList = khachHangBUS.GetAll().Where(k => k.TT == 1).ToList();
                cboKhachHang.DataSource = khList;
                cboKhachHang.DisplayMember = "TEN";
                cboKhachHang.ValueMember = "MKH";

                // Load Nhân viên - CHỈ LẤY NHÂN VIÊN ĐANG HOẠT ĐỘNG (TT = 1)
                var nvList = nhanVienBUS.GetAll().Where(nv => nv.TT == 1).ToList();
                cboNhanVien.DataSource = nvList;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";

                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    // Load phieu data
                    PhieuXuatDTO phieu = phieuXuatBUS.GetById(maphieu.Value);
                    if (phieu != null)
                    {
                        txtMaPhieu.Text = phieu.MPX.ToString();
                        cboKhachHang.SelectedValue = phieu.MKH;
                        cboNhanVien.SelectedValue = phieu.MNV;
                        dtpThoiGian.Value = phieu.TG;
                        txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                        // Load chi tiet
                        danhSachChiTiet = chiTietDAO.selectAll(maphieu.Value.ToString());
                        LoadChiTietGrid();
                    }
                }
                else
                {
                    danhSachChiTiet = new List<ChiTietPhieuXuatDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietGrid()
        {
            try
            {
                var sanPhamBUS = new SanPhamBUS();
                var displayList = danhSachChiTiet.Select(ct => new
                {
                    MSP = ct.MSP,
                    TenSP = sanPhamBUS.GetByMaSP(ct.MSP)?.TEN ?? "",
                    SL = ct.SL,
                    GIA = ct.TIENXUAT,
                    ThanhTien = ct.SL * ct.TIENXUAT
                }).ToList();

                dgvChiTiet.DataSource = displayList;
                CalculateTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTongTien()
        {
            decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENXUAT);
            txtTongTien.Text = tongTien.ToString("N0");
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            ChonSanPhamXuatDialog dialog = new ChonSanPhamXuatDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newItem = dialog.SelectedItem;
                if (newItem != null)
                {
                    // Check if product already exists
                    var existing = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == newItem.MSP);
                    if (existing != null)
                    {
                        // Update quantity
                        existing.SL += newItem.SL;
                        existing.TIENXUAT = newItem.TIENXUAT;
                    }
                    else
                    {
                        danhSachChiTiet.Add(newItem);
                    }
                    LoadChiTietGrid();
                }
            }
        }

        private void BtnSuaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int msp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
            var item = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == msp);

            if (item != null)
            {
                ChonSanPhamXuatDialog dialog = new ChonSanPhamXuatDialog(item);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var updatedItem = dialog.SelectedItem;
                    item.SL = updatedItem.SL;
                    item.TIENXUAT = updatedItem.TIENXUAT;
                    LoadChiTietGrid();
                }
            }
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int msp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                danhSachChiTiet.RemoveAll(ct => ct.MSP == msp);
                LoadChiTietGrid();
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (cboKhachHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENXUAT);

                if (mode == DialogMode.Add)
                {
                    // Kiểm tra tồn kho trước khi thêm
                    if (!phieuXuatBUS.CheckSLPx(danhSachChiTiet))
                    {
                        MessageBox.Show("Không đủ hàng trong kho! Vui lòng kiểm tra lại số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create new phieu
                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MKH = (int)cboKhachHang.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Chờ duyệt
                    };

                    int newMPX = phieuXuatBUS.Insert(phieu, danhSachChiTiet);
                    if (newMPX > 0)
                    {
                        MessageBox.Show("Thêm phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (mode == DialogMode.Edit && maphieu.HasValue)
                {
                    // Kiểm tra tồn kho trước khi sửa
                    if (!phieuXuatBUS.CheckSLPx(danhSachChiTiet))
                    {
                        MessageBox.Show("Không đủ hàng trong kho! Vui lòng kiểm tra lại số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update existing phieu
                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MPX = maphieu.Value,
                        MKH = (int)cboKhachHang.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Keep status
                    };

                    bool success = phieuXuatBUS.Update(phieu, danhSachChiTiet);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
