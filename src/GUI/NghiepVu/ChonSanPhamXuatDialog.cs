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
        private Label lblTonKho;
        private TextBox txtTonKho;
        private Button btnChon;
        private Button btnHuy;

        private ChiTietPhieuXuatDTO editingItem = null;
        private int currentStockLevel = 0;

        public ChonSanPhamXuatDialog(ChiTietPhieuXuatDTO existingItem = null)
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
            this.Text = editingItem == null ? "Chọn sản phẩm xuất" : "Sửa sản phẩm xuất";
            this.Size = new System.Drawing.Size(950, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Panel Search
            pnlSearch = new Panel();
            pnlSearch.Location = new Point(20, 20);
            pnlSearch.Size = new Size(910, 50);

            Label lblSearch = new Label();
            lblSearch.Text = "Tìm kiếm sản phẩm:";
            lblSearch.Location = new Point(10, 15);
            lblSearch.AutoSize = true;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(150, 12);
            txtSearch.Size = new Size(520, 25);
            txtSearch.PlaceholderText = "Nhập tên hoặc mã sản phẩm...";

            btnSearch = new Button();
            btnSearch.Text = "Tìm";
            btnSearch.Location = new Point(690, 10);
            btnSearch.Size = new Size(80, 30);
            btnSearch.BackColor = Color.FromArgb(8, 133, 204);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;

            pnlSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch });

            // DataGridView
            dgvSanPham = new DataGridView();
            dgvSanPham.Location = new Point(20, 80);
            dgvSanPham.Size = new Size(910, 380);
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.MultiSelect = false;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AllowUserToDeleteRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.BackgroundColor = Color.White;
            dgvSanPham.RowHeadersVisible = false;
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSanPham.EnableHeadersVisualStyles = false;
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            // Add columns
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MSP", HeaderText = "Mã SP", DataPropertyName = "MSP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TEN", HeaderText = "Tên sản phẩm", DataPropertyName = "TEN" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "SL", HeaderText = "Tồn kho", DataPropertyName = "SL" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TIENX", HeaderText = "Giá xuất", DataPropertyName = "TIENX" });

            dgvSanPham.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSanPham.Columns["TIENX"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["TIENX"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Panel Input
            pnlInput = new Panel();
            pnlInput.Location = new Point(20, 470);
            pnlInput.Size = new Size(910, 100);
            pnlInput.BorderStyle = BorderStyle.FixedSingle;

            // Row 1: Tồn kho hiện tại
            lblTonKho = new Label();
            lblTonKho.Text = "Tồn kho hiện tại:";
            lblTonKho.Location = new Point(20, 20);
            lblTonKho.AutoSize = true;

            txtTonKho = new TextBox();
            txtTonKho.Location = new Point(140, 17);
            txtTonKho.Size = new Size(150, 25);
            txtTonKho.ReadOnly = true;
            txtTonKho.BackColor = Color.LightGray;
            txtTonKho.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            txtTonKho.ForeColor = Color.Blue;

            // Row 2: Số lượng xuất và giá
            lblSoLuong = new Label();
            lblSoLuong.Text = "Số lượng xuất:";
            lblSoLuong.Location = new Point(20, 55);
            lblSoLuong.AutoSize = true;

            nudSoLuong = new NumericUpDown();
            nudSoLuong.Location = new Point(140, 52);
            nudSoLuong.Size = new Size(150, 25);
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000000;
            nudSoLuong.Value = 1;

            lblGia = new Label();
            lblGia.Text = "Đơn giá xuất:";
            lblGia.Location = new Point(320, 55);
            lblGia.AutoSize = true;

            txtGia = new TextBox();
            txtGia.Location = new Point(430, 52);
            txtGia.Size = new Size(200, 25);
            txtGia.Text = "0";

            btnChon = new Button();
            btnChon.Text = "Chọn";
            btnChon.Location = new Point(700, 12);
            btnChon.Size = new Size(90, 75);
            btnChon.BackColor = Color.FromArgb(40, 167, 69);
            btnChon.ForeColor = Color.White;
            btnChon.FlatStyle = FlatStyle.Flat;
            btnChon.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnChon.Click += BtnChon_Click;

            btnHuy = new Button();
            btnHuy.Text = "Hủy";
            btnHuy.Location = new Point(800, 12);
            btnHuy.Size = new Size(90, 75);
            btnHuy.BackColor = Color.Gray;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnHuy.Click += BtnHuy_Click;

            pnlInput.Controls.AddRange(new Control[] {
                lblTonKho, txtTonKho,
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
