using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.NghiepVu
{
    public partial class PhieuKiemKeForm : Form
    {
        private PhieuKiemKeBUS phieuKiemKeBUS;
        private NhanVienBUS nhanVienBUS;

        public PhieuKiemKeForm()
        {
            try
            {
                InitializeComponent();
                
                phieuKiemKeBUS = new PhieuKiemKeBUS();
                nhanVienBUS = new NhanVienBUS();
                
                LoadFilters();
                LoadData();
                CheckPermissions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPermissions()
        {
            try
            {
                btnThem.Enabled = SessionManager.CanCreate("kiemke");
                btnSua.Enabled = SessionManager.CanUpdate("kiemke");
                btnXoa.Enabled = SessionManager.CanDelete("kiemke");
                btnDuyet.Enabled = SessionManager.CanApprove("kiemke");
            }
            catch { }
        }

        private void LoadFilters()
        {
            try
            {
                // Load ComboBox Nhân viên
                var nvList = nhanVienBUS.GetAll();
                var nvListWithAll = new[] { new NhanVienDTO { MNV = 0, HOTEN = "-- Tất cả --" } }
                    .Concat(nvList ?? new List<NhanVienDTO>()).ToList();

                cboNhanVien.DataSource = nvListWithAll;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";
                
                // Mặc định chọn "Tất cả" trạng thái để người dùng thấy dữ liệu ngay
                rdoTatCa.Checked = true;
                
                // Mặc định KHÔNG lọc theo ngày
                if(chkLocTheoNgay != null) chkLocTheoNgay.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải bộ lọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                phieuKiemKeBUS.LoadData();
                var list = phieuKiemKeBUS.GetAll();
                
                // Áp dụng bộ lọc
                list = ApplyFilters(list);
                
                // Lấy tên nhân viên để hiển thị đẹp hơn
                var nvList = nhanVienBUS.GetAll();
                
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
                        TT = p.TT // Giữ cột này để tô màu
                    };
                }).OrderByDescending(p => p.MPKK).ToList();

                dgvPhieuKiemKe.DataSource = displayList;
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<PhieuKiemKeDTO> ApplyFilters(List<PhieuKiemKeDTO> list)
        {
            if (list == null) return new List<PhieuKiemKeDTO>();

            // 1. Lọc theo Text (Mã phiếu)
            if (!string.IsNullOrWhiteSpace(txtSearch?.Text))
            {
                string searchText = txtSearch.Text.Trim();
                list = list.Where(p => p.MPKK.ToString().Contains(searchText)).ToList();
            }

            // 2. Lọc theo Nhân viên
            if (cboNhanVien?.SelectedValue != null && (int)cboNhanVien.SelectedValue > 0)
            {
                int mnv = (int)cboNhanVien.SelectedValue;
                list = list.Where(p => p.MNV == mnv).ToList();
            }

            // 3. Lọc theo Ngày (CHỈ KHI CHECKBOX ĐƯỢC CHỌN)
            if (chkLocTheoNgay != null && chkLocTheoNgay.Checked && dtpTuNgay != null && dtpDenNgay != null)
            {
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
                list = list.Where(p => p.TG >= tuNgay && p.TG <= denNgay).ToList();
            }

            // 4. Lọc theo Trạng thái
            if (rdoChoDuyet != null && rdoChoDuyet.Checked)
            {
                list = list.Where(p => p.TT == 2).ToList();
            }
            else if (rdoDaDuyet != null && rdoDaDuyet.Checked)
            {
                list = list.Where(p => p.TT == 1).ToList();
            }
            else if (rdoDaXoa != null && rdoDaXoa.Checked)
            {
                list = list.Where(p => p.TT == 0).ToList();
            }
            // rdoTatCa checked -> Không lọc gì

            return list;
        }

        private void FormatDataGridView()
        {
            if (dgvPhieuKiemKe.Rows.Count == 0) return;

            // Tô màu trạng thái
            foreach (DataGridViewRow row in dgvPhieuKiemKe.Rows)
            {
                if (row.Cells["TT"]?.Value != null && row.Cells["TrangThai"] != null)
                {
                    int tt = Convert.ToInt32(row.Cells["TT"].Value);
                    if (tt == 2) // Chờ duyệt -> Cam
                    {
                        row.Cells["TrangThai"].Style.BackColor = Color.Orange;
                        row.Cells["TrangThai"].Style.ForeColor = Color.White;
                    }
                    else if (tt == 1) // Đã duyệt -> Xanh
                    {
                        row.Cells["TrangThai"].Style.BackColor = Color.Green;
                        row.Cells["TrangThai"].Style.ForeColor = Color.White;
                    }
                    else if (tt == 0) // Đã xóa -> Xám
                    {
                        row.Cells["TrangThai"].Style.BackColor = Color.Gray;
                        row.Cells["TrangThai"].Style.ForeColor = Color.White;
                    }
                }
            }
        }

        // ===== CÁC SỰ KIỆN NÚT BẤM =====

        private void BtnTimKiem_Click(object sender, EventArgs e) => LoadData();
        
        private void RadioButton_CheckedChanged(object sender, EventArgs e) => LoadData();

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ChiTietPhieuKiemKeDialog dialog = new ChiTietPhieuKiemKeDialog(DialogMode.Add);
            if (dialog.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void BtnXem_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.SelectedRows.Count == 0) return;
            int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
            new ChiTietPhieuKiemKeDialog(DialogMode.View, mpkk).ShowDialog();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.SelectedRows.Count == 0) return;
            int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
            int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

            if (tt != 2) { MessageBox.Show("Chỉ sửa được phiếu đang chờ duyệt!"); return; }

            if (new ChiTietPhieuKiemKeDialog(DialogMode.Edit, mpkk).ShowDialog() == DialogResult.OK) LoadData();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.SelectedRows.Count == 0) return;
            int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
            int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

            if (tt != 2) { MessageBox.Show("Chỉ xóa được phiếu đang chờ duyệt!"); return; }

            if (MessageBox.Show("Xóa phiếu này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (phieuKiemKeBUS.Cancel(mpkk))
                {
                    MessageBox.Show("Đã xóa!");
                    LoadData();
                }
            }
        }

        private void BtnDuyet_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.SelectedRows.Count == 0) return;
            int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
            int tt = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["TT"].Value);

            if (tt != 2) { MessageBox.Show("Chỉ duyệt được phiếu đang chờ duyệt!"); return; }

            if (MessageBox.Show("Duyệt phiếu này? Kho sẽ được cập nhật.", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (phieuKiemKeBUS.DuyetPhieuKiemKe(mpkk))
                {
                    MessageBox.Show("Đã duyệt!");
                    LoadData();
                }
            }
        }

        private void BtnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.SelectedRows.Count == 0) return;
            try
            {
                int mpkk = Convert.ToInt32(dgvPhieuKiemKe.SelectedRows[0].Cells["MPKK"].Value);
                new WritePDF().WritePKK(mpkk);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xuất PDF: " + ex.Message); }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiemKe.Rows.Count == 0) return;
            try
            {
                TableExporter.ExportTableToExcel(dgvPhieuKiemKe, "PKK");
                MessageBox.Show("Xuất Excel thành công!");
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xuất Excel: " + ex.Message); }
        }
    }
}