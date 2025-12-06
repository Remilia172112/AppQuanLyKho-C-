using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;

namespace src.GUI.NghiepVu
{
    public partial class ChonSanPhamXuatDialog : Form
    {
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();
        public ChiTietPhieuXuatDTO SelectedItem { get; private set; }

        private ChiTietPhieuXuatDTO editingItem = null;
        private int currentStockLevel = 0;

        public ChonSanPhamXuatDialog(ChiTietPhieuXuatDTO existingItem = null)
        {
            this.editingItem = existingItem;
            InitializeComponent();
            
            // Set dynamic form title based on mode
            this.Text = editingItem == null ? "Chọn sản phẩm xuất" : "Sửa sản phẩm xuất";
            
            LoadData();
            if (existingItem != null)
            {
                LoadExistingItem();
            }
        }

        private void LoadData()
        {
            try
            {
                // LẤY CHỈ SẢN PHẨM CÓ TỒN KHO VÀ ĐANG HOẠT ĐỘNG
                var list = sanPhamBUS.GetAll()
                    .Where(p => p.TT == 1 && p.SL > 0)
                    .ToList();
                
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
                        
                        // Load stock level
                        currentStockLevel = Convert.ToInt32(row.Cells["SL"].Value);
                        txtTonKho.Text = currentStockLevel.ToString();
                        nudSoLuong.Maximum = currentStockLevel;
                        
                        break;
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // TÌM KIẾM CHỈ TRONG SẢN PHẨM CÓ TỒN KHO VÀ ĐANG HOẠT ĐỘNG
                var list = sanPhamBUS.GetAll()
                    .Where(p => p.TT == 1 && p.SL > 0)
                    .ToList();
                
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
                var gia = dgvSanPham.Rows[e.RowIndex].Cells["TIENX"].Value;
                if (gia != null)
                {
                    txtGia.Text = gia.ToString();
                }

                // Lấy tồn kho và giới hạn số lượng xuất
                var tonKho = dgvSanPham.Rows[e.RowIndex].Cells["SL"].Value;
                if (tonKho != null)
                {
                    currentStockLevel = Convert.ToInt32(tonKho);
                    txtTonKho.Text = currentStockLevel.ToString();
                    
                    // Giới hạn số lượng xuất không vượt quá tồn kho
                    nudSoLuong.Maximum = currentStockLevel;
                    
                    if (nudSoLuong.Value > currentStockLevel)
                    {
                        nudSoLuong.Value = currentStockLevel;
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
            if (nudSoLuong.Value > currentStockLevel)
            {
                MessageBox.Show(
                    $"Số lượng xuất không được vượt quá tồn kho hiện tại ({currentStockLevel})!", 
                    "Thông báo", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia <= 0)
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
                MKM = 0  // Không có khuyến mãi mặc định
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
