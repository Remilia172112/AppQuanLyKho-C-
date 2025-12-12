using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using src.BUS;
using src.DTO;
using src.Helper;
using src.GUI.Components;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyTaiKhoanForm : Form
    {
        private TaiKhoanBUS taiKhoanBUS;
        private NhanVienBUS nhanVienBUS;
        private NhomQuyenBUS nhomQuyenBUS;
        private bool isEditing = false;
        private int currentMaNV = -1;

        public QuanLyTaiKhoanForm()
        {
            try
            {
                InitializeComponent();
                taiKhoanBUS = new TaiKhoanBUS();
                nhanVienBUS = new NhanVienBUS();
                nhomQuyenBUS = new NhomQuyenBUS();
                
                // Tạo cột trước
                InitializeDataGridView();
                
                LoadNhomQuyen();
                LoadNhanVien();
                
                // Load dữ liệu sau
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
            dgvTaiKhoan.Columns.Clear();
            dgvTaiKhoan.AutoGenerateColumns = false;

            // Mã NV
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MNV",
                DataPropertyName = "MNV",
                HeaderText = "Mã NV",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Tên đăng nhập
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TDN",
                DataPropertyName = "TDN",
                HeaderText = "Tên đăng nhập",
                Width = 200
            });

            // Mã nhóm quyền
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MNQ",
                DataPropertyName = "MNQ",
                HeaderText = "Mã nhóm quyền",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Ẩn cột Mật khẩu (Bảo mật)
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MK",
                DataPropertyName = "MK",
                Visible = false
            });

            // Ẩn cột Trạng thái
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                Visible = false
            });
            
            // Ẩn cột OTP
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OTP",
                DataPropertyName = "OTP",
                Visible = false
            });
        }

        private void LoadData()
        {
            try
            {
                var taiKhoanList = taiKhoanBUS.GetTaiKhoanAll();
                if (taiKhoanList == null) taiKhoanList = new System.Collections.Generic.List<TaiKhoanDTO>();

                dgvTaiKhoan.DataSource = null; // Reset để tránh lỗi
                dgvTaiKhoan.DataSource = new BindingList<TaiKhoanDTO>(taiKhoanList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadNhomQuyen()
        {
            try
            {
                var listNhomQuyen = nhomQuyenBUS.GetAll();
                cboNhomQuyen.Items.Clear();
                
                foreach (var nq in listNhomQuyen)
                {
                    cboNhomQuyen.Items.Add(new ComboBoxItem { Text = nq.Tennhomquyen, Value = nq.Manhomquyen });
                }
                
                if (cboNhomQuyen.Items.Count > 0)
                    cboNhomQuyen.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load nhóm quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNhanVien()
        {
            try
            {
                cboMaNV.Items.Clear();
                
                // Lấy danh sách tất cả nhân viên
                var allNV = nhanVienBUS.GetAll();
                
                // Lấy danh sách tài khoản hiện có
                var allTK = taiKhoanBUS.GetTaiKhoanAll();
                
                // Lọc nhân viên chưa có tài khoản
                var nvChuaCoTK = allNV.Where(nv => 
                    !allTK.Any(tk => tk.MNV == nv.MNV) && nv.TT == 1
                ).ToList();
                
                foreach (var nv in nvChuaCoTK)
                {
                    cboMaNV.Items.Add(new ComboBoxItem
                    {
                        Text = $"{nv.MNV} - {nv.HOTEN}",
                        Value = nv.MNV
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedItem != null && isEditing)
            {
                var item = (ComboBoxItem)cboMaNV.SelectedItem;
                int mnv = item.Value;
                
                var nv = nhanVienBUS.GetById(mnv);
                if (nv != null)
                {
                    txtTenNV.Text = nv.HOTEN;
                }
            }
        }


        private void DgvTaiKhoan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow != null && !isEditing)
            {
                DisplayTaiKhoanInfo();
            }
        }

        private void DisplayTaiKhoanInfo()
        {
            try
            {
                if (dgvTaiKhoan.CurrentRow == null) return;

                var row = dgvTaiKhoan.CurrentRow;
                int mnv = int.Parse(row.Cells["MNV"].Value?.ToString() ?? "0");
                currentMaNV = mnv;
                
                // Hiển thị thông tin nhân viên
                var nv = nhanVienBUS.GetById(mnv);
                if (nv != null)
                {
                    cboMaNV.Items.Clear();
                    cboMaNV.Items.Add(new ComboBoxItem
                    {
                        Text = $"{nv.MNV} - {nv.HOTEN}",
                        Value = nv.MNV
                    });
                    cboMaNV.SelectedIndex = 0;
                    txtTenNV.Text = nv.HOTEN;
                }
                
                txtTenDangNhap.Text = row.Cells["TDN"].Value?.ToString() ?? "";
                
                // Chọn nhóm quyền
                int mnq = int.Parse(row.Cells["MNQ"].Value?.ToString() ?? "0");
                for (int i = 0; i < cboNhomQuyen.Items.Count; i++)
                {
                    var item = (ComboBoxItem)cboNhomQuyen.Items[i];
                    if (item.Value == mnq)
                    {
                        cboNhomQuyen.SelectedIndex = i;
                        break;
                    }
                }
                
                // Clear password fields khi hiển thị
                txtMatKhau.Clear();
                txtXacNhanMK.Clear();
            }
            catch (Exception ex)
            {
                // Silent catch
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string searchType = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

            var result = taiKhoanBUS.Search(keyword, searchType);
            dgvTaiKhoan.DataSource = new BindingList<TaiKhoanDTO>(result);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTimKiem.SelectedIndex = 0;
            LoadData();
        }

        private void ClearForm()
        {
            LoadNhanVien(); // Reload danh sách nhân viên chưa có tài khoản
            txtTenNV.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtXacNhanMK.Clear();
            if (cboNhomQuyen.Items.Count > 0)
                cboNhomQuyen.SelectedIndex = 0;
            currentMaNV = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("taikhoan");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("taikhoan");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("taikhoan");
            btnResetMK.Enabled = !editing;
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvTaiKhoan.Enabled = !editing;

            cboMaNV.Enabled = editing && currentMaNV == -1; // Chỉ cho chọn khi thêm mới
            txtTenDangNhap.ReadOnly = !editing;
            txtMatKhau.ReadOnly = !editing;
            txtXacNhanMK.ReadOnly = !editing;
            cboNhomQuyen.Enabled = editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            currentMaNV = -1;
            cboMaNV.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow != null)
            {
                currentMaNV = int.Parse(dgvTaiKhoan.CurrentRow.Cells["MNV"].Value?.ToString() ?? "0");
                SetButtonStates(true);
                txtTenDangNhap.Focus();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", 
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int mnv = int.Parse(dgvTaiKhoan.CurrentRow.Cells["MNV"].Value?.ToString() ?? "0");
                        taiKhoanBUS.DeleteAcc(mnv);
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LoadNhanVien(); // Reload danh sách nhân viên
                        ClearForm();
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
            if (cboMaNV.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaNV.Focus();
                return;
            }

            var selectedNV = (ComboBoxItem)cboMaNV.SelectedItem;
            int mnv = selectedNV.Value;

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            if (currentMaNV == -1) // Thêm mới - cần mật khẩu
            {
                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text != txtXacNhanMK.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMK.Focus();
                    return;
                }
            }

            try
            {

                // Lấy mã nhóm quyền
                var selectedItem = (ComboBoxItem)cboNhomQuyen.SelectedItem;
                int mnq = selectedItem.Value;

                TaiKhoanDTO tk = new TaiKhoanDTO
                {
                    MNV = mnv,
                    TDN = txtTenDangNhap.Text.Trim(),
                    MNQ = mnq,
                    TT = 1
                };

                bool success;
                if (currentMaNV == -1) // Thêm mới
                {
                    // Kiểm tra tên đăng nhập đã tồn tại
                    if (!taiKhoanBUS.CheckTDN(tk.TDN))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenDangNhap.Focus();
                        return;
                    }

                    // Gán mật khẩu plain text - BUS sẽ tự mã hóa
                    tk.MK = txtMatKhau.Text;
                    
                    taiKhoanBUS.AddAcc(tk);
                    success = true;
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Cập nhật
                {
                    // Giữ nguyên mật khẩu cũ nếu không nhập mật khẩu mới
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        // Lấy mật khẩu cũ từ database
                        int index = taiKhoanBUS.GetTaiKhoanByMaNV(currentMaNV);
                        if (index != -1)
                        {
                            var oldTk = taiKhoanBUS.GetTaiKhoan(index);
                            tk.MK = oldTk.MK;
                        }
                    }
                    else
                    {
                        // Có nhập mật khẩu mới
                        if (txtMatKhau.Text.Length < 6)
                        {
                            MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMatKhau.Focus();
                            return;
                        }

                        if (txtMatKhau.Text != txtXacNhanMK.Text)
                        {
                            MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtXacNhanMK.Focus();
                            return;
                        }

                        // Gán mật khẩu plain text - BUS sẽ kiểm tra và mã hóa
                        tk.MK = txtMatKhau.Text;
                    }
                    
                    taiKhoanBUS.UpdateAcc(tk);
                    success = true;
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (success)
                {
                    LoadData();
                    SetButtonStates(false);
                    ClearForm();
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
            LoadNhanVien(); // Reload lại danh sách nhân viên
            if (dgvTaiKhoan.CurrentRow != null)
            {
                DisplayTaiKhoanInfo();
            }
            else
            {
                ClearForm();
            }
        }

        private void BtnResetMK_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần reset mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn reset mật khẩu về '123456'?", 
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int mnv = int.Parse(dgvTaiKhoan.CurrentRow.Cells["MNV"].Value?.ToString() ?? "0");
                    int index = taiKhoanBUS.GetTaiKhoanByMaNV(mnv);
                    
                    if (index != -1)
                    {
                        var tk = taiKhoanBUS.GetTaiKhoan(index);
                        // Gán mật khẩu plain text - BUS sẽ tự mã hóa
                        tk.MK = "123456";
                        taiKhoanBUS.UpdateAcc(tk);
                        
                        MessageBox.Show("Reset mật khẩu thành công!\nMật khẩu mới: 123456", 
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Helper class cho ComboBox
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTaiKhoan.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Gọi Helper xuất Excel với Prefix "TK"
                TableExporter.ExportTableToExcel(dgvTaiKhoan, "TK");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
