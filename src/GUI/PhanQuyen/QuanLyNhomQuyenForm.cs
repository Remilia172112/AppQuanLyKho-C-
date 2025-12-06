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
        
        // UI Controls - Left Panel (Danh sách nhóm quyền)
        private DataGridView dgvNhomQuyen;
        private TextBox txtMaNhomQuyen;
        private TextBox txtTenNhomQuyen;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;
        
        // UI Controls - Right Panel (Phân quyền chi tiết)
        private Panel pnlChucNang;
        private Dictionary<string, Dictionary<string, CheckBox>> checkBoxQuyen; // MCN -> (Action -> CheckBox)
        
        // Trạng thái form
        private enum FormMode { View, Add, Edit }
        private FormMode currentMode = FormMode.View;
        
        private SplitContainer mainSplit; // Lưu lại để xử lý resize

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

        private void InitializeComponent()
        {
            this.Text = "Quản lý Phân quyền";
            this.Size = new Size(1400, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Layout: Split Container (Left: Danh sách | Right: Chi tiết quyền)
            mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = (int)(this.Width * 0.45), // 45% chiều rộng form
                IsSplitterFixed = false // Cho phép kéo chia lại
            };

            // === LEFT PANEL: Danh sách nhóm quyền ===
            Panel leftPanel = new Panel { Dock = DockStyle.Fill };
            
            // Title
            Label lblTitle = new Label
            {
                Text = "DANH SÁCH NHÓM QUYỀN",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            leftPanel.Controls.Add(lblTitle);

            // Search Panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(510, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            txtTimKiem = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(380, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtTimKiem.KeyDown += TxtTimKiem_KeyDown;

            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(400, 5),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTimKiem.Click += BtnTimKiem_Click;

            searchPanel.Controls.AddRange(new Control[] { txtTimKiem, btnTimKiem });
            leftPanel.Controls.Add(searchPanel);

            // DataGridView
            dgvNhomQuyen = new DataGridView
            {
                Location = new Point(20, 105),
                Size = new Size(510, 350),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            
            InitializeDataGridView();
            dgvNhomQuyen.SelectionChanged += DgvNhomQuyen_SelectionChanged;
            leftPanel.Controls.Add(dgvNhomQuyen);

            // Info Panel
            GroupBox infoGroup = new GroupBox
            {
                Text = "Thông tin nhóm quyền",
                Location = new Point(20, 465),
                Size = new Size(510, 100),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblMa = new Label
            {
                Text = "Mã nhóm quyền:",
                Location = new Point(15, 30),
                Size = new Size(120, 20)
            };

            txtMaNhomQuyen = new TextBox
            {
                Location = new Point(140, 27),
                Size = new Size(150, 25),
                ReadOnly = true,
                BackColor = Color.LightGray
            };

            Label lblTen = new Label
            {
                Text = "Tên nhóm quyền:",
                Location = new Point(15, 60),
                Size = new Size(120, 20)
            };

            txtTenNhomQuyen = new TextBox
            {
                Location = new Point(140, 57),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 10)
            };

            infoGroup.Controls.AddRange(new Control[] { lblMa, txtMaNhomQuyen, lblTen, txtTenNhomQuyen });
            leftPanel.Controls.Add(infoGroup);

            // Button Panel
            Panel btnPanel = new Panel
            {
                Location = new Point(20, 575),
                Size = new Size(510, 50)
            };

            btnThem = CreateButton("Thêm", new Point(0, 0), Color.FromArgb(46, 204, 113));
            btnThem.Click += BtnThem_Click;

            btnSua = CreateButton("Sửa", new Point(105, 0), Color.FromArgb(52, 152, 219));
            btnSua.Click += BtnSua_Click;

            btnXoa = CreateButton("Xóa", new Point(210, 0), Color.FromArgb(231, 76, 60));
            btnXoa.Click += BtnXoa_Click;

            btnLuu = CreateButton("Lưu", new Point(315, 0), Color.FromArgb(26, 188, 156));
            btnLuu.Click += BtnLuu_Click;

            btnHuy = CreateButton("Hủy", new Point(420, 0), Color.FromArgb(149, 165, 166));
            btnHuy.Click += BtnHuy_Click;

            btnPanel.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLuu, btnHuy });
            leftPanel.Controls.Add(btnPanel);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(20, 635),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnRefresh.Click += BtnRefresh_Click;
            leftPanel.Controls.Add(btnRefresh);

            mainSplit.Panel1.Controls.Add(leftPanel);

            // === RIGHT PANEL: Phân quyền chi tiết ===
            Panel rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            
            Label lblQuyenTitle = new Label
            {
                Text = "PHÂN QUYỀN CHI TIẾT",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            rightPanel.Controls.Add(lblQuyenTitle);

            pnlChucNang = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(530, 615), // Giảm chiều ngang panel phải
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };
            
            InitializePermissionPanel();
            rightPanel.Controls.Add(pnlChucNang);

            mainSplit.Panel2.Controls.Add(rightPanel);
            this.Controls.Add(mainSplit);
        }

        private Button CreateButton(string text, Point location, Color color)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
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
                    Size = new Size(720, 60), // Tăng chiều rộng để chứa checkbox "Duyệt"
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
