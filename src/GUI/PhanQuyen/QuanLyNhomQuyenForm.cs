using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DAO;
using src.DTO;

namespace src.GUI.PhanQuyen
{
    public partial class QuanLyNhomQuyenForm : Form
    {
        private NhomQuyenBUS nhomQuyenBUS;
        private DanhMucChucNangDAO chucNangDAO;
        
        // Trạng thái form
        private enum FormMode { View, Add, Edit }
        private FormMode currentMode = FormMode.View;

        public QuanLyNhomQuyenForm()
        {
            nhomQuyenBUS = new NhomQuyenBUS();
            chucNangDAO = DanhMucChucNangDAO.Instance;
            checkBoxQuyen = new Dictionary<string, Dictionary<string, CheckBox>>();

            InitializeComponent();
            LoadNhomQuyen();
            SetFormMode(FormMode.View);

            // Đặt lại SplitterDistance theo tỉ lệ khi khởi tạo
            if (mainSplit != null)
            {
                mainSplit.SplitterDistance = (int)(this.Width * 0.45);
            }

            // Xử lý tự động chia lại khi resize
            this.Resize += (s, e) =>
            {
                if (mainSplit != null)
                {
                    mainSplit.SplitterDistance = (int)(this.Width * 0.45);
                }
            };
        }

        private void InitializeDataGridView()
        {
            dgvNhomQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaNhomQuyen",
                HeaderText = "Mã nhóm quyền",
                DataPropertyName = "Manhomquyen",
                Width = 120
            });

            dgvNhomQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNhomQuyen",
                HeaderText = "Tên nhóm quyền",
                DataPropertyName = "Tennhomquyen",
                Width = 360,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Styling
            dgvNhomQuyen.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            dgvNhomQuyen.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNhomQuyen.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvNhomQuyen.EnableHeadersVisualStyles = false;
        }

        private void InitializePermissionPanel()
        {
            var listChucNang = chucNangDAO.SelectAll();
            string[] actions = { "view", "create", "update", "delete", "approve" };
            string[] actionLabels = { "Xem", "Thêm", "Sửa", "Xóa", "Duyệt" };

            int yPos = 15;

            foreach (var cn in listChucNang)
            {
                // Group box cho mỗi chức năng
                GroupBox grpChucNang = new GroupBox
                {
                    Text = cn.TEN,
                    Location = new Point(15, yPos),
                    Size = new Size(600, 60), // Tăng chiều rộng để chứa checkbox "Duyệt"
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 73, 94)
                };

                var actionDict = new Dictionary<string, CheckBox>();

                for (int i = 0; i < actions.Length; i++)
                {
                    // Chỉ hiển thị checkbox "Duyệt" cho nhaphang, xuathang, kiemke
                    if (actions[i] == "approve" && 
                        cn.MCN != "nhaphang" && 
                        cn.MCN != "xuathang" && 
                        cn.MCN != "kiemke")
                    {
                        continue; // Bỏ qua checkbox "Duyệt" cho các chức năng khác
                    }

                    CheckBox chk = new CheckBox
                    {
                        Text = actionLabels[i],
                        Tag = $"{cn.MCN}_{actions[i]}",
                        Location = new Point(20 + (i * 120), 25), // Các checkbox gần nhau hơn
                        Size = new Size(100, 25),
                        Font = new Font("Segoe UI", 9),
                        Enabled = false // Disabled khi View mode
                    };

                    actionDict[actions[i]] = chk;
                    grpChucNang.Controls.Add(chk);
                }

                checkBoxQuyen[cn.MCN] = actionDict;
                pnlChucNang.Controls.Add(grpChucNang);
                yPos += 70;
            }
        }

        private void LoadNhomQuyen()
        {
            try
            {
                dgvNhomQuyen.DataSource = null;
                dgvNhomQuyen.DataSource = nhomQuyenBUS.GetAll();
                
                if (dgvNhomQuyen.Rows.Count > 0)
                {
                    dgvNhomQuyen.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNhomQuyen_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvNhomQuyen.CurrentRow != null && currentMode == FormMode.View)
            {
                var selectedRow = dgvNhomQuyen.CurrentRow;
                txtMaNhomQuyen.Text = selectedRow.Cells["MaNhomQuyen"].Value?.ToString() ?? "";
                txtTenNhomQuyen.Text = selectedRow.Cells["TenNhomQuyen"].Value?.ToString() ?? "";

                // Load chi tiết quyền
                if (int.TryParse(txtMaNhomQuyen.Text, out int maNhomQuyen))
                {
                    LoadChiTietQuyen(maNhomQuyen);
                }
            }
        }

        private void LoadChiTietQuyen(int maNhomQuyen)
        {
            try
            {
                // Reset tất cả checkbox
                foreach (var chucNang in checkBoxQuyen.Values)
                {
                    foreach (var cb in chucNang.Values)
                    {
                        cb.Checked = false;
                    }
                }

                // Lấy danh sách quyền từ BUS
                var chiTietQuyen = nhomQuyenBUS.GetChiTietQuyen(maNhomQuyen);

                // Check các checkbox tương ứng
                foreach (var quyen in chiTietQuyen)
                {
                    if (checkBoxQuyen.ContainsKey(quyen.Machucnang))
                    {
                        if (checkBoxQuyen[quyen.Machucnang].ContainsKey(quyen.Hanhdong))
                        {
                            checkBoxQuyen[quyen.Machucnang][quyen.Hanhdong].Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetFormMode(FormMode mode)
        {
            currentMode = mode;

            switch (mode)
            {
                case FormMode.View:
                    txtTenNhomQuyen.ReadOnly = true;
                    txtTenNhomQuyen.BackColor = Color.White;
                    dgvNhomQuyen.Enabled = true;
                    
                    btnThem.Enabled = true;
                    btnSua.Enabled = dgvNhomQuyen.Rows.Count > 0;
                    btnXoa.Enabled = dgvNhomQuyen.Rows.Count > 0;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;
                    btnTimKiem.Enabled = true;
                    btnRefresh.Enabled = true;

                    // Disable checkboxes
                    foreach (var dict in checkBoxQuyen.Values)
                    {
                        foreach (var cb in dict.Values)
                        {
                            cb.Enabled = false;
                        }
                    }
                    break;

                case FormMode.Add:
                case FormMode.Edit:
                    txtTenNhomQuyen.ReadOnly = false;
                    txtTenNhomQuyen.BackColor = Color.White;
                    dgvNhomQuyen.Enabled = false;
                    
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnTimKiem.Enabled = false;
                    btnRefresh.Enabled = false;

                    // Enable checkboxes
                    foreach (var dict in checkBoxQuyen.Values)
                    {
                        foreach (var cb in dict.Values)
                        {
                            cb.Enabled = true;
                        }
                    }
                    break;
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
            txtMaNhomQuyen.Text = nhomQuyenBUS.GetAll().Count > 0 
                ? (nhomQuyenBUS.GetAll().Max(x => x.Manhomquyen) + 1).ToString()
                : "1";
            txtTenNhomQuyen.Text = "";
            
            // Uncheck tất cả
            foreach (var dict in checkBoxQuyen.Values)
            {
                foreach (var cb in dict.Values)
                {
                    cb.Checked = false;
                }
            }
            
            txtTenNhomQuyen.Focus();
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dgvNhomQuyen.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetFormMode(FormMode.Edit);
            txtTenNhomQuyen.Focus();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvNhomQuyen.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa nhóm quyền '{txtTenNhomQuyen.Text}'?\n\nLưu ý: Các tài khoản thuộc nhóm này sẽ bị ảnh hưởng!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maNhomQuyen = int.Parse(txtMaNhomQuyen.Text);
                    var nhomQuyen = new NhomQuyenDTO(maNhomQuyen, txtTenNhomQuyen.Text);
                    
                    bool success = nhomQuyenBUS.Delete(nhomQuyen);
                    
                    if (success)
                    {
                        MessageBox.Show("Xóa nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNhomQuyen();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa nhóm quyền thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa nhóm quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtTenNhomQuyen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhóm quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenNhomQuyen.Focus();
                    return;
                }

                // Lấy danh sách quyền được check
                List<ChiTietQuyenDTO> chiTietQuyen = new List<ChiTietQuyenDTO>();
                
                foreach (var kvp in checkBoxQuyen)
                {
                    string machucnang = kvp.Key;
                    foreach (var actionKvp in kvp.Value)
                    {
                        if (actionKvp.Value.Checked)
                        {
                            chiTietQuyen.Add(new ChiTietQuyenDTO(0, machucnang, actionKvp.Key));
                        }
                    }
                }

                bool success = false;

                if (currentMode == FormMode.Add)
                {
                    // Thêm mới
                    success = nhomQuyenBUS.Add(txtTenNhomQuyen.Text.Trim(), chiTietQuyen);
                    
                    if (success)
                    {
                        MessageBox.Show("Thêm nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhóm quyền thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (currentMode == FormMode.Edit)
                {
                    // Cập nhật
                    int maNhomQuyen = int.Parse(txtMaNhomQuyen.Text);
                    var nhomQuyen = new NhomQuyenDTO(maNhomQuyen, txtTenNhomQuyen.Text.Trim());
                    
                    int currentIndex = dgvNhomQuyen.CurrentRow.Index;
                    success = nhomQuyenBUS.Update(nhomQuyen, chiTietQuyen, currentIndex);
                    
                    if (success)
                    {
                        MessageBox.Show("Cập nhật nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật nhóm quyền thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (success)
                {
                    LoadNhomQuyen();
                    SetFormMode(FormMode.View);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            SetFormMode(FormMode.View);
            
            if (dgvNhomQuyen.Rows.Count > 0)
            {
                dgvNhomQuyen.Rows[0].Selected = true;
            }
            else
            {
                ClearForm();
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                LoadNhomQuyen();
            }
            else
            {
                var result = nhomQuyenBUS.Search(keyword);
                dgvNhomQuyen.DataSource = null;
                dgvNhomQuyen.DataSource = result;
            }
        }

        private void TxtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            LoadNhomQuyen();
            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearForm()
        {
            txtMaNhomQuyen.Text = "";
            txtTenNhomQuyen.Text = "";
            
            foreach (var dict in checkBoxQuyen.Values)
            {
                foreach (var cb in dict.Values)
                {
                    cb.Checked = false;
                }
            }
        }
    }
}
