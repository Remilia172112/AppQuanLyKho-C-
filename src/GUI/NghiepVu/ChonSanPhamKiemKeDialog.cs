using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;

namespace src.GUI.NghiepVu
{
    /// <summary>
    /// Dialog chọn sản phẩm cho kiểm kê - cho phép chọn nhiều sản phẩm cùng lúc
    /// Khác với ChonSanPhamDialog: không cần nhập số lượng/giá, chỉ cần checkbox để chọn
    /// Cho phép chọn cả sản phẩm hết hàng (SL=0) để kiểm kê
    /// </summary>
    public partial class ChonSanPhamKiemKeDialog : Form
    {
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();
        public List<int> SelectedProductIds { get; private set; } = new List<int>();

        public ChonSanPhamKiemKeDialog()
        {
            InitializeComponent();
            LoadData();
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

        private void BtnChon_Click(object sender, EventArgs e)
        {
            // Lấy tất cả các dòng được check
            SelectedProductIds.Clear();
            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                if (row.Cells["colSelect"].Value != null && (bool)row.Cells["colSelect"].Value)
                {
                    int msp = Convert.ToInt32(row.Cells["MSP"].Value);
                    SelectedProductIds.Add(msp);
                }
            }

            if (SelectedProductIds.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                row.Cells["colSelect"].Value = true;
            }
        }

        private void BtnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                row.Cells["colSelect"].Value = false;
            }
        }
    }
}
