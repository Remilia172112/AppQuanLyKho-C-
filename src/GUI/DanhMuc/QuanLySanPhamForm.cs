using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.DanhMuc
{
    public partial class QuanLySanPhamForm : Form
    {
        private SanPhamBUS sanPhamBUS;
        private NhaSanXuatBUS nhaSanXuatBUS;
        private KhuVucKhoBUS khuVucKhoBUS;
        private LoaiSanPhamBUS loaiSanPhamBUS;
        private string selectedImagePath = "";
        private bool isEditing = false;
        private int currentMaSP = -1;

        public QuanLySanPhamForm()
        {
            InitializeComponent();
            sanPhamBUS = new SanPhamBUS();
            InitializeDataGridView();
            nhaSanXuatBUS = new NhaSanXuatBUS();
            khuVucKhoBUS = new KhuVucKhoBUS();
            loaiSanPhamBUS = new LoaiSanPhamBUS();
            LoadData();
            LoadComboBoxData();
            SetButtonStates(false);
            CheckPermissions();
        }

        private void InitializeDataGridView()
        {
            dgvSanPham.Columns.Clear();
            // Ngăn tự động tạo cột để tránh lặp
            dgvSanPham.AutoGenerateColumns = false;

            // 1. Mã Sản Phẩm
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MSP",
                DataPropertyName = "MSP",
                HeaderText = "Mã SP",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            // 2. Tên Sản Phẩm (Cho giãn hết phần còn lại)
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên sản phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Resizable = DataGridViewTriState.True
            });

            // 3. Danh Mục
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DANHMUC",
                DataPropertyName = "DANHMUC",
                HeaderText = "Danh mục",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            // 4. Giá Xuất (Định dạng tiền tệ)
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TIENX",
                DataPropertyName = "TIENX",
                HeaderText = "Giá bán",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            // 5. Số Lượng
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SL",
                DataPropertyName = "SL",
                HeaderText = "Tồn kho",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.True
            });

            // --- CÁC CỘT ẨN (Để Binding dữ liệu nhưng không hiện) ---
            string[] hiddenCols = { "HINHANH", "MNSX", "MKVK", "MLSP", "TIENN", "TT" };
            foreach (var col in hiddenCols)
            {
                dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col,
                    DataPropertyName = col,
                    Visible = false
                });
            }
            dgvSanPham.CellFormatting += DgvSanPham_CellFormatting;
        }

        private void DgvSanPham_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "SL" && e.RowIndex >= 0)
            {
                var cellValue = dgvSanPham.Rows[e.RowIndex].Cells["SL"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int soLuong))
                {
                    if (soLuong <= 5)
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.Font = new Font(dgvSanPham.Font, FontStyle.Bold);
                    }
                    else if (soLuong <= 10)
                    {
                        e.CellStyle.BackColor = Color.OrangeRed;
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.Font = new Font(dgvSanPham.Font, FontStyle.Bold);
                    }
                    else if (soLuong <= 20)
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void LoadData()
        {
            try
            {
                var listSanPham = sanPhamBUS.GetAll();

                if (listSanPham == null)
                {
                    listSanPham = new List<SanPhamDTO>();
                }

                // Dùng BindingList để hỗ trợ cập nhật giao diện tốt hơn
                dgvSanPham.DataSource = new System.ComponentModel.BindingList<SanPhamDTO>(listSanPham);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}\n\nChi tiết: {ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string type = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

                // Lấy danh sách đã lọc từ BUS
                var filteredList = sanPhamBUS.Search(keyword, type);

                // Gán dữ liệu vào Grid
                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = new System.ComponentModel.BindingList<SanPhamDTO>(filteredList);

                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPham.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi Helper xuất Excel (Prefix "SP")
                TableExporter.ExportTableToExcel(dgvSanPham, "SP");
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
                // 1. Đọc file Excel
                List<SanPhamDTO> listNewData = ExcelHelper.ReadSanPhamFromExcel();

                if (listNewData != null && listNewData.Count > 0)
                {
                    // 2. Thêm vào DB
                    int count = sanPhamBUS.AddMany(listNewData);

                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} sản phẩm!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại Grid
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
