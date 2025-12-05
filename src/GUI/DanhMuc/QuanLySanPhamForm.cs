using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.DanhMuc
{
    public partial class QuanLySanPhamForm : Form
    {
        private SanPhamBUS sanPhamBUS;
        private NhaSanXuatBUS nhaSanXuatBUS;
        private KhuVucKhoBUS khuVucKhoBUS;
        private LoaiSanPhamBUS loaiSanPhamBUS;
        private DataGridView dgvSanPham;
        private TextBox txtMaSP, txtTenSP, txtGiaNhap, txtGiaXuat, txtSoLuong, txtTimKiem;
        private ComboBox cboNhaSX, cboKhuVuc, cboTimKiem, cboDanhMuc, cboLoaiSP;
        private PictureBox picHinhAnh;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnChonAnh, btnTimKiem, btnRefresh;
        private string selectedImagePath = "";
        private bool isEditing = false;
        private int currentMaSP = -1;

        public QuanLySanPhamForm()
        {
            InitializeComponent();
            sanPhamBUS = new SanPhamBUS();
            nhaSanXuatBUS = new NhaSanXuatBUS();
            khuVucKhoBUS = new KhuVucKhoBUS();
            loaiSanPhamBUS = new LoaiSanPhamBUS();
            LoadData();
            LoadComboBoxData();
            SetButtonStates(false);
            CheckPermissions();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "Quản lý Sản phẩm";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Header
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ SẢN PHẨM",
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
            dgvSanPham = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(750, 550),
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSanPham.SelectionChanged += DgvSanPham_SelectionChanged;
            this.Controls.Add(dgvSanPham);

            // Form Panel
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);
            this.Controls.Add(formPanel);

            // Button Panel
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 710);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1150, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            cboTimKiem = new ComboBox
            {
                Location = new Point(10, 12),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new string[] { "Tất cả", "Tên SP" });
            cboTimKiem.SelectedIndex = 0;
            panel.Controls.Add(cboTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(140, 12),
                Size = new Size(250, 25)
            };
            panel.Controls.Add(txtTimKiem);

            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(400, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;
            panel.Controls.Add(btnTimKiem);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(500, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            panel.Controls.Add(btnRefresh);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(380, 550),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            int y = 15;

            // Mã SP
            panel.Controls.Add(new Label { Text = "Mã SP:", Location = new Point(15, y), Size = new Size(100, 25) });
            txtMaSP = new TextBox { Location = new Point(120, y), Size = new Size(240, 25), ReadOnly = true, BackColor = SystemColors.Control };
            panel.Controls.Add(txtMaSP);
            y += 35;

            // Tên SP
            panel.Controls.Add(new Label { Text = "Tên SP: *", Location = new Point(15, y), Size = new Size(100, 25) });
            txtTenSP = new TextBox { Location = new Point(120, y), Size = new Size(240, 25) };
            panel.Controls.Add(txtTenSP);
            y += 35;

            // Danh mục (ComboBox)
            panel.Controls.Add(new Label { Text = "Danh mục:", Location = new Point(15, y), Size = new Size(100, 25) });
            cboDanhMuc = new ComboBox { Location = new Point(120, y), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDown };
            cboDanhMuc.Items.AddRange(new string[] { "Laptop", "Màn hình", "Bàn phím", "Chuột", "Tai nghe", 
                "Phụ kiện", "Linh kiện", "Điện thoại", "Thiết bị mạng", "Máy in" });
            panel.Controls.Add(cboDanhMuc);
            y += 35;

            // Loại sản phẩm
            panel.Controls.Add(new Label { Text = "Loại SP: *", Location = new Point(15, y), Size = new Size(100, 25) });
            cboLoaiSP = new ComboBox { Location = new Point(120, y), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cboLoaiSP);
            y += 35;

            // Nhà SX
            panel.Controls.Add(new Label { Text = "Nhà SX: *", Location = new Point(15, y), Size = new Size(100, 25) });
            cboNhaSX = new ComboBox { Location = new Point(120, y), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cboNhaSX);
            y += 35;

            // Khu vực
            panel.Controls.Add(new Label { Text = "Khu vực: *", Location = new Point(15, y), Size = new Size(100, 25) });
            cboKhuVuc = new ComboBox { Location = new Point(120, y), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cboKhuVuc);
            y += 35;

            // Giá nhập
            panel.Controls.Add(new Label { Text = "Giá nhập:", Location = new Point(15, y), Size = new Size(100, 25) });
            txtGiaNhap = new TextBox { Location = new Point(120, y), Size = new Size(240, 25), Text = "0" };
            panel.Controls.Add(txtGiaNhap);
            y += 35;

            // Giá xuất
            panel.Controls.Add(new Label { Text = "Giá xuất:", Location = new Point(15, y), Size = new Size(100, 25) });
            txtGiaXuat = new TextBox { Location = new Point(120, y), Size = new Size(240, 25), Text = "0" };
            panel.Controls.Add(txtGiaXuat);
            y += 35;

            // Số lượng
            panel.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(15, y), Size = new Size(100, 25) });
            txtSoLuong = new TextBox { Location = new Point(120, y), Size = new Size(240, 25), Text = "0" };
            panel.Controls.Add(txtSoLuong);
            y += 35;

            // Hình ảnh
            picHinhAnh = new PictureBox
            {
                Location = new Point(120, y),
                Size = new Size(150, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            panel.Controls.Add(picHinhAnh);

            btnChonAnh = new Button
            {
                Text = "Chọn ảnh",
                Location = new Point(280, y),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnChonAnh.FlatAppearance.BorderSize = 0;
            btnChonAnh.Click += BtnChonAnh_Click;
            panel.Controls.Add(btnChonAnh);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1150, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            int x = 10;
            btnThem = CreateButton("Thêm", x, Color.FromArgb(46, 204, 113), BtnThem_Click);
            panel.Controls.Add(btnThem);
            x += 110;

            btnSua = CreateButton("Sửa", x, Color.FromArgb(52, 152, 219), BtnSua_Click);
            panel.Controls.Add(btnSua);
            x += 110;

            btnXoa = CreateButton("Xóa", x, Color.FromArgb(231, 76, 60), BtnXoa_Click);
            panel.Controls.Add(btnXoa);
            x += 110;

            btnLuu = CreateButton("Lưu", x, Color.FromArgb(41, 128, 185), BtnLuu_Click);
            panel.Controls.Add(btnLuu);
            x += 110;

            btnHuy = CreateButton("Hủy", x, Color.FromArgb(149, 165, 166), BtnHuy_Click);
            panel.Controls.Add(btnHuy);

            return panel;
        }

        private Button CreateButton(string text, int x, Color color, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 10),
                Size = new Size(100, 30),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += clickHandler;
            return btn;
        }

        private void LoadData()
        {
            try
            {
                var list = sanPhamBUS.GetAll();
                
                // Debug: Kiểm tra số lượng sản phẩm
                Console.WriteLine($"Số lượng sản phẩm load được: {list?.Count ?? 0}");
                
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu sản phẩm trong database hoặc tất cả sản phẩm đã bị xóa (TT=0)", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = new System.ComponentModel.BindingList<SanPhamDTO>(list);

                if (dgvSanPham.Columns.Count > 0)
                {
                    dgvSanPham.Columns["MSP"].HeaderText = "Mã SP";
                    dgvSanPham.Columns["TEN"].HeaderText = "Tên sản phẩm";
                    dgvSanPham.Columns["DANHMUC"].HeaderText = "Danh mục";
                    dgvSanPham.Columns["TIENX"].HeaderText = "Giá xuất";
                    dgvSanPham.Columns["SL"].HeaderText = "Số lượng";
                    dgvSanPham.Columns["HINHANH"].Visible = false;
                    dgvSanPham.Columns["MNSX"].Visible = false;
                    dgvSanPham.Columns["MKVK"].Visible = false;
                    dgvSanPham.Columns["MLSP"].Visible = false;
                    dgvSanPham.Columns["TIENN"].Visible = false;
                    dgvSanPham.Columns["TT"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                var listNSX = nhaSanXuatBUS.GetAll();
                if (listNSX != null && listNSX.Count > 0)
                {
                    cboNhaSX.DataSource = new System.ComponentModel.BindingList<NhaSanXuatDTO>(listNSX);
                    cboNhaSX.DisplayMember = "TEN";
                    cboNhaSX.ValueMember = "MNSX";
                }

                var listKVK = khuVucKhoBUS.GetAll();
                if (listKVK != null && listKVK.Count > 0)
                {
                    cboKhuVuc.DataSource = new System.ComponentModel.BindingList<KhuVucKhoDTO>(listKVK);
                    cboKhuVuc.DisplayMember = "TEN";
                    cboKhuVuc.ValueMember = "MKVK";
                }

                var listLSP = loaiSanPhamBUS.GetAll();
                if (listLSP != null && listLSP.Count > 0)
                {
                    cboLoaiSP.DataSource = new System.ComponentModel.BindingList<LoaiSanPhamDTO>(listLSP);
                    cboLoaiSP.DisplayMember = "TEN";
                    cboLoaiSP.ValueMember = "MLSP";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPermissions()
        {
            btnThem.Enabled = SessionManager.CanCreate("sanpham");
            btnSua.Enabled = SessionManager.CanUpdate("sanpham");
            btnXoa.Enabled = SessionManager.CanDelete("sanpham");
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("sanpham");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("sanpham");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("sanpham");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            
            txtTenSP.ReadOnly = !editing;
            cboDanhMuc.Enabled = editing;
            cboLoaiSP.Enabled = editing;
            txtGiaNhap.ReadOnly = !editing;
            txtGiaXuat.ReadOnly = !editing;
            txtSoLuong.ReadOnly = !editing;
            cboNhaSX.Enabled = editing;
            cboKhuVuc.Enabled = editing;
            btnChonAnh.Enabled = editing;
            dgvSanPham.Enabled = !editing;
        }

        private void ClearForm()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboDanhMuc.SelectedIndex = -1;
            cboDanhMuc.Text = "";
            txtGiaNhap.Text = "0";
            txtGiaXuat.Text = "0";
            txtSoLuong.Text = "0";
            picHinhAnh.Image = null;
            selectedImagePath = "";
            currentMaSP = -1;
        }

        private void DgvSanPham_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0 && !isEditing)
            {
                try
                {
                    var row = dgvSanPham.SelectedRows[0];
                    txtMaSP.Text = row.Cells["MSP"].Value?.ToString() ?? "";
                    txtTenSP.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                    cboDanhMuc.Text = row.Cells["DANHMUC"].Value?.ToString() ?? "";
                    txtGiaNhap.Text = row.Cells["TIENN"].Value?.ToString() ?? "0";
                    txtGiaXuat.Text = row.Cells["TIENX"].Value?.ToString() ?? "0";
                    txtSoLuong.Text = row.Cells["SL"].Value?.ToString() ?? "0";

                    if (row.Cells["MLSP"].Value != null)
                        cboLoaiSP.SelectedValue = Convert.ToInt32(row.Cells["MLSP"].Value);
                    if (row.Cells["MNSX"].Value != null)
                        cboNhaSX.SelectedValue = Convert.ToInt32(row.Cells["MNSX"].Value);
                    if (row.Cells["MKVK"].Value != null)
                        cboKhuVuc.SelectedValue = Convert.ToInt32(row.Cells["MKVK"].Value);

                    string imagePath = row.Cells["HINHANH"].Value?.ToString() ?? "";
                    LoadProductImage(imagePath);
                }
                catch { }
            }
        }

        private void LoadProductImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Đường dẫn tương đối từ thư mục gốc project
                    string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
                    string fullPath = Path.Combine(projectRoot, "img_product", imagePath);
                    
                    if (File.Exists(fullPath))
                    {
                        // Giải phóng ảnh cũ nếu có
                        if (picHinhAnh.Image != null)
                        {
                            var oldImage = picHinhAnh.Image;
                            picHinhAnh.Image = null;
                            oldImage.Dispose();
                        }
                        
                        // Load ảnh mới từ memory stream để không lock file
                        using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            picHinhAnh.Image = Image.FromStream(stream);
                        }
                        selectedImagePath = imagePath;
                        return;
                    }
                }
            }
            catch
            {
                // Không hiển thị lỗi, chỉ set ảnh mặc định
            }
            
            // Nếu không load được ảnh, set null
            if (picHinhAnh.Image != null)
            {
                var oldImage = picHinhAnh.Image;
                picHinhAnh.Image = null;
                oldImage.Dispose();
            }
            selectedImagePath = "";
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtTenSP.Focus();
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentMaSP = Convert.ToInt32(txtMaSP.Text);
            SetButtonStates(true);
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int maSP = Convert.ToInt32(txtMaSP.Text);
                    SanPhamDTO? sp = sanPhamBUS.GetByMaSP(maSP);
                    
                    if (sp != null && sanPhamBUS.Delete(sp))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                // selectedImagePath chỉ chứa tên file, không có đường dẫn đầy đủ
                string imagePath = selectedImagePath;

                SanPhamDTO sp = new SanPhamDTO
                {
                    MSP = currentMaSP == -1 ? 0 : currentMaSP,
                    TEN = txtTenSP.Text.Trim(),
                    DANHMUC = cboDanhMuc.Text.Trim(),
                    HINHANH = imagePath,
                    MNSX = Convert.ToInt32(cboNhaSX.SelectedValue),
                    MKVK = Convert.ToInt32(cboKhuVuc.SelectedValue),
                    MLSP = Convert.ToInt32(cboLoaiSP.SelectedValue),
                    TIENN = int.Parse(txtGiaNhap.Text),
                    TIENX = int.Parse(txtGiaXuat.Text),
                    SL = int.Parse(txtSoLuong.Text),
                    TT = 1
                };

                bool success = currentMaSP == -1 ? sanPhamBUS.Add(sp) : sanPhamBUS.Update(sp);

                if (success)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            SetButtonStates(false);
            if (dgvSanPham.Rows.Count > 0)
                dgvSanPham.Rows[0].Selected = true;
        }

        private void BtnChonAnh_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Chọn ảnh sản phẩm";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Lấy đường dẫn thư mục img_product
                        string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
                        string imgFolder = Path.Combine(projectRoot, "img_product");
                        
                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(imgFolder))
                        {
                            Directory.CreateDirectory(imgFolder);
                        }
                        
                        // Lấy tên file gốc
                        string fileName = Path.GetFileName(ofd.FileName);
                        string destPath = Path.Combine(imgFolder, fileName);
                        
                        // Nếu file đã tồn tại, thêm timestamp vào tên
                        if (File.Exists(destPath))
                        {
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                            string extension = Path.GetExtension(fileName);
                            fileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                            destPath = Path.Combine(imgFolder, fileName);
                        }
                        
                        // Copy file vào thư mục img_product
                        File.Copy(ofd.FileName, destPath, true);
                        
                        // Load ảnh và lưu đường dẫn (chỉ tên file)
                        LoadProductImage(fileName);
                        selectedImagePath = fileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi chọn ảnh: {ex.Message}", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            var all = sanPhamBUS.GetAll();
            var filtered = all.FindAll(sp => sp.TEN.ToLower().Contains(keyword));
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = filtered;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtGiaNhap.Text, out _) || !int.TryParse(txtGiaXuat.Text, out _) || !int.TryParse(txtSoLuong.Text, out _))
            {
                MessageBox.Show("Giá và số lượng phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
