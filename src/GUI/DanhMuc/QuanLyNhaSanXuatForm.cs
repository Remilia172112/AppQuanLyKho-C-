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
    public partial class QuanLyNhaSanXuatForm : Form
    {
        private NhaSanXuatBUS nhaSanXuatBUS;
        private bool isEditing = false;
        private int currentMaNSX = -1;

        public QuanLyNhaSanXuatForm()
        {
            try
            {
                InitializeComponent();
                nhaSanXuatBUS = new NhaSanXuatBUS();
                InitializeDataGridView(); // THÊM: Tạo columns THỦ CÔNG
                LoadData();
                SetButtonStates(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridView()
        {
            dgvNhaSanXuat.Columns.Clear();

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MNSX",
                DataPropertyName = "MNSX",
                HeaderText = "Mã NSX",
                Width = 80
            });

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên NSX",
                Width = 200
            });

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DIACHI",
                DataPropertyName = "DIACHI",
                HeaderText = "Địa chỉ",
                Width = 200
            });

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "Số điện thoại",
                Width = 110
            });

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 150
            });

            dgvNhaSanXuat.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 100
            });
        }

        // LOAD DATA AN TOÀN - Columns đã được tạo sẵn
        private void LoadData()
        {
            try
            {
                var nhaSanXuatList = nhaSanXuatBUS.GetAll();
                
                if (nhaSanXuatList == null)
                {
                    nhaSanXuatList = new List<NhaSanXuatDTO>();
                }

                // Bind data - Columns đã tạo sẵn nên KHÔNG CÒN LỖI
                dgvNhaSanXuat.DataSource = new BindingList<NhaSanXuatDTO>(nhaSanXuatList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}\n\nChi tiết: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNhaSanXuat_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNhaSanXuat.CurrentRow != null && !isEditing)
            {
                DisplayNhaSanXuatInfo();
            }
        }

        private void DisplayNhaSanXuatInfo()
        {
            try
            {
                if (dgvNhaSanXuat.CurrentRow != null)
                {
                    var row = dgvNhaSanXuat.CurrentRow;
                    txtMaNSX.Text = row.Cells["MNSX"].Value?.ToString() ?? "";
                    txtTenNSX.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                    txtDiaChi.Text = row.Cells["DIACHI"].Value?.ToString() ?? "";
                    txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";
                    txtEmail.Text = row.Cells["EMAIL"].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị thông tin: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtMaNSX.Clear();
            txtTenNSX.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            currentMaNSX = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("nhasanxuat");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("nhasanxuat");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("nhasanxuat");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvNhaSanXuat.Enabled = !editing;
            
            txtTenNSX.ReadOnly = !editing;
            txtDiaChi.ReadOnly = !editing;
            txtSDT.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtMaNSX.Text = "(Tự động)";
            txtTenNSX.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaSanXuat.CurrentRow != null)
            {
                currentMaNSX = Convert.ToInt32(dgvNhaSanXuat.CurrentRow.Cells["MNSX"].Value);
                SetButtonStates(true);
                txtTenNSX.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà sản xuất cần sửa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhaSanXuat.CurrentRow != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà sản xuất này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maNSX = Convert.ToInt32(dgvNhaSanXuat.CurrentRow.Cells["MNSX"].Value);
                        int index = dgvNhaSanXuat.CurrentRow.Index;
                        var nhaSanXuat = nhaSanXuatBUS.GetAll().Find(x => x.MNSX == maNSX);
                        if (nhaSanXuat != null && nhaSanXuatBUS.Delete(nhaSanXuat, index))
                        {
                            MessageBox.Show("Xóa nhà sản xuất thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhà sản xuất thất bại!", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà sản xuất cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTenNSX.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà sản xuất!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNSX.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (!Validation.IsPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!Validation.IsEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                var nhaSanXuat = new NhaSanXuatDTO
                {
                    TEN = txtTenNSX.Text.Trim(),
                    DIACHI = txtDiaChi.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    TT = 1
                };

                if (currentMaNSX == -1) // Thêm mới
                {
                    if (nhaSanXuatBUS.Add(nhaSanXuat))
                    {
                        MessageBox.Show("Thêm nhà sản xuất thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhà sản xuất thất bại!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Cập nhật
                {
                    nhaSanXuat.MNSX = currentMaNSX;
                    if (nhaSanXuatBUS.Update(nhaSanXuat))
                    {
                        MessageBox.Show("Cập nhật nhà sản xuất thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật nhà sản xuất thất bại!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetButtonStates(false);
            if (dgvNhaSanXuat.CurrentRow != null)
            {
                DisplayNhaSanXuatInfo();
            }
            else
            {
                ClearForm();
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string searchType = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

            try
            {
                var result = nhaSanXuatBUS.Search(keyword, searchType);
                dgvNhaSanXuat.DataSource = new BindingList<NhaSanXuatDTO>(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTimKiem.SelectedIndex = 0;
            LoadData();
            ClearForm();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhaSanXuat.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TableExporter.ExportTableToExcel(dgvNhaSanXuat);
                MessageBox.Show("Xuất file Excel thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
