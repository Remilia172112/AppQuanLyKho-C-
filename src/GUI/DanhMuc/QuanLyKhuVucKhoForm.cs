using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyKhuVucKhoForm : Form
    {
        private KhuVucKhoBUS khuVucKhoBUS;
        private DataGridView dgvKhuVucKho;
        private TextBox txtMaKV, txtTenKV, txtGhiChu, txtTimKiem;
        private ComboBox cboTimKiem;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnTimKiem, btnRefresh;
        private bool isEditing = false;
        private int currentMaKV = -1;

        public QuanLyKhuVucKhoForm()
        {
            InitializeComponent();
            khuVucKhoBUS = new KhuVucKhoBUS();
            LoadData();
            SetButtonStates(false);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Khu vực kho";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ KHU VỰC KHO",
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
            dgvKhuVucKho = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(750, 450),
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
            InitializeDataGridView();
            dgvKhuVucKho.SelectionChanged += DgvKhuVucKho_SelectionChanged;
            this.Controls.Add(dgvKhuVucKho);

            // Form Panel
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);
            this.Controls.Add(formPanel);

            // Button Panel
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 610);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
        }

        private void InitializeDataGridView()
        {
            dgvKhuVucKho.Columns.Clear();

            // Column MKVK
            DataGridViewTextBoxColumn colMKVK = new DataGridViewTextBoxColumn
            {
                Name = "MKVK",
                DataPropertyName = "MKVK",
                HeaderText = "Mã khu vực",
                Width = 100
            };
            dgvKhuVucKho.Columns.Add(colMKVK);

            // Column TEN
            DataGridViewTextBoxColumn colTEN = new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên khu vực",
                Width = 200
            };
            dgvKhuVucKho.Columns.Add(colTEN);

            // Column GHICHU
            DataGridViewTextBoxColumn colGHICHU = new DataGridViewTextBoxColumn
            {
                Name = "GHICHU",
                DataPropertyName = "GHICHU",
                HeaderText = "Ghi chú",
                Width = 300
            };
            dgvKhuVucKho.Columns.Add(colGHICHU);

            // Column TT
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 120
            };
            dgvKhuVucKho.Columns.Add(colTT);

            // Style header
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã KV", "Tên khu vực" });
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

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(550, 450),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN KHU VỰC KHO",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã KV
            Label lblMaKV = new Label
            {
                Text = "Mã khu vực:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaKV);

            txtMaKV = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(380, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            panel.Controls.Add(txtMaKV);
            yPos += 40;

            // Tên KV
            Label lblTenKV = new Label
            {
                Text = "Tên khu vực: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblTenKV);

            txtTenKV = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(380, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTenKV);
            yPos += 40;

            // Ghi chú
            Label lblGhiChu = new Label
            {
                Text = "Ghi chú:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblGhiChu);

            txtGhiChu = new TextBox
            {
                Location = new Point(140, yPos),
                Size = new Size(380, 80),
                Font = new Font("Segoe UI", 10F),
                Multiline = true
            };
            panel.Controls.Add(txtGhiChu);

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

        private void LoadData()
        {
            try
            {
                var khuVucKhoList = khuVucKhoBUS.GetAll();
                
                if (khuVucKhoList != null)
                {
                    dgvKhuVucKho.DataSource = new BindingList<KhuVucKhoDTO>(khuVucKhoList);
                }
                else
                {
                    dgvKhuVucKho.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvKhuVucKho_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null && !isEditing)
            {
                DisplayKhuVucKhoInfo();
            }
        }

        private void DisplayKhuVucKhoInfo()
        {
            try
            {
                if (dgvKhuVucKho.CurrentRow == null) return;

                var row = dgvKhuVucKho.CurrentRow;
                txtMaKV.Text = row.Cells["MKVK"].Value?.ToString() ?? "";
                txtTenKV.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                txtGhiChu.Text = row.Cells["GHICHU"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                // Silent catch - không hiển thị lỗi cho user khi display info
            }
        }

        private void ClearForm()
        {
            txtMaKV.Clear();
            txtTenKV.Clear();
            txtGhiChu.Clear();
            currentMaKV = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing;
            btnXoa.Enabled = !editing;
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvKhuVucKho.Enabled = !editing;
            
            txtTenKV.ReadOnly = !editing;
            txtGhiChu.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtMaKV.Text = "(Tự động)";
            txtTenKV.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null)
            {
                currentMaKV = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                SetButtonStates(true);
                txtTenKV.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khu vực kho cần sửa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khu vực kho này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maKV = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                        int index = dgvKhuVucKho.CurrentRow.Index;
                        var khuVucKho = khuVucKhoBUS.GetAll().Find(x => x.MKVK == maKV);
                        if (khuVucKho != null && khuVucKhoBUS.Delete(khuVucKho, index))
                        {
                            MessageBox.Show("Xóa khu vực kho thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa khu vực kho thất bại!", "Lỗi", 
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
                MessageBox.Show("Vui lòng chọn khu vực kho cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khu vực!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKV.Focus();
                return;
            }

            try
            {
                var khuVucKho = new KhuVucKhoDTO
                {
                    TEN = txtTenKV.Text.Trim(),
                    GHICHU = txtGhiChu.Text.Trim(),
                    TT = 1
                };

                if (currentMaKV == -1) // Thêm mới
                {
                    if (khuVucKhoBUS.Add(khuVucKho))
                    {
                        MessageBox.Show("Thêm khu vực kho thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Thêm khu vực kho thất bại!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Cập nhật
                {
                    khuVucKho.MKVK = currentMaKV;
                    if (khuVucKhoBUS.Update(khuVucKho))
                    {
                        MessageBox.Show("Cập nhật khu vực kho thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khu vực kho thất bại!", "Lỗi", 
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
            if (dgvKhuVucKho.CurrentRow != null)
            {
                DisplayKhuVucKhoInfo();
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
                var result = khuVucKhoBUS.Search(keyword, searchType);
                dgvKhuVucKho.DataSource = new BindingList<KhuVucKhoDTO>(result);
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
    }
}
