using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.NghiepVu
{
    public partial class PhieuXuatForm : Form
    {
        private PhieuXuatBUS phieuXuatBUS = new PhieuXuatBUS();
        private KhachHangBUS khachHangBUS = new KhachHangBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();

        public PhieuXuatForm()
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
            try
            {
                // Kiểm tra xem DataGridView có dữ liệu không
                if (dgvPhieuXuat.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phiếu xuất để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi hàm xuất Excel từ Helper
                TableExporter.ExportTableToExcel(dgvPhieuXuat, "PX");

                MessageBox.Show("Xuất file Excel thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra xem người dùng đã chọn dòng nào chưa
                if (dgvPhieuXuat.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu xuất cần xuất PDF!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Lấy Mã phiếu xuất (MPX) từ dòng được chọn
                int mpx = Convert.ToInt32(dgvPhieuXuat.SelectedRows[0].Cells["MPX"].Value);

                // 3. Gọi class WritePDF để xuất file
                WritePDF pdfWriter = new WritePDF(); 
                pdfWriter.WritePX(mpx); // Gọi hàm WritePX dành cho phiếu xuất

                // Hàm WritePX đã tự động mở file sau khi lưu nên không cần thông báo thêm
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
