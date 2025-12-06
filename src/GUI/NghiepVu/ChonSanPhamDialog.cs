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

        // UI Controls
        private Panel pnlSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private DataGridView dgvSanPham;
        private Panel pnlInput;
        private Label lblSoLuong;
        private NumericUpDown nudSoLuong;
        private Label lblGia;
        private TextBox txtGia;
        private Button btnChon;
        private Button btnHuy;

        private ChiTietPhieuNhapDTO editingItem = null;

        public ChonSanPhamDialog(ChiTietPhieuNhapDTO existingItem = null)
        {
            this.editingItem = existingItem;
            InitializeComponent();
            LoadData();
            if (existingItem != null)
            {
                LoadExistingItem();
            }
        }

        private void InitializeComponent()
        {
            this.Text = editingItem == null ? "Chọn sản phẩm" : "Sửa sản phẩm";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Panel Search
            pnlSearch = new Panel();
            pnlSearch.Location = new Point(20, 20);
            pnlSearch.Size = new Size(860, 50);

            Label lblSearch = new Label();
            lblSearch.Text = "Tìm kiếm sản phẩm:";
            lblSearch.Location = new Point(10, 15);
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(150, 12);
            txtSearch.Size = new Size(500, 25);
            txtSearch.PlaceholderText = "Nhập tên hoặc mã sản phẩm...";

            btnSearch = new Button();
            btnSearch.Text = "Tìm";
            btnSearch.Location = new Point(670, 10);
            btnSearch.Size = new Size(80, 30);
            btnSearch.BackColor = Color.FromArgb(8, 133, 204);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;

            pnlSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch });

            // DataGridView
            dgvSanPham = new DataGridView();
            dgvSanPham.Location = new Point(20, 80);
            dgvSanPham.Size = new Size(860, 350);
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.MultiSelect = false;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AllowUserToDeleteRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.BackgroundColor = Color.White;
            dgvSanPham.RowHeadersVisible = false;
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(8, 133, 204);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSanPham.EnableHeadersVisualStyles = false;
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            // Add columns
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MSP", HeaderText = "Mã SP", DataPropertyName = "MSP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TEN", HeaderText = "Tên sản phẩm", DataPropertyName = "TEN" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "SL", HeaderText = "Tồn kho", DataPropertyName = "SL" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TIENN", HeaderText = "Giá", DataPropertyName = "TIENN" });

            dgvSanPham.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSanPham.Columns["TIENN"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["TIENN"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Panel Input
            pnlInput = new Panel();
            pnlInput.Location = new Point(20, 440);
            pnlInput.Size = new Size(860, 60);
            pnlInput.BorderStyle = BorderStyle.FixedSingle;

            lblSoLuong = new Label();
            lblSoLuong.Text = "Số lượng:";
            lblSoLuong.Location = new Point(20, 20);
            lblSoLuong.AutoSize = true;

            nudSoLuong = new NumericUpDown();
            nudSoLuong.Location = new Point(100, 17);
            nudSoLuong.Size = new Size(150, 25);
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000000;
            nudSoLuong.Value = 1;

            lblGia = new Label();
            lblGia.Text = "Đơn giá:";
            lblGia.Location = new Point(280, 20);
            lblGia.AutoSize = true;

            txtGia = new TextBox();
            txtGia.Location = new Point(350, 17);
            txtGia.Size = new Size(200, 25);
            txtGia.Text = "0";

            btnChon = new Button();
            btnChon.Text = "Chọn";
            btnChon.Location = new Point(650, 12);
            btnChon.Size = new Size(90, 35);
            btnChon.BackColor = Color.FromArgb(40, 167, 69);
            btnChon.ForeColor = Color.White;
            btnChon.FlatStyle = FlatStyle.Flat;
            btnChon.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnChon.Click += BtnChon_Click;

            btnHuy = new Button();
            btnHuy.Text = "Hủy";
            btnHuy.Location = new Point(750, 12);
            btnHuy.Size = new Size(90, 35);
            btnHuy.BackColor = Color.Gray;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnHuy.Click += BtnHuy_Click;

            pnlInput.Controls.AddRange(new Control[] {
                lblSoLuong, nudSoLuong, lblGia, txtGia, btnChon, btnHuy
            });

            // Add to form
            this.Controls.AddRange(new Control[] {
                pnlSearch, dgvSanPham, pnlInput
            });
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
