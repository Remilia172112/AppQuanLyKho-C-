using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.Helper;
using src.GUI.Components;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyKhachHangForm : Form
    {
        private KhachHangBUS khachHangBUS;
        private DataGridView dgvKhachHang;
        private TextBox txtMaKH, txtHoTen, txtDiaChi, txtSDT, txtEmail, txtTimKiem;
        private ComboBox cboTimKiem;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnExport;
        private bool isEditing = false;
        private int currentMaKH = -1;

        public QuanLyKhachHangForm()
        {
            try
            {
                InitializeComponent();
                khachHangBUS = new KhachHangBUS();
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

        // QUAN TRỌNG: Tạo columns THỦ CÔNG TRƯỚC KHI LOAD DATA
        private void InitializeDataGridView()
        {
            dgvKhachHang.Columns.Clear();

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MKH",
                DataPropertyName = "MKH",
                HeaderText = "Mã KH",
                Width = 70,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HOTEN",
                DataPropertyName = "HOTEN",
                HeaderText = "Họ tên",
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NGAYTHAMGIA",
                DataPropertyName = "NGAYTHAMGIA",
                HeaderText = "Ngày tham gia",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DIACHI",
                DataPropertyName = "DIACHI",
                HeaderText = "Địa chỉ",
                Width = 160,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "SĐT",
                Width = 110,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 90,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });
        }

        // LOAD DATA AN TOÀN - Columns đã được tạo sẵn
        private void LoadData()
        {
            try
            {
                var khachHangList = khachHangBUS.GetAll();
                
                if (khachHangList == null)
                {
                    khachHangList = new List<KhachHangDTO>();
                }

                // Bind data - Columns đã tạo sẵn nên KHÔNG CÒN LỖI
                dgvKhachHang.DataSource = new BindingList<KhachHangDTO>(khachHangList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}\n\nChi tiết: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvKhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow != null && !isEditing)
            {
                DisplayKhachHangInfo();
            }
        }

        private void DisplayKhachHangInfo()
        {
            try
            {
                if (dgvKhachHang.CurrentRow != null)
                {
                    var row = dgvKhachHang.CurrentRow;
                    txtMaKH.Text = row.Cells["MKH"].Value?.ToString() ?? "";
                    txtHoTen.Text = row.Cells["HOTEN"].Value?.ToString() ?? "";
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
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            currentMaKH = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("khachhang");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("khachhang");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("khachhang");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvKhachHang.Enabled = !editing;
            
            txtHoTen.ReadOnly = !editing;
            txtDiaChi.ReadOnly = !editing;
            txtSDT.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtMaKH.Text = "(Tự động)";
            txtHoTen.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow != null)
            {
                currentMaKH = Convert.ToInt32(dgvKhachHang.CurrentRow.Cells["MKH"].Value);
                SetButtonStates(true);
                txtHoTen.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maKH = Convert.ToInt32(dgvKhachHang.CurrentRow.Cells["MKH"].Value);
                        var khachHang = khachHangBUS.GetAll().Find(x => x.MKH == maKH);
                        if (khachHang != null && khachHangBUS.Delete(khachHang))
                        {
                            MessageBox.Show("Xóa khách hàng thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa khách hàng thất bại!", "Lỗi", 
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
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
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
                var khachHang = new KhachHangDTO
                {
                    HOTEN = txtHoTen.Text.Trim(),
                    NGAYTHAMGIA = DateTime.Now,
                    DIACHI = txtDiaChi.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    TT = 1
                };

                if (currentMaKH == -1) // Thêm mới
                {
                    if (khachHangBUS.Add(khachHang))
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Thêm khách hàng thất bại!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Cập nhật
                {
                    khachHang.MKH = currentMaKH;
                    if (khachHangBUS.Update(khachHang))
                    {
                        MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khách hàng thất bại!", "Lỗi", 
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
            if (dgvKhachHang.CurrentRow != null)
            {
                DisplayKhachHangInfo();
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
                var result = khachHangBUS.Search(keyword, searchType);
                dgvKhachHang.DataSource = new BindingList<KhachHangDTO>(result);
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
                if (dgvKhachHang.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                JTableExporter.ExportJTableToExcel(dgvKhachHang);
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
