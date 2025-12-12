using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.Helper;
using src.GUI.Components;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyNhaCungCapForm : Form
    {
        private NhaCungCapBUS nhaCungCapBUS;
        private bool isEditing = false;
        private int currentMaNCC = -1;

        public QuanLyNhaCungCapForm()
        {
            try
            {
                InitializeComponent();
                nhaCungCapBUS = new NhaCungCapBUS();
                InitializeDataGridView();
                LoadData();
                SetButtonStates(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridView()
        {
            dgvNhaCungCap.Columns.Clear();

            // Column MNCC
            DataGridViewTextBoxColumn colMNCC = new DataGridViewTextBoxColumn
            {
                Name = "MNCC",
                DataPropertyName = "MNCC",
                HeaderText = "Mã NCC",
                Width = 80
            };
            dgvNhaCungCap.Columns.Add(colMNCC);

            // Column TEN
            DataGridViewTextBoxColumn colTEN = new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên nhà cung cấp",
                Width = 200
            };
            dgvNhaCungCap.Columns.Add(colTEN);

            // Column DIACHI
            DataGridViewTextBoxColumn colDIACHI = new DataGridViewTextBoxColumn
            {
                Name = "DIACHI",
                DataPropertyName = "DIACHI",
                HeaderText = "Địa chỉ",
                Width = 200
            };
            dgvNhaCungCap.Columns.Add(colDIACHI);

            // Column SDT
            DataGridViewTextBoxColumn colSDT = new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "Số điện thoại",
                Width = 110
            };
            dgvNhaCungCap.Columns.Add(colSDT);

            // Column EMAIL
            DataGridViewTextBoxColumn colEMAIL = new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 150
            };
            dgvNhaCungCap.Columns.Add(colEMAIL);

            // Column TT
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 100
            };
            dgvNhaCungCap.Columns.Add(colTT);

            // Style header
            dgvNhaCungCap.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvNhaCungCap.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNhaCungCap.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvNhaCungCap.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void LoadData()
        {
            try
            {
                var nhaCungCapList = nhaCungCapBUS.GetAll();
                
                if (nhaCungCapList != null)
                {
                    dgvNhaCungCap.DataSource = new BindingList<NhaCungCapDTO>(nhaCungCapList);
                }
                else
                {
                    dgvNhaCungCap.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNhaCungCap_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow != null && !isEditing)
            {
                DisplayNhaCungCapInfo();
            }
        }

        private void DisplayNhaCungCapInfo()
        {
            try
            {
                if (dgvNhaCungCap.CurrentRow == null) return;

                var row = dgvNhaCungCap.CurrentRow;
                txtMaNCC.Text = row.Cells["MNCC"].Value?.ToString() ?? "";
                txtTenNCC.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                txtDiaChi.Text = row.Cells["DIACHI"].Value?.ToString() ?? "";
                txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["EMAIL"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                // Silent catch - không hiển thị lỗi cho user khi display info
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string searchType = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

            var result = nhaCungCapBUS.Search(keyword, searchType);
            dgvNhaCungCap.DataSource = new BindingList<NhaCungCapDTO>(result);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTimKiem.SelectedIndex = 0;
            LoadData();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                TableExporter.ExportTableToExcel(dgvNhaCungCap, "NCC");
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            currentMaNCC = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("nhacungcap");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("nhacungcap");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("nhacungcap");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvNhaCungCap.Enabled = !editing;

            txtTenNCC.ReadOnly = !editing;
            txtDiaChi.ReadOnly = !editing;
            txtSDT.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            currentMaNCC = -1;
            txtTenNCC.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow != null)
            {
                currentMaNCC = int.Parse(txtMaNCC.Text);
                SetButtonStates(true);
                txtTenNCC.Focus();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", 
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int index = dgvNhaCungCap.CurrentRow.Index;
                        NhaCungCapDTO ncc = nhaCungCapBUS.GetByIndex(index);
                        
                        if (nhaCungCapBUS.Delete(ncc, index))
                        {
                            MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (!Validation.IsPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!Validation.IsEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO
                {
                    MNCC = currentMaNCC,
                    TEN = txtTenNCC.Text.Trim(),
                    DIACHI = txtDiaChi.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    TT = 1
                };

                bool success;
                if (currentMaNCC == -1) // Thêm mới
                {
                    success = nhaCungCapBUS.Add(ncc);
                    if (success)
                    {
                        MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Cập nhật
                {
                    success = nhaCungCapBUS.Update(ncc);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (success)
                {
                    LoadData();
                    SetButtonStates(false);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dgvNhaCungCap.CurrentRow != null)
            {
                DisplayNhaCungCapInfo();
            }
            else
            {
                ClearForm();
            }
        }
    }
}
