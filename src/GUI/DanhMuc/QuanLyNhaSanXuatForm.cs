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
        private DataGridView dgvNhaSanXuat;
        private TextBox txtMaNSX, txtTenNSX, txtDiaChi, txtSDT, txtEmail, txtTimKiem;
        private ComboBox cboTimKiem;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh, btnExport;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Nhà sản xuất";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ NHÀ SẢN XUẤT",
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

            // DataGridView - TẮT AUTO GENERATE COLUMNS
            dgvNhaSanXuat = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(850, 450),
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
            dgvNhaSanXuat.SelectionChanged += DgvNhaSanXuat_SelectionChanged;
            this.Controls.Add(dgvNhaSanXuat);

            // Form Panel
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(900, 140);
            this.Controls.Add(formPanel);

            // Button Panel
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 610);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
        }

        // QUAN TRỌNG: Tạo columns THỦ CÔNG TRƯỚC KHI LOAD DATA
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

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1300, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            cboTimKiem = new ComboBox
            {
                Location = new Point(10, 12),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã NSX", "Tên NSX", "Số điện thoại", "Email" });
            cboTimKiem.SelectedIndex = 0;
            panel.Controls.Add(cboTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(170, 12),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTimKiem);

            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(480, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;
            panel.Controls.Add(btnTimKiem);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(590, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            panel.Controls.Add(btnRefresh);

            btnExport = new Button
            {
                Text = "Xuất Excel",
                Location = new Point(700, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
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
                Size = new Size(450, 450),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN NHÀ SẢN XUẤT",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã NSX
            Label lblMaNSX = new Label
            {
                Text = "Mã NSX:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaNSX);

            txtMaNSX = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            panel.Controls.Add(txtMaNSX);
            yPos += 40;

            // Tên NSX
            Label lblTenNSX = new Label
            {
                Text = "Tên NSX: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblTenNSX);

            txtTenNSX = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTenNSX);
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
                Text = "Số điện thoại: *",
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
                Size = new Size(1300, 50),
                BackColor = Color.Transparent
            };

            int xPos = 0;
            int btnWidth = 100;
            int btnHeight = 35;
            int spacing = 15;

            btnThem = new Button
            {
                Text = "Thêm",
                Location = new Point(xPos, 10),
                Size = new Size(btnWidth, btnHeight),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            panel.Controls.Add(btnThem);
            xPos += btnWidth + spacing;

            btnSua = new Button
            {
                Text = "Sửa",
                Location = new Point(xPos, 10),
                Size = new Size(btnWidth, btnHeight),
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            panel.Controls.Add(btnSua);
            xPos += btnWidth + spacing;

            btnXoa = new Button
            {
                Text = "Xóa",
                Location = new Point(xPos, 10),
                Size = new Size(btnWidth, btnHeight),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            panel.Controls.Add(btnXoa);
            xPos += btnWidth + spacing;

            btnLuu = new Button
            {
                Text = "Lưu",
                Location = new Point(xPos, 10),
                Size = new Size(btnWidth, btnHeight),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Enabled = false
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            panel.Controls.Add(btnLuu);
            xPos += btnWidth + spacing;

            btnHuy = new Button
            {
                Text = "Hủy",
                Location = new Point(xPos, 10),
                Size = new Size(btnWidth, btnHeight),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Enabled = false
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;
            panel.Controls.Add(btnHuy);

            return panel;
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

                JTableExporter.ExportJTableToExcel(dgvNhaSanXuat);
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
