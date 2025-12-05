using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using src.BUS;
using src.DTO;
using src.Helper;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyTaiKhoanForm : Form
    {
        private TaiKhoanBUS taiKhoanBUS;
        private NhanVienBUS nhanVienBUS;
        private NhomQuyenBUS nhomQuyenBUS;
        private DataGridView dgvTaiKhoan;
        private ComboBox cboMaNV, cboNhomQuyen, cboTimKiem;
        private TextBox txtTenNV, txtTenDangNhap, txtMatKhau, txtXacNhanMK, txtTimKiem;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnResetMK;
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
                InitializeDataGridView();
                LoadNhomQuyen();
                LoadNhanVien();
                LoadData();
                SetButtonStates(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Tài khoản";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ TÀI KHOẢN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Search Panel
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);
            this.Controls.Add(searchPanel);

            // DataGridView
            dgvTaiKhoan = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(750, 500),
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false
            };
            dgvTaiKhoan.SelectionChanged += DgvTaiKhoan_SelectionChanged;
            this.Controls.Add(dgvTaiKhoan);

            // Form Panel
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);
            this.Controls.Add(formPanel);

            // Button Panel
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 660);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
        }

        private void InitializeDataGridView()
        {
            dgvTaiKhoan.Columns.Clear();

            // Column MNV
            DataGridViewTextBoxColumn colMNV = new DataGridViewTextBoxColumn
            {
                Name = "MNV",
                DataPropertyName = "MNV",
                HeaderText = "Mã NV",
                Width = 80
            };
            dgvTaiKhoan.Columns.Add(colMNV);

            // Column TDN
            DataGridViewTextBoxColumn colTDN = new DataGridViewTextBoxColumn
            {
                Name = "TDN",
                DataPropertyName = "TDN",
                HeaderText = "Tên đăng nhập",
                Width = 180
            };
            dgvTaiKhoan.Columns.Add(colTDN);

            // Column MK (ẩn hoặc hiển thị **)
            DataGridViewTextBoxColumn colMK = new DataGridViewTextBoxColumn
            {
                Name = "MK",
                DataPropertyName = "MK",
                HeaderText = "Mật khẩu",
                Width = 150,
                Visible = false // Ẩn mật khẩu
            };
            dgvTaiKhoan.Columns.Add(colMK);

            // Column MNQ
            DataGridViewTextBoxColumn colMNQ = new DataGridViewTextBoxColumn
            {
                Name = "MNQ",
                DataPropertyName = "MNQ",
                HeaderText = "Mã nhóm quyền",
                Width = 120
            };
            dgvTaiKhoan.Columns.Add(colMNQ);

            // Column TT
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 100
            };
            dgvTaiKhoan.Columns.Add(colTT);

            // Style header
            dgvTaiKhoan.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvTaiKhoan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTaiKhoan.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvTaiKhoan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1320, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            cboTimKiem = new ComboBox
            {
                Location = new Point(10, 12),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã nhân viên", "Username" });
            cboTimKiem.SelectedIndex = 0;
            panel.Controls.Add(cboTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(170, 12),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTimKiem);

            btnTimKiem = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(580, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;
            panel.Controls.Add(btnTimKiem);

            btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(690, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            panel.Controls.Add(btnRefresh);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(550, 500),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN TÀI KHOẢN",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NV
            Label lblMaNV = new Label
            {
                Text = "Nhân viên: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaNV);

            cboMaNV = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMaNV.SelectedIndexChanged += CboMaNV_SelectedIndexChanged;
            panel.Controls.Add(cboMaNV);
            yPos += 40;

            // Tên NV (readonly - lấy từ DB)
            Label lblTenNV = new Label
            {
                Text = "Tên nhân viên:",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblTenNV);

            txtTenNV = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTenNV);
            yPos += 40;

            // Tên đăng nhập
            Label lblTenDangNhap = new Label
            {
                Text = "Tên đăng nhập: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblTenDangNhap);

            txtTenDangNhap = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTenDangNhap);
            yPos += 40;

            // Mật khẩu
            Label lblMatKhau = new Label
            {
                Text = "Mật khẩu: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMatKhau);

            txtMatKhau = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                PasswordChar = '●',
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtMatKhau);
            yPos += 40;

            // Xác nhận mật khẩu
            Label lblXacNhanMK = new Label
            {
                Text = "Xác nhận MK: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblXacNhanMK);

            txtXacNhanMK = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                PasswordChar = '●',
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtXacNhanMK);
            yPos += 40;

            // Nhóm quyền
            Label lblNhomQuyen = new Label
            {
                Text = "Nhóm quyền: *",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblNhomQuyen);

            cboNhomQuyen = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(330, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(cboNhomQuyen);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1320, 60),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            btnThem = new Button
            {
                Text = "➕ Thêm",
                Location = new Point(20, 15),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            panel.Controls.Add(btnThem);

            btnSua = new Button
            {
                Text = "✏️ Sửa",
                Location = new Point(130, 15),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            panel.Controls.Add(btnSua);

            btnXoa = new Button
            {
                Text = "🗑️ Xóa",
                Location = new Point(240, 15),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            panel.Controls.Add(btnXoa);

            btnLuu = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(350, 15),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            panel.Controls.Add(btnLuu);

            btnHuy = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(460, 15),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;
            panel.Controls.Add(btnHuy);

            btnResetMK = new Button
            {
                Text = "🔑 Reset MK",
                Location = new Point(570, 15),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnResetMK.FlatAppearance.BorderSize = 0;
            btnResetMK.Click += BtnResetMK_Click;
            panel.Controls.Add(btnResetMK);

            return panel;
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

        private void LoadData()
        {
            try
            {
                var taiKhoanList = taiKhoanBUS.GetTaiKhoanAll();
                
                if (taiKhoanList != null)
                {
                    dgvTaiKhoan.DataSource = new BindingList<TaiKhoanDTO>(taiKhoanList);
                }
                else
                {
                    dgvTaiKhoan.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing;
            btnXoa.Enabled = !editing;
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
    }
}
