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
            dgvNhaCungCap.AutoGenerateColumns = false; // Quan trọng: Chặn tự sinh cột

            // 1. Mã NCC
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MNCC",
                DataPropertyName = "MNCC",
                HeaderText = "Mã NCC",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 2. Tên NCC
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên nhà cung cấp",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Tự động giãn
            });

            // 3. Địa chỉ
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DIACHI",
                DataPropertyName = "DIACHI",
                HeaderText = "Địa chỉ",
                Width = 200
            });

            // 4. Số điện thoại
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "Số điện thoại",
                Width = 120
            });

            // 5. Email
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 150
            });

            // 6. Cột ẩn (Trạng thái)
            dgvNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                Visible = false
            });
        }

        private void FormatDataGridView()
        {
            if (dgvNhaCungCap.Columns.Count == 0) return;

            if (dgvNhaCungCap.Columns.Contains("MNCC"))
            {
                dgvNhaCungCap.Columns["MNCC"].HeaderText = "Mã NCC";
                dgvNhaCungCap.Columns["MNCC"].Width = 80;
                dgvNhaCungCap.Columns["MNCC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvNhaCungCap.Columns.Contains("TEN"))
            {
                dgvNhaCungCap.Columns["TEN"].HeaderText = "Tên nhà cung cấp";
                dgvNhaCungCap.Columns["TEN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvNhaCungCap.Columns.Contains("DIACHI"))
            {
                dgvNhaCungCap.Columns["DIACHI"].HeaderText = "Địa chỉ";
                dgvNhaCungCap.Columns["DIACHI"].Width = 200;
            }

            if (dgvNhaCungCap.Columns.Contains("SDT"))
            {
                dgvNhaCungCap.Columns["SDT"].HeaderText = "Số điện thoại";
                dgvNhaCungCap.Columns["SDT"].Width = 120;
            }

            if (dgvNhaCungCap.Columns.Contains("EMAIL"))
            {
                dgvNhaCungCap.Columns["EMAIL"].HeaderText = "Email";
                dgvNhaCungCap.Columns["EMAIL"].Width = 150;
            }

            // Ẩn cột Trạng thái
            if (dgvNhaCungCap.Columns.Contains("TT"))
            {
                dgvNhaCungCap.Columns["TT"].Visible = false;
            }
        }

        private void LoadData()
        {
            try
            {
                var list = nhaCungCapBUS.GetAll();
                if (list == null) list = new System.Collections.Generic.List<NhaCungCapDTO>();

                dgvNhaCungCap.DataSource = new System.ComponentModel.BindingList<NhaCungCapDTO>(list);
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
            
            dgvNhaCungCap.DataSource = null;
            dgvNhaCungCap.DataSource = new BindingList<NhaCungCapDTO>(result);
            
            FormatDataGridView(); // Định dạng lại sau khi tìm kiếm
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTimKiem.SelectedIndex = 0;
            LoadData();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi Helper đọc file
                List<NhaCungCapDTO> listNewData = ExcelHelper.ReadNhaCungCapFromExcel();

                if (listNewData != null && listNewData.Count > 0)
                {
                    // Gọi BUS thêm hàng loạt
                    int count = nhaCungCapBUS.AddMany(listNewData);

                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} nhà cung cấp!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại grid
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được dữ liệu nào (Có thể do lỗi DB).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhập Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearForm()
        {
            txtMaNCC.Text = nhaCungCapBUS.getAutoIncrement().ToString();
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            currentMaNCC = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;

            // Nhóm nút chức năng chính (Enable/Disable)
            btnThem.Enabled = !editing && SessionManager.CanCreate("nhacungcap");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("nhacungcap");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("nhacungcap");

            // Nhóm nút Lưu/Hủy (Ẩn/Hiện - Visible)
            btnLuu.Visible = editing;
            btnHuy.Visible = editing;

            // Khóa bảng khi đang sửa
            dgvNhaCungCap.Enabled = !editing;

            // Trạng thái các ô nhập liệu
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
            // 1. Quay về chế độ xem
            SetButtonStates(false);

            // 2. Select lại dòng đầu tiên (Item 1)
            if (dgvNhaCungCap.Rows.Count > 0)
            {
                dgvNhaCungCap.ClearSelection();
                dgvNhaCungCap.Rows[0].Selected = true;
                
                // Đặt current cell về dòng đầu để focus chuẩn (tránh lỗi focus ảo)
                dgvNhaCungCap.CurrentCell = dgvNhaCungCap.Rows[0].Cells[0];
                
                // Gọi hàm hiển thị thông tin để load dữ liệu từ grid lên textbox
                DisplayNhaCungCapInfo();
            }
            else
            {
                ClearForm();
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhaCungCap.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                TableExporter.ExportTableToExcel(dgvNhaCungCap, "NCC");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
