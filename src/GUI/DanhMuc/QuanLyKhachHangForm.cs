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
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnExport, btnImport;
        private bool isEditing = false;
        private int currentMaKH = -1;

        public QuanLyKhachHangForm()
        {
            try
            {
                InitializeComponent();
                khachHangBUS = new KhachHangBUS();
                InitializeDataGridView();
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
            dgvKhachHang.Columns.Clear();
            dgvKhachHang.AutoGenerateColumns = false; // Quan trọng: Chặn tự sinh cột

            // 1. Mã Khách Hàng
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MKH",
                DataPropertyName = "MKH",
                HeaderText = "Mã KH",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 2. Họ Tên (Giãn tự động)
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HOTEN",
                DataPropertyName = "HOTEN",
                HeaderText = "Họ tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
            });

            // 3. Số Điện Thoại
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SDT",
                DataPropertyName = "SDT",
                HeaderText = "Số điện thoại",
                Width = 120
            });

            // 4. Địa Chỉ
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DIACHI",
                DataPropertyName = "DIACHI",
                HeaderText = "Địa chỉ",
                Width = 200
            });

            // 5. Ngày Tham Gia
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NGAYTHAMGIA",
                DataPropertyName = "NGAYTHAMGIA",
                HeaderText = "Ngày tham gia",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 6. Email (Hiện hoặc Ẩn tùy bạn, ở đây mình hiện để tiện tra cứu)
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EMAIL",
                DataPropertyName = "EMAIL",
                HeaderText = "Email",
                Width = 150,
                Visible = false
            });

            // 7. Cột Ẩn (Trạng thái)
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                Visible = false
            });
        }
        private void FormatDataGridView()
        {
            if (dgvKhachHang.Columns.Count == 0) return;

            // Cấu hình cột hiển thị
            if (dgvKhachHang.Columns.Contains("MKH"))
            {
                dgvKhachHang.Columns["MKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["MKH"].Width = 80;
                dgvKhachHang.Columns["MKH"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvKhachHang.Columns.Contains("HOTEN"))
            {
                dgvKhachHang.Columns["HOTEN"].HeaderText = "Họ tên";
                dgvKhachHang.Columns["HOTEN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvKhachHang.Columns.Contains("SDT"))
            {
                dgvKhachHang.Columns["SDT"].HeaderText = "Số điện thoại";
                dgvKhachHang.Columns["SDT"].Width = 120;
            }

            if (dgvKhachHang.Columns.Contains("DIACHI"))
            {
                dgvKhachHang.Columns["DIACHI"].HeaderText = "Địa chỉ";
                dgvKhachHang.Columns["DIACHI"].Width = 200;
            }

            if (dgvKhachHang.Columns.Contains("NGAYTHAMGIA"))
            {
                dgvKhachHang.Columns["NGAYTHAMGIA"].HeaderText = "Ngày tham gia";
                dgvKhachHang.Columns["NGAYTHAMGIA"].Width = 120;
                dgvKhachHang.Columns["NGAYTHAMGIA"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvKhachHang.Columns["NGAYTHAMGIA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Ẩn cột không cần thiết
            string[] hiddenCols = { "EMAIL", "TT" };
            foreach (var col in hiddenCols)
            {
                if (dgvKhachHang.Columns.Contains(col))
                    dgvKhachHang.Columns[col].Visible = false;
            }
        }

        // 2. Cập nhật hàm LoadData
        private void LoadData()
        {
            try
            {
                var listKhachHang = khachHangBUS.GetAll();

                if (listKhachHang == null)
                {
                    listKhachHang = new List<KhachHangDTO>();
                }

                // Gán dữ liệu vào Grid (Cột đã được tạo sẵn ở InitializeDataGridView)
                dgvKhachHang.DataSource = new BindingList<KhachHangDTO>(listKhachHang);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string searchType = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

                var result = khachHangBUS.Search(keyword, searchType);
                
                dgvKhachHang.DataSource = null;
                dgvKhachHang.DataSource = new BindingList<KhachHangDTO>(result);
                
                FormatDataGridView(); // Định dạng lại sau khi tìm kiếm
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                TableExporter.ExportTableToExcel(dgvKhachHang, "KH");
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
                // Gọi Helper đọc file
                List<KhachHangDTO> listNewData = ExcelHelper.ReadKhachHangFromExcel();

                if (listNewData != null && listNewData.Count > 0)
                {
                    // Gọi BUS thêm hàng loạt
                    int count = khachHangBUS.AddMany(listNewData);

                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} khách hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại grid
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được dòng nào (Có thể do lỗi dữ liệu).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
