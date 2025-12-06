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
    public partial class PhieuNhapForm : Form
    {
        private PhieuNhapBUS phieuNhapBUS = new PhieuNhapBUS();
        private NhaCungCapBUS nhaCungCapBUS = new NhaCungCapBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();

        // UI Controls
        private System.Windows.Forms.Panel pnlTop;
        private Label lblTitle;
        private System.Windows.Forms.Panel pnlFilter;
        private TextBox txtSearch;
        private ComboBox cboNhaCungCap;
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
        private DataGridView dgvPhieuNhap;

        public PhieuNhapForm()
        {
            InitializeComponent();
            LoadData();
            LoadFilters();
            CheckPermissions();
        }

        private void LoadData()
        {
            try
            {
                var list = phieuNhapBUS.GetAll();
                var displayList = list.Select(p => new
                {
                    MPN = p.MPN,
                    TenNCC = nhaCungCapBUS.GetById(p.MNCC)?.TEN ?? "",
                    TenNV = nhanVienBUS.GetById(p.MNV)?.HOTEN ?? "",
                    TG = p.TG.ToString("dd/MM/yyyy HH:mm"),
                    TongTien = p.TIEN,
                    TrangThai = p.TT == 1 ? "Đã duyệt" : (p.TT == 2 ? "Chờ duyệt" : "Đã xóa")
                }).ToList();

                dgvPhieuNhap.DataSource = displayList;
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
                // Load Nhà cung cấp
                var nccList = nhaCungCapBUS.GetAll();
                cboNhaCungCap.DataSource = new[] { new NhaCungCapDTO { MNCC = 0, TEN = "-- Tất cả --" } }.Concat(nccList).ToList();
                cboNhaCungCap.DisplayMember = "TEN";
                cboNhaCungCap.ValueMember = "MNCC";

                // Load Nhân viên
                var nvList = nhanVienBUS.GetAll();
                cboNhanVien.DataSource = new[] { new NhanVienDTO { MNV = 0, HOTEN = "-- Tất cả --" } }.Concat(nvList).ToList();
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
            btnThem.Enabled = SessionManager.CanCreate("nhaphang");
            btnSua.Enabled = SessionManager.CanUpdate("nhaphang");
            btnXoa.Enabled = SessionManager.CanDelete("nhaphang");
            btnDuyet.Enabled = SessionManager.CanApprove("nhaphang");
            btnXem.Enabled = SessionManager.CanView("nhaphang");
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                var list = phieuNhapBUS.GetAll();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    list = list.Where(p => p.MPN.ToString().Contains(txtSearch.Text)).ToList();
                }

                // Filter by NCC
                if (cboNhaCungCap.SelectedValue != null && (int)cboNhaCungCap.SelectedValue > 0)
                {
                    int mncc = (int)cboNhaCungCap.SelectedValue;
                    list = list.Where(p => p.MNCC == mncc).ToList();
                }

                // Filter by NV
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
                    MPN = p.MPN,
                    TenNCC = nhaCungCapBUS.GetById(p.MNCC)?.TEN ?? "",
                    TenNV = nhanVienBUS.GetById(p.MNV)?.HOTEN ?? "",
                    TG = p.TG.ToString("dd/MM/yyyy HH:mm"),
                    TongTien = p.TIEN,
                    TrangThai = p.TT == 1 ? "Đã duyệt" : (p.TT == 2 ? "Chờ duyệt" : "Đã xóa")
                }).ToList();

                dgvPhieuNhap.DataSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboNhaCungCap.SelectedIndex = 0;
            cboNhanVien.SelectedIndex = 0;
            chkLocTheoNgay.Checked = false;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;
            rdoTatCa.Checked = true;
            LoadData();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ChiTietPhieuNhapDialog dialog = new ChiTietPhieuNhapDialog(DialogMode.Add);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnXem_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xem!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpn = Convert.ToInt32(dgvPhieuNhap.SelectedRows[0].Cells["MPN"].Value);
            ChiTietPhieuNhapDialog dialog = new ChiTietPhieuNhapDialog(DialogMode.View, mpn);
            dialog.ShowDialog();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpn = Convert.ToInt32(dgvPhieuNhap.SelectedRows[0].Cells["MPN"].Value);

            if (!phieuNhapBUS.CanUpdate(mpn))
            {
                MessageBox.Show("Chỉ có thể sửa phiếu nhập đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChiTietPhieuNhapDialog dialog = new ChiTietPhieuNhapDialog(DialogMode.Edit, mpn);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpn = Convert.ToInt32(dgvPhieuNhap.SelectedRows[0].Cells["MPN"].Value);

            if (!phieuNhapBUS.CanDelete(mpn))
            {
                MessageBox.Show("Chỉ có thể xóa phiếu nhập đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phiếu nhập {mpn}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    int kq = phieuNhapBUS.CancelPhieuNhap(mpn);
                    if (kq > 0)
                    {
                        MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu nhập thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dgvPhieuNhap.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mpn = Convert.ToInt32(dgvPhieuNhap.SelectedRows[0].Cells["MPN"].Value);
            PhieuNhapDTO phieu = phieuNhapBUS.GetById(mpn);

            if (phieu == null)
            {
                MessageBox.Show("Không tìm thấy phiếu nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (phieu.TT != 2)
            {
                MessageBox.Show("Chỉ có thể duyệt phiếu nhập đang chờ duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn duyệt phiếu nhập {mpn}?\n" +
                $"Hành động này sẽ cập nhật số lượng tồn kho và không thể hoàn tác!",
                "Xác nhận duyệt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    int kq = phieuNhapBUS.DuyetPhieuNhap(mpn);
                    if (kq > 0)
                    {
                        MessageBox.Show("Duyệt phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Duyệt phiếu nhập thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void DgvPhieuNhap_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnXem_Click(sender, e);
            }
        }
    }
}
