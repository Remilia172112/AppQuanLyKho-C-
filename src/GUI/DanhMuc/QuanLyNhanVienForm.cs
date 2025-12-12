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
    public partial class QuanLyNhanVienForm : Form
    {
        private NhanVienBUS nhanVienBUS;
        private bool isEditing = false;
        private int currentMaNV = -1;

        public QuanLyNhanVienForm()
        {
            try
            {
                InitializeComponent();
                nhanVienBUS = new NhanVienBUS();
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
            dgvNhanVien.Columns.Clear();

            // Column MNV
            DataGridViewTextBoxColumn colMNV = new DataGridViewTextBoxColumn
            {
                Name = "MNV",
                DataPropertyName = "MNV",
                HeaderText = "Mã NV",
                Width = 70
            };
            dgvNhanVien.Columns.Add(colMNV);

            // Column HOTEN
            DataGridViewTextBoxColumn colHOTEN = new DataGridViewTextBoxColumn
            {
                Name = "HOTEN",
                DataPropertyName = "HOTEN",
                HeaderText = "Họ tên",
                Width = 180
            };
            dgvNhanVien.Columns.Add(colHOTEN);

            // Column GIOITINH
            DataGridViewTextBoxColumn colGIOITINH = new DataGridViewTextBoxColumn
            {
                Name = "GIOITINH",
                DataPropertyName = "GIOITINH",
                HeaderText = "Giới tính",
                Width = 80
            };
            dgvNhanVien.Columns.Add(colGIOITINH);

            // Column NGAYSINH
            DataGridViewTextBoxColumn colNGAYSINH = new DataGridViewTextBoxColumn
            {
                Name = "NGAYSINH",
                DataPropertyName = "NGAYSINH",
                HeaderText = "Ngày sinh",
                Width = 120
            };
            colNGAYSINH.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvNhanVien.Columns.Add(colNGAYSINH);

            // Column SDT
            DataGridViewTextBoxColumn colSDT = new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "Số điện thoại",
                Width = 120
            };
            dgvNhanVien.Columns.Add(colSDT);

            // Column EMAIL
            DataGridViewTextBoxColumn colEMAIL = new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 180
            };
            dgvNhanVien.Columns.Add(colEMAIL);

            // Column TT
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 90
            };
            dgvNhanVien.Columns.Add(colTT);

            // Style header
            dgvNhanVien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvNhanVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvNhanVien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void LoadData()
        {
            try
            {
                var nhanVienList = nhanVienBUS.GetAll();
                
                if (nhanVienList != null)
                {
                    dgvNhanVien.DataSource = new BindingList<NhanVienDTO>(nhanVienList);
                }
                else
                {
                    dgvNhanVien.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null && !isEditing)
            {
                DisplayNhanVienInfo();
            }
        }

        private void DisplayNhanVienInfo()
        {
            try
            {
                if (dgvNhanVien.CurrentRow == null) return;

                var row = dgvNhanVien.CurrentRow;
                txtMaNV.Text = row.Cells["MNV"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["HOTEN"].Value?.ToString() ?? "";
                
                int gioiTinh = int.Parse(row.Cells["GIOITINH"].Value?.ToString() ?? "1");
                cboGioiTinh.SelectedIndex = gioiTinh == 1 ? 0 : 1;
                
                if (row.Cells["NGAYSINH"].Value != null && DateTime.TryParse(row.Cells["NGAYSINH"].Value.ToString(), out DateTime ngaySinh))
                {
                    dtpNgaySinh.Value = ngaySinh;
                }
                
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

            var result = nhanVienBUS.Search(keyword, searchType);
            dgvNhanVien.DataSource = new BindingList<NhanVienDTO>(result);
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
                if (dgvNhanVien.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TableExporter.ExportTableToExcel(dgvNhanVien,"NV");
                MessageBox.Show("Xuất file Excel thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            cboGioiTinh.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Now.AddYears(-20);
            currentMaNV = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("nhanvien");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("nhanvien");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("nhanvien");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvNhanVien.Enabled = !editing;

            txtHoTen.ReadOnly = !editing;
            txtSDT.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
            cboGioiTinh.Enabled = editing;
            dtpNgaySinh.Enabled = editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            currentMaNV = -1;
            txtHoTen.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                currentMaNV = int.Parse(txtMaNV.Text);
                SetButtonStates(true);
                txtHoTen.Focus();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?\nLưu ý: Tài khoản liên quan cũng sẽ bị xóa!", 
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int index = dgvNhanVien.CurrentRow.Index;
                        NhanVienDTO nv = nhanVienBUS.GetByIndex(index);
                        
                        if (nhanVienBUS.DeleteNv(nv))
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
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

            // Kiểm tra tuổi (phải >= 18 tuổi)
            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (tuoi < 18)
            {
                MessageBox.Show("Nhân viên phải từ 18 tuổi trở lên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    MNV = currentMaNV,
                    HOTEN = txtHoTen.Text.Trim(),
                    GIOITINH = cboGioiTinh.SelectedIndex == 0 ? 1 : 0,
                    NGAYSINH = dtpNgaySinh.Value,
                    SDT = txtSDT.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    TT = 1
                };

                bool success;
                if (currentMaNV == -1) // Thêm mới
                {
                    success = nhanVienBUS.Add(nv);
                    
                    if (success)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Cập nhật
                {
                    success = nhanVienBUS.Update(nv);
                    
                    if (success)
                    {
                        MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dgvNhanVien.CurrentRow != null)
            {
                DisplayNhanVienInfo();
            }
            else
            {
                ClearForm();
            }
        }
    }
}
