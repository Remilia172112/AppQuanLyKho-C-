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
            InitializeDataGridView();
            LoadData();
            SetButtonStates(false);
            CheckPermissions();
        }

        private void CheckPermissions()
        {
            btnThem.Enabled = SessionManager.CanCreate("loaisanpham");
            btnSua.Enabled = SessionManager.CanUpdate("loaisanpham");
            btnXoa.Enabled = SessionManager.CanDelete("loaisanpham");
        }

        private void InitializeDataGridView()
        {
            dgvLoaiSanPham.Columns.Clear();
            dgvLoaiSanPham.AutoGenerateColumns = false; 

            // 1. Mã Loại Sản Phẩm
            dgvLoaiSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MLSP",
                DataPropertyName = "MLSP",
                HeaderText = "Mã Loại",
                Width = 5,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 2. Tên Loại Sản Phẩm
            dgvLoaiSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên Loại Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // 3. Tỉ Lệ Giá Xuất (MỚI)
            dgvLoaiSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TLGX",
                DataPropertyName = "TLGX",
                HeaderText = "Tỉ Lệ GX (%)",
                Width = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 4. Ghi Chú
            dgvLoaiSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GHICHU",
                DataPropertyName = "GHICHU",
                HeaderText = "Ghi Chú",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // 5. Trạng Thái (Ẩn)
            dgvLoaiSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                Visible = false
            });
        }

        private void LoadData()
        {
            try
            {
                dgvLoaiSanPham.DataSource = lspBUS.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        // Sự kiện khi click vào bảng -> Hiển thị lên Form
        private void DgvLoaiSanPham_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra dòng hiện tại khác null và không đang ở chế độ thêm/sửa
            if (dgvLoaiSanPham.CurrentRow != null && !isEditing)
            {
                var row = dgvLoaiSanPham.CurrentRow;
                
                // Gán dữ liệu từ Grid lên các TextBox
                txtMaLSP.Text = row.Cells["MLSP"].Value?.ToString() ?? "";
                txtTenLSP.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                txtTLGX.Text = row.Cells["TLGX"].Value?.ToString() ?? "0"; // <--- Cột Tỉ lệ giá xuất
                txtGhiChu.Text = row.Cells["GHICHU"].Value?.ToString() ?? "";
            }
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            
            btnThem.Enabled = !editing && SessionManager.CanCreate("loaisanpham");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("loaisanpham");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("loaisanpham");
            
            btnLuu.Visible = editing;
            btnHuy.Visible = editing;
            
            txtTenLSP.ReadOnly = !editing;
            txtTLGX.ReadOnly = !editing; // <--- MỚI
            txtGhiChu.ReadOnly = !editing;
            
            dgvLoaiSanPham.Enabled = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            currentID = -1; 
            txtMaLSP.Text = lspBUS.getAutoIncrement().ToString();
            txtTenLSP.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvLoaiSanPham.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm cần sửa!");
                return;
            }
            currentID = int.Parse(txtMaLSP.Text);
            SetButtonStates(true);
            txtTenLSP.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLoaiSanPham.CurrentRow == null) return;
            
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int id = int.Parse(txtMaLSP.Text);
                if (lspBUS.Delete(id))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string ten = txtTenLSP.Text.Trim();
            string ghichu = txtGhiChu.Text.Trim();
            
            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm!");
                return;
            }

            // Validate TLGX
            if (!int.TryParse(txtTLGX.Text, out int tlgx) || tlgx < 0)
            {
                MessageBox.Show("Vui lòng nhập tỉ lệ giá xuất hợp lệ (số nguyên >= 0)!");
                return;
            }

            if (currentID == -1) // Thêm mới
            {
                if (lspBUS.IsTenExists(ten))
                {
                    MessageBox.Show("Tên loại sản phẩm đã tồn tại!");
                    return;
                }

                if (lspBUS.Add(ten, tlgx, ghichu)) // <--- MỚI: Truyền tlgx
                {
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                    SetButtonStates(false);
                    ClearForm();
                }
            }
            else // Cập nhật
            {
                if (lspBUS.IsTenExists(ten, currentID))
                {
                    MessageBox.Show("Tên loại sản phẩm đã tồn tại!");
                    return;
                }

                if (lspBUS.Update(currentID, ten, tlgx, ghichu)) // <--- MỚI: Truyền tlgx
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData();
                    SetButtonStates(false);
                    ClearForm();
                }
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetButtonStates(false);
            if (dgvLoaiSanPham.Rows.Count > 0)
            {
                dgvLoaiSanPham.ClearSelection();
                dgvLoaiSanPham.Rows[0].Selected = true;
                dgvLoaiSanPham.CurrentCell = dgvLoaiSanPham.Rows[0].Cells[0];
                DgvLoaiSanPham_SelectionChanged(sender, e);
            }
            else
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtMaLSP.Text = "";
            txtTenLSP.Text = "";
            txtTLGX.Text = "0"; // <--- MỚI
            txtGhiChu.Text = "";
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            dgvLoaiSanPham.DataSource = lspBUS.Search(kw);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadData();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLoaiSanPham.Rows.Count == 0) return;
                TableExporter.ExportTableToExcel(dgvLoaiSanPham, "LSP");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                List<LoaiSanPhamDTO> listNewData = ExcelHelper.ReadLoaiSanPhamFromExcel();
                if (listNewData != null && listNewData.Count > 0)
                {
                    int count = lspBUS.AddMany(listNewData);
                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} loại sản phẩm!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được dữ liệu nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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