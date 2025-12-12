using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.DanhMuc
{
    public partial class LoaiSanPhamForm : Form
    {
        private LoaiSanPhamBUS lspBUS;
        private bool isEditing = false;
        private int currentID = -1;

        public LoaiSanPhamForm()
        {
            InitializeComponent();
            lspBUS = new LoaiSanPhamBUS();
            
            LoadData();
            SetButtonStates(false);
            CheckPermissions();
        }

        private void CheckPermissions()
        {
            // Kiểm tra quyền trên chức năng "loaisanpham"
            btnThem.Enabled = SessionManager.CanCreate("loaisanpham");
            btnSua.Enabled = SessionManager.CanUpdate("loaisanpham");
            btnXoa.Enabled = SessionManager.CanDelete("loaisanpham");
        }

        private void LoadData()
        {
            try
            {
                var list = lspBUS.GetAll();
                dgvLoaiSanPham.DataSource = null;
                dgvLoaiSanPham.DataSource = list;

                if (dgvLoaiSanPham.Columns.Count > 0)
                {
                    dgvLoaiSanPham.Columns["MLSP"].HeaderText = "Mã Loại";
                    dgvLoaiSanPham.Columns["TEN"].HeaderText = "Tên Loại Sản Phẩm";
                    dgvLoaiSanPham.Columns["GHICHU"].HeaderText = "Ghi chú";
                    dgvLoaiSanPham.Columns["TT"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            // Nếu đang edit thì khóa các nút Thêm/Sửa/Xóa, mở Lưu/Hủy
            btnThem.Enabled = !editing && SessionManager.CanCreate("loaisanpham");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("loaisanpham");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("loaisanpham");
            
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            
            // Control states
            txtTenLSP.ReadOnly = !editing;
            txtGhiChu.ReadOnly = !editing;
            dgvLoaiSanPham.Enabled = !editing;
        }

        private void ClearForm()
        {
            txtMaLSP.Clear();
            txtTenLSP.Clear();
            txtGhiChu.Clear();
            currentID = -1;
        }

        private void DgvLoaiSanPham_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLoaiSanPham.SelectedRows.Count > 0 && !isEditing)
            {
                try
                {
                    var row = dgvLoaiSanPham.SelectedRows[0];
                    txtMaLSP.Text = row.Cells["MLSP"].Value?.ToString();
                    txtTenLSP.Text = row.Cells["TEN"].Value?.ToString();
                    txtGhiChu.Text = row.Cells["GHICHU"].Value?.ToString();
                }
                catch { }
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtTenLSP.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLSP.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            currentID = int.Parse(txtMaLSP.Text);
            SetButtonStates(true);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLSP.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = int.Parse(txtMaLSP.Text);
                    if (lspBUS.Delete(id))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenLSP.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LoaiSanPhamDTO lsp = new LoaiSanPhamDTO
                {
                    MLSP = currentID == -1 ? 0 : currentID,
                    TEN = txtTenLSP.Text.Trim(),
                    GHICHU = txtGhiChu.Text.Trim(),
                    TT = 1
                };

                bool success;
                if (currentID == -1) // Thêm mới
                {
                    success = lspBUS.Add(lsp);
                }
                else // Cập nhật
                {
                    success = lspBUS.Update(lsp);
                }

                if (success)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    SetButtonStates(false);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetButtonStates(false);
            ClearForm();
            // Select lại dòng đầu nếu có
            if (dgvLoaiSanPham.Rows.Count > 0)
                DgvLoaiSanPham_SelectionChanged(null, null);
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            var all = lspBUS.GetAll();
            var filtered = all.FindAll(x => x.TEN.ToLower().Contains(keyword));
            
            dgvLoaiSanPham.DataSource = null;
            dgvLoaiSanPham.DataSource = filtered;
        }
        // Sự kiện nút Xuất Excel
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLoaiSanPham.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Gọi Helper xuất Excel (Prefix file là "LSP")
                TableExporter.ExportTableToExcel(dgvLoaiSanPham, "LSP");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Nhập Excel
        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Gọi Helper để đọc file và lấy List DTO
                List<LoaiSanPhamDTO> listNewData = ExcelHelper.ReadLoaiSanPhamFromExcel();

                if (listNewData != null && listNewData.Count > 0)
                {
                    // 2. Gọi BUS để thêm hàng loạt vào DB
                    int count = lspBUS.AddMany(listNewData);

                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} loại sản phẩm!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại Grid
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được dữ liệu nào (Có thể do lỗi DB hoặc dữ liệu trống).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhập Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}