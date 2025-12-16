using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;

namespace src.GUI.NghiepVu
{
    public partial class ChonSanPhamXuatDialog : Form
    {
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();
        private PhieuXuatBUS phieuXuatBUS = new PhieuXuatBUS();
        public ChiTietPhieuXuatDTO SelectedItem { get; private set; }

        private ChiTietPhieuXuatDTO editingItem = null;
        private int currentStockLevel = 0;

        public ChonSanPhamXuatDialog(ChiTietPhieuXuatDTO existingItem = null)
        {
            this.editingItem = existingItem;
            InitializeComponent();
            
            InitializeDataGridView(); 

            this.Text = editingItem == null ? "Chọn sản phẩm xuất" : "Sửa sản phẩm xuất";
            
            LoadData();

            if (existingItem != null)
            {
                LoadExistingItem();
            }
            txtGia.ReadOnly = true;
        }
        private void InitializeDataGridView()
        {
            dgvSanPham.Columns.Clear();
            dgvSanPham.AutoGenerateColumns = false;

            // Cột Mã SP
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "MSP", 
                DataPropertyName = "MSP", 
                HeaderText = "Mã SP", 
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Tên SP
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TEN", 
                DataPropertyName = "TEN", 
                HeaderText = "Tên sản phẩm", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
            });

            // Cột Hình ảnh
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "HINHANH", 
                DataPropertyName = "HINHANH", 
                HeaderText = "Hình ảnh", 
                Width = 100 
            });

            // Cột Tồn kho (SL) - QUAN TRỌNG: Phải có Name = "SL"
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "SL", 
                DataPropertyName = "SL", 
                HeaderText = "Tồn kho", 
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Giá xuất (TIENX) - QUAN TRỌNG: Phải có Name = "TIENX"
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TIENX", 
                DataPropertyName = "TIENX", 
                HeaderText = "Giá xuất", 
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Giá nhập (TIENN) - Có thể ẩn nếu không cần thiết
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TIENN", 
                DataPropertyName = "TIENN", 
                HeaderText = "Giá nhập", 
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Visible = false // Thường ẩn giá nhập khi bán hàng
            });
        }

        private void LoadData()
        {
            try
            {
                // Gọi hàm đã tính toán trừ đi hàng chưa duyệt
                var list = GetSanPhamKhaDung();
                dgvSanPham.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingItem()
        {
            if (editingItem != null)
            {
                nudSoLuong.Value = editingItem.SL;
                txtGia.Text = editingItem.TIENXUAT.ToString();

                // Select the product in grid
                foreach (DataGridViewRow row in dgvSanPham.Rows)
                {
                    if (Convert.ToInt32(row.Cells["MSP"].Value) == editingItem.MSP)
                    {
                        row.Selected = true;
                        dgvSanPham.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Cũng lấy từ nguồn đã trừ tồn kho
                var list = GetSanPhamKhaDung();
                
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    string search = txtSearch.Text.ToLower();
                    list = list.Where(sp =>
                        sp.TEN.ToLower().Contains(search) ||
                        sp.MSP.ToString().Contains(search)
                    ).ToList();
                }
                
                dgvSanPham.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy giá xuất (TIENX)
                if (dgvSanPham.Columns.Contains("TIENX") && dgvSanPham.Rows[e.RowIndex].Cells["TIENX"].Value != null)
                {
                    txtGia.Text = dgvSanPham.Rows[e.RowIndex].Cells["TIENX"].Value.ToString();
                }

                // Lấy tồn kho và giới hạn số lượng xuất
                if (dgvSanPham.Columns.Contains("SL") && dgvSanPham.Rows[e.RowIndex].Cells["SL"].Value != null)
                {
                    currentStockLevel = Convert.ToInt32(dgvSanPham.Rows[e.RowIndex].Cells["SL"].Value);
                    txtTonKho.Text = currentStockLevel.ToString();
                    
                    // Reset max và value khi chọn sản phẩm mới
                    if (currentStockLevel > 0)
                    {
                        nudSoLuong.Maximum = currentStockLevel;
                        if (nudSoLuong.Value > currentStockLevel) nudSoLuong.Value = currentStockLevel;
                        else if (nudSoLuong.Value == 0) nudSoLuong.Value = 1;
                    }
                    else
                    {
                        nudSoLuong.Value = 0;
                        nudSoLuong.Maximum = 0;
                    }
                }
            }
        }

        private void BtnChon_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // KIỂM TRA KHÔNG VƯỢT QUÁ TỒN KHO
            // Lưu ý: Nếu đang sửa (editingItem != null), logic kiểm tra có thể cần linh hoạt hơn
            // (Ví dụ: Tồn kho 5, đang có trong phiếu 2, muốn sửa thành 3 -> Tổng cần 3, kho có 5+2=7 -> OK)
            // Code dưới đây kiểm tra đơn giản theo Tồn kho hiện tại hiển thị trên lưới.
            if (nudSoLuong.Value > currentStockLevel + (editingItem?.SL ?? 0))
            {
                MessageBox.Show(
                    $"Số lượng xuất vượt quá tồn kho khả dụng!", 
                    "Thông báo", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int msp = Convert.ToInt32(dgvSanPham.SelectedRows[0].Cells["MSP"].Value);
            
            SelectedItem = new ChiTietPhieuXuatDTO
            {
                MSP = msp,
                SL = (int)nudSoLuong.Value,
                TIENXUAT = (int)gia,
                MKM = 0
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DgvSanPham_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && 
                dgvSanPham.Columns[e.ColumnIndex].Name == "HINHANH")
            {
                var hinhAnh = dgvSanPham.Rows[e.RowIndex].Cells["HINHANH"].Value?.ToString();
                if (!string.IsNullOrEmpty(hinhAnh))
                {
                    try
                    {
                        string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
                        string imagePath = Path.Combine(projectRoot, "img_product", hinhAnh);

                        if (File.Exists(imagePath))
                        {
                            pictureBoxPreview.Image = Image.FromFile(imagePath);
                            
                            var cellRect = dgvSanPham.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                            int x = dgvSanPham.Location.X + cellRect.Right + 10;
                            int y = dgvSanPham.Location.Y + cellRect.Top;
                            
                            if (x + pictureBoxPreview.Width > this.ClientSize.Width)
                                x = dgvSanPham.Location.X + cellRect.Left - pictureBoxPreview.Width - 10;
                            if (y + pictureBoxPreview.Height > this.ClientSize.Height)
                                y = this.ClientSize.Height - pictureBoxPreview.Height - 10;
                            
                            pictureBoxPreview.Location = new Point(x, y);
                            pictureBoxPreview.Visible = true;
                            pictureBoxPreview.BringToFront();
                        }
                    }
                    catch { pictureBoxPreview.Visible = false; }
                }
            }
        }

        private void DgvSanPham_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && 
                dgvSanPham.Columns[e.ColumnIndex].Name == "HINHANH")
            {
                pictureBoxPreview.Visible = false;
                if (pictureBoxPreview.Image != null)
                {
                    pictureBoxPreview.Image.Dispose();
                    pictureBoxPreview.Image = null;
                }
            }
        }
        private List<SanPhamDTO> GetSanPhamKhaDung()
        {
            // B1: Lấy tất cả sản phẩm đang hoạt động
            var listSP = sanPhamBUS.GetAll().Where(p => p.TT == 1).ToList();

            // B2: Lấy tất cả chi tiết phiếu đang chờ duyệt từ BUS
            // (Lưu ý: phieuXuatBUS đã bao gồm logic gọi DAO chi tiết)
            var listChiTietCho = phieuXuatBUS.GetChiTietPhieuChoDuyet();

            // B3: Tổng hợp số lượng đang bị giữ
            Dictionary<int, int> luongGiuCho = new Dictionary<int, int>();
            foreach (var ct in listChiTietCho)
            {
                if (luongGiuCho.ContainsKey(ct.MSP))
                    luongGiuCho[ct.MSP] += ct.SL;
                else
                    luongGiuCho.Add(ct.MSP, ct.SL);
            }

            // B4: Trừ tồn kho thực tế
            foreach (var sp in listSP)
            {
                if (luongGiuCho.ContainsKey(sp.MSP))
                {
                    sp.SL -= luongGiuCho[sp.MSP];
                }
            }

            // B5: Trả về SP còn tồn > 0
            return listSP.Where(p => p.SL > 0).ToList();
        }
    }
}