using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;

namespace src.GUI.NghiepVu
{
    public partial class ChonSanPhamDialog : Form
    {
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();
        public ChiTietPhieuNhapDTO SelectedItem { get; private set; }

        private ChiTietPhieuNhapDTO editingItem = null;

        public ChonSanPhamDialog(ChiTietPhieuNhapDTO existingItem = null)
        {
            this.editingItem = existingItem;
            InitializeComponent();
            
            // Set dynamic form title based on mode
            this.Text = editingItem == null ? "Chọn sản phẩm" : "Sửa sản phẩm";
            
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
                var list = sanPhamBUS.GetAll();
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
                txtGia.Text = editingItem.TIENNHAP.ToString();

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
                var list = sanPhamBUS.GetAll();
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
                var gia = dgvSanPham.Rows[e.RowIndex].Cells["TIENN"].Value;
                if (gia != null)
                {
                    txtGia.Text = gia.ToString();
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

            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int msp = Convert.ToInt32(dgvSanPham.SelectedRows[0].Cells["MSP"].Value);
            SelectedItem = new ChiTietPhieuNhapDTO
            {
                MPN = editingItem?.MPN ?? 0,
                MSP = msp,
                SL = (int)nudSoLuong.Value,
                TIENNHAP = (int)gia,
                HINHTHUC = editingItem?.HINHTHUC ?? 0  // Mặc định 0 (nhập thường)
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
