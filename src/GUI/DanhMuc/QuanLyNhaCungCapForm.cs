using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.Helper;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyNhaCungCapForm : Form
    {
        private NhaCungCapBUS nhaCungCapBUS;
        private DataGridView dgvNhaCungCap;
        private TextBox txtMaNCC, txtTenNCC, txtDiaChi, txtSDT, txtEmail, txtTimKiem;
        private ComboBox cboTimKiem;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnExport;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Nhà cung cấp";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ NHÀ CUNG CẤP",
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
            dgvNhaCungCap = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(850, 500),
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
            dgvNhaCungCap.SelectionChanged += DgvNhaCungCap_SelectionChanged;
            this.Controls.Add(dgvNhaCungCap);

            // Form Panel
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(900, 140);
            this.Controls.Add(formPanel);

            // Button Panel
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 660);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
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
                Size = new Size(180, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã nhà cung cấp", "Tên nhà cung cấp", "Địa chỉ", "Số điện thoại", "Email" });
            cboTimKiem.SelectedIndex = 0;
            panel.Controls.Add(cboTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(200, 12),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTimKiem);

            btnTimKiem = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(610, 10),
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
                Location = new Point(720, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            panel.Controls.Add(btnRefresh);

            btnExport = new Button
            {
                Text = "📥 Xuất Excel",
                Location = new Point(830, 10),
                Size = new Size(110, 30),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;
            panel.Controls.Add(btnExport);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(450, 500),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN NHÀ CUNG CẤP",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NCC
            Label lblMaNCC = new Label
            {
                Text = "Mã NCC:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaNCC);

            txtMaNCC = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            panel.Controls.Add(txtMaNCC);
            yPos += 40;

            // Tên NCC
            Label lblTenNCC = new Label
            {
                Text = "Tên NCC: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblTenNCC);

            txtTenNCC = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTenNCC);
            yPos += 40;

            // Địa chỉ
            Label lblDiaChi = new Label
            {
                Text = "Địa chỉ: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblDiaChi);

            txtDiaChi = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtDiaChi);
            yPos += 40;

            // Số điện thoại
            Label lblSDT = new Label
            {
                Text = "Số ĐT: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblSDT);

            txtSDT = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtSDT);
            yPos += 40;

            // Email
            Label lblEmail = new Label
            {
                Text = "Email: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtEmail);

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

            return panel;
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
                JTableExporter.ExportJTableToExcel(dgvNhaCungCap);
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
            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing;
            btnXoa.Enabled = !editing;
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
