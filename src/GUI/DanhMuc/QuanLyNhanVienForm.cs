using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.Helper;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyNhanVienForm : Form
    {
        private NhanVienBUS nhanVienBUS;
        private DataGridView dgvNhanVien;
        private TextBox txtMaNV, txtHoTen, txtSDT, txtEmail, txtTimKiem;
        private ComboBox cboGioiTinh, cboTimKiem;
        private DateTimePicker dtpNgaySinh;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnExport;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Nhân viên";
            this.Size = new Size(1400, 800);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ NHÂN VIÊN",
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
            dgvNhanVien = new DataGridView
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
            dgvNhanVien.SelectionChanged += DgvNhanVien_SelectionChanged;
            this.Controls.Add(dgvNhanVien);

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
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Họ tên", "Email", "Số điện thoại" });
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

            btnExport = new Button
            {
                Text = "📥 Xuất Excel",
                Location = new Point(800, 10),
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
                Text = "THÔNG TIN NHÂN VIÊN",
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
                Text = "Mã NV:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaNV);

            txtMaNV = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            panel.Controls.Add(txtMaNV);
            yPos += 40;

            // Họ tên
            Label lblHoTen = new Label
            {
                Text = "Họ tên: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblHoTen);

            txtHoTen = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtHoTen);
            yPos += 40;

            // Giới tính
            Label lblGioiTinh = new Label
            {
                Text = "Giới tính: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblGioiTinh);

            cboGioiTinh = new ComboBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;
            panel.Controls.Add(cboGioiTinh);
            yPos += 40;

            // Ngày sinh
            Label lblNgaySinh = new Label
            {
                Text = "Ngày sinh: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblNgaySinh);

            dtpNgaySinh = new DateTimePicker
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10F),
                MaxDate = DateTime.Now.AddYears(-18), // Tối thiểu 18 tuổi
                Value = DateTime.Now.AddYears(-20)
            };
            panel.Controls.Add(dtpNgaySinh);
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
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Xuất danh sách nhân viên",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var nhanVienList = nhanVienBUS.GetAll();
                    nhanVienBUS.ExportToExcelFile(nhanVienList, saveFileDialog.FileName);
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing;
            btnXoa.Enabled = !editing;
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
