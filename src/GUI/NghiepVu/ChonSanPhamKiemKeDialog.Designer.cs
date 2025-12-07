using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class ChonSanPhamKiemKeDialog
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private TextBox txtSearch;
        private Button btnSearch;
        private DataGridView dgvSanPham;
        private Button btnChon;
        private Button btnHuy;
        private Button btnChonTatCa;
        private Button btnBoChonTatCa;
        private Label lblSoLuongDaChon;
        private DataGridViewCheckBoxColumn colSelect;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.dgvSanPham = new DataGridView();
            this.btnChon = new Button();
            this.btnHuy = new Button();
            this.btnChonTatCa = new Button();
            this.btnBoChonTatCa = new Button();
            this.lblSoLuongDaChon = new Label();
            this.colSelect = new DataGridViewCheckBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Size = new Size(860, 30);
            this.lblTitle.Text = "CHỌN SẢN PHẨM KIỂM KÊ";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // txtSearch
            this.txtSearch.Font = new Font("Segoe UI", 10F);
            this.txtSearch.Location = new Point(20, 60);
            this.txtSearch.Size = new Size(700, 25);
            this.txtSearch.PlaceholderText = "Tìm kiếm theo mã hoặc tên sản phẩm...";

            // btnSearch
            this.btnSearch.BackColor = Color.FromArgb(0, 122, 204);
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.Location = new Point(730, 58);
            this.btnSearch.Size = new Size(150, 30);
            this.btnSearch.Text = "🔍 Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += BtnSearch_Click;

            // dgvSanPham
            this.dgvSanPham.AllowUserToAddRows = false;
            this.dgvSanPham.AllowUserToDeleteRows = false;
            this.dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPham.BackgroundColor = Color.White;
            this.dgvSanPham.ColumnHeadersHeight = 40;
            this.dgvSanPham.Location = new Point(20, 100);
            this.dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSanPham.Size = new Size(860, 400);
            this.dgvSanPham.ReadOnly = false;
            
            // Add checkbox column first
            this.colSelect.HeaderText = "Chọn";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 60;  // Fixed width instead of FillWeight
            this.colSelect.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvSanPham.Columns.Add(this.colSelect);

            // btnChonTatCa
            this.btnChonTatCa.BackColor = Color.FromArgb(46, 125, 50);
            this.btnChonTatCa.FlatStyle = FlatStyle.Flat;
            this.btnChonTatCa.Font = new Font("Segoe UI", 9F);
            this.btnChonTatCa.ForeColor = Color.White;
            this.btnChonTatCa.Location = new Point(20, 515);
            this.btnChonTatCa.Size = new Size(150, 35);
            this.btnChonTatCa.Text = "✓ Chọn tất cả";
            this.btnChonTatCa.UseVisualStyleBackColor = false;
            this.btnChonTatCa.Click += BtnChonTatCa_Click;

            // btnBoChonTatCa
            this.btnBoChonTatCa.BackColor = Color.FromArgb(211, 47, 47);
            this.btnBoChonTatCa.FlatStyle = FlatStyle.Flat;
            this.btnBoChonTatCa.Font = new Font("Segoe UI", 9F);
            this.btnBoChonTatCa.ForeColor = Color.White;
            this.btnBoChonTatCa.Location = new Point(180, 515);
            this.btnBoChonTatCa.Size = new Size(150, 35);
            this.btnBoChonTatCa.Text = "✗ Bỏ chọn tất cả";
            this.btnBoChonTatCa.UseVisualStyleBackColor = false;
            this.btnBoChonTatCa.Click += BtnBoChonTatCa_Click;

            // lblSoLuongDaChon
            this.lblSoLuongDaChon.AutoSize = false;
            this.lblSoLuongDaChon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSoLuongDaChon.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblSoLuongDaChon.Location = new Point(340, 515);
            this.lblSoLuongDaChon.Size = new Size(200, 35);
            this.lblSoLuongDaChon.Text = "Đã chọn: 0 sản phẩm";
            this.lblSoLuongDaChon.TextAlign = ContentAlignment.MiddleLeft;

            // btnChon
            this.btnChon.BackColor = Color.FromArgb(0, 122, 204);
            this.btnChon.FlatStyle = FlatStyle.Flat;
            this.btnChon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnChon.ForeColor = Color.White;
            this.btnChon.Location = new Point(650, 515);
            this.btnChon.Size = new Size(110, 35);
            this.btnChon.Text = "✓ Chọn";
            this.btnChon.UseVisualStyleBackColor = false;
            this.btnChon.Click += BtnChon_Click;

            // btnHuy
            this.btnHuy.BackColor = Color.FromArgb(158, 158, 158);
            this.btnHuy.FlatStyle = FlatStyle.Flat;
            this.btnHuy.Font = new Font("Segoe UI", 10F);
            this.btnHuy.ForeColor = Color.White;
            this.btnHuy.Location = new Point(770, 515);
            this.btnHuy.Size = new Size(110, 35);
            this.btnHuy.Text = "✗ Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += BtnHuy_Click;

            // ChonSanPhamKiemKeDialog
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(900, 570);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvSanPham);
            this.Controls.Add(this.btnChonTatCa);
            this.Controls.Add(this.btnBoChonTatCa);
            this.Controls.Add(this.lblSoLuongDaChon);
            this.Controls.Add(this.btnChon);
            this.Controls.Add(this.btnHuy);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChonSanPhamKiemKeDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chọn sản phẩm kiểm kê";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            this.ResumeLayout(false);

            // Update count label when checkbox changes
            this.dgvSanPham.CellValueChanged += (s, e) =>
            {
                if (e.ColumnIndex == 0 && e.RowIndex >= 0) // checkbox column
                {
                    int count = dgvSanPham.Rows.Cast<DataGridViewRow>()
                        .Count(r => r.Cells["colSelect"].Value != null && (bool)r.Cells["colSelect"].Value);
                    lblSoLuongDaChon.Text = $"Đã chọn: {count} sản phẩm";
                }
            };

            // Commit edit immediately for checkbox
            this.dgvSanPham.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvSanPham.IsCurrentCellDirty && dgvSanPham.CurrentCell.ColumnIndex == 0)
                {
                    dgvSanPham.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
        }
    }
}
