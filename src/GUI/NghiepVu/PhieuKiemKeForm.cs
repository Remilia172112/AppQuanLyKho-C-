using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.NghiepVu
{
    /// <summary>
    /// PHIẾU KIỂM KÊ - XÂY DỰNG LẠI HOÀN TOÀN
    /// Based on PhieuNhapForm structure
    /// </summary>
    public partial class PhieuKiemKeForm : Form
    {
        private PhieuKiemKeBUS phieuKiemKeBUS;
        private NhanVienBUS nhanVienBUS;

        public PhieuKiemKeForm()
        {
            try
            {
                // Step 1: Initialize UI components first
                InitializeComponent();
                
                // Step 2: Initialize BUS instances
                phieuKiemKeBUS = new PhieuKiemKeBUS();
                nhanVienBUS = new NhanVienBUS();
                
                // Step 3: Load filter data
                LoadFilters();
                
                // Step 4: Load main data
                LoadData();
                
                // Step 5: Check permissions
                CheckPermissions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPermissions()
        {
            try
            {
                bool canCreate = SessionManager.CanCreate("kiemke");
                bool canUpdate = SessionManager.CanUpdate("kiemke");
                bool canDelete = SessionManager.CanDelete("kiemke");
                bool canApprove = SessionManager.CanApprove("kiemke");

                btnThem.Enabled = canCreate;
                btnSua.Enabled = canUpdate;
                btnXoa.Enabled = canDelete;
                btnDuyet.Enabled = canApprove;
            }
            catch (Exception ex)
            {
                // Non-critical error, log but don't crash
                Console.WriteLine($"Warning: Cannot check permissions: {ex.Message}");
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Load danh sách nhân viên for filter
                var nvList = nhanVienBUS.GetAll();
                
                if (nvList == null || nvList.Count == 0)
                {
                    MessageBox.Show("Không có nhân viên trong hệ thống", 
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add "All" option
                var nvListWithAll = new[] { new NhanVienDTO { MNV = -1, HOTEN = "-- Tất cả --" } }
                    .Concat(nvList).ToList();

                cboNhanVien.DataSource = nvListWithAll;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";
                
                // Default to "Chờ duyệt"
                rdoChoDuyet.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bộ lọc: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                // Step 1: Get all phieu
                var list = phieuKiemKeBUS.GetAll();
                
                // Step 2: Get all employees (cache for performance)
                var nvList = nhanVienBUS.GetAll();
                
                // Step 3: Apply filters
                list = ApplyFilters(list);
                
                // Step 4: Build display list
                var displayList = list.Select(p =>
                {
                    var nv = nvList?.FirstOrDefault(n => n.MNV == p.MNV);
                    var chiTiet = phieuKiemKeBUS.GetChiTietPhieu(p.MPKK);

                    return new
                    {
                        MPKK = p.MPKK,
                        NhanVien = nv?.HOTEN ?? "(Không xác định)",
                        ThoiGian = p.TG.ToString("dd/MM/yyyy HH:mm"),
                        SoSP = chiTiet?.Count ?? 0,
                        TrangThai = p.TT == 1 ? "Đã duyệt" : (p.TT == 2 ? "Chờ duyệt" : "Đã xóa"),
                        TT = p.TT
                    };
                }).OrderByDescending(p => p.MPKK).ToList();

                // Step 4: Bind to grid
                dgvPhieuKiemKe.DataSource = displayList;
                
                // Step 5: Format grid
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Clear grid on error
                dgvPhieuKiemKe.DataSource = null;
            }
        }

        private List<PhieuKiemKeDTO> ApplyFilters(List<PhieuKiemKeDTO> list)
        {
            // Filter by search text (Mã phiếu)
            if (!string.IsNullOrWhiteSpace(txtSearch?.Text))
            {
                string searchText = txtSearch.Text.Trim();
                list = list.Where(p => p.MPKK.ToString().Contains(searchText)).ToList();
            }

            // Filter by Nhân viên
            if (cboNhanVien?.SelectedValue != null && (int)cboNhanVien.SelectedValue > 0)
            {
                int mnv = (int)cboNhanVien.SelectedValue;
                list = list.Where(p => p.MNV == mnv).ToList();
            }

            // Filter by date range
            if (dtpTuNgay != null && dtpDenNgay != null)
            {
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1); // End of day
                list = list.Where(p => p.TG >= tuNgay && p.TG <= denNgay).ToList();
            }

            // Filter by status (radio buttons)
            if (rdoChoDuyet?.Checked == true)
            {
                list = list.Where(p => p.TT == 2).ToList();
            }
            else if (rdoDaDuyet?.Checked == true)
            {
                list = list.Where(p => p.TT == 1).ToList();
            }
            else if (rdoDaXoa?.Checked == true)
            {
                list = list.Where(p => p.TT == 0).ToList();
            }
            // rdoTatCa - no filter

            return list;
        }

        private void FormatDataGridView()
        {
            try
            {
                // Safety check
                if (dgvPhieuKiemKe.Columns.Count == 0) return;

                // Format columns
                if (dgvPhieuKiemKe.Columns.Contains("MPKK"))
                {
                    dgvPhieuKiemKe.Columns["MPKK"].HeaderText = "Mã phiếu";
                    dgvPhieuKiemKe.Columns["MPKK"].Width = 100;
                }

                if (dgvPhieuKiemKe.Columns.Contains("NhanVien"))
                {
                    dgvPhieuKiemKe.Columns["NhanVien"].HeaderText = "Nhân viên";
                    dgvPhieuKiemKe.Columns["NhanVien"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvPhieuKiemKe.Columns.Contains("ThoiGian"))
                {
                    dgvPhieuKiemKe.Columns["ThoiGian"].HeaderText = "Thời gian";
                    dgvPhieuKiemKe.Columns["ThoiGian"].Width = 150;
                }

                if (dgvPhieuKiemKe.Columns.Contains("SoSP"))
                {
                    dgvPhieuKiemKe.Columns["SoSP"].HeaderText = "Số SP";
                    dgvPhieuKiemKe.Columns["SoSP"].Width = 80;
                    dgvPhieuKiemKe.Columns["SoSP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvPhieuKiemKe.Columns.Contains("TrangThai"))
                {
                    dgvPhieuKiemKe.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvPhieuKiemKe.Columns["TrangThai"].Width = 120;
                }

                // Hide TT column (used for color coding only)
                if (dgvPhieuKiemKe.Columns.Contains("TT"))
                {
                    dgvPhieuKiemKe.Columns["TT"].Visible = false;
                }

                // Apply color coding
                foreach (DataGridViewRow row in dgvPhieuKiemKe.Rows)
                {
                    if (row.Cells["TT"]?.Value != null && row.Cells["TrangThai"] != null)
                    {
                        int tt = Convert.ToInt32(row.Cells["TT"].Value);
                        
                        if (tt == 2) // Chờ duyệt
                        {
                            row.Cells["TrangThai"].Style.BackColor = Color.Orange;
                            row.Cells["TrangThai"].Style.ForeColor = Color.White;
                            row.Cells["TrangThai"].Style.Font = new Font(dgvPhieuKiemKe.Font, FontStyle.Bold);
                        }
                        else if (tt == 1) // Đã duyệt
                        {
                            row.Cells["TrangThai"].Style.BackColor = Color.Green;
                            row.Cells["TrangThai"].Style.ForeColor = Color.White;
                            row.Cells["TrangThai"].Style.Font = new Font(dgvPhieuKiemKe.Font, FontStyle.Bold);
                        }
                        else if (tt == 0) // Đã xóa
                        {
                            row.Cells["TrangThai"].Style.BackColor = Color.Gray;
                            row.Cells["TrangThai"].Style.ForeColor = Color.White;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Non-critical - log but don't crash
                Console.WriteLine($"Warning: Cannot format grid: {ex.Message}");
            }
        }

        // ===== EVENT HANDLERS =====
        
        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                LoadData(); // Reload with filter applied
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            try
            {
                LoadData(); // Reload when filter changes
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi bộ lọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuKiemKeDialog dialog = new ChiTietPhieuKiemKeDialog(DialogMode.Add);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // Refresh grid after adding
                    MessageBox.Show("Thêm phiếu kiểm kê thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiếu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXem_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuKiemKe.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm kê cần xem!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                ChiTietPhieuKiemKeDialog dialog = new ChiTietPhieuKiemKeDialog(DialogMode.View, mpkk);
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem phiếu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuKiemKe.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm kê cần sửa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

                // Chỉ sửa được phiếu chờ duyệt (TT = 2)
                if (tt != 2)
                {
                    MessageBox.Show("Chỉ có thể sửa phiếu ở trạng thái 'Chờ duyệt'!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ChiTietPhieuKiemKeDialog dialog = new ChiTietPhieuKiemKeDialog(DialogMode.Edit, mpkk);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // Refresh grid after editing
                    MessageBox.Show("Cập nhật phiếu kiểm kê thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa phiếu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuKiemKe.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm kê cần xóa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

                // Chỉ xóa được phiếu chờ duyệt (TT = 2)
                if (tt != 2)
                {
                    MessageBox.Show("Chỉ có thể xóa phiếu ở trạng thái 'Chờ duyệt'!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phiếu kiểm kê #{mpkk}?", 
                    "Xác nhận xóa", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (phieuKiemKeBUS.Cancel(mpkk))
                    {
                        LoadData(); // Refresh grid after deleting
                        MessageBox.Show("Xóa phiếu kiểm kê thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa phiếu kiểm kê!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phiếu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDuyet_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuKiemKe.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm kê cần duyệt!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

                // Chỉ duyệt được phiếu chờ duyệt (TT = 2)
                if (tt != 2)
                {
                    MessageBox.Show("Phiếu này không ở trạng thái 'Chờ duyệt'!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm before approving (important action)
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn DUYỆT phiếu kiểm kê #{mpkk}?\n\n" +
                    "Sau khi duyệt, số lượng tồn kho sẽ được điều chỉnh theo kết quả kiểm kê và không thể sửa lại!", 
                    "Xác nhận duyệt", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (phieuKiemKeBUS.DuyetPhieuKiemKe(mpkk))
                    {
                        LoadData(); // Refresh grid after approval
                        MessageBox.Show("Duyệt phiếu kiểm kê thành công!\n\nTồn kho đã được điều chỉnh.", 
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể duyệt phiếu kiểm kê!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi duyệt phiếu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXuatPDF_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuKiemKe.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm kê cần xuất PDF!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                
                // TODO: Implement PDF export using writePDF helper
                MessageBox.Show($"Chức năng xuất PDF cho phiếu #{mpkk} đang được phát triển!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
