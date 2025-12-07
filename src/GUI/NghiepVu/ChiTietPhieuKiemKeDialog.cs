using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.NghiepVu
{
    // ViewModel for editable grid binding
    public class ChiTietKiemKeViewModel
    {
        public int MSP { get; set; }
        public string TenSP { get; set; } = "";
        public int TonHienTai { get; set; }
        public int SoLuongThucTe { get; set; }
        public int ChenhLech => SoLuongThucTe - TonHienTai;
        public decimal GiaTriChenhLech { get; set; }
        public string GhiChu { get; set; } = "";
    }

    public partial class ChiTietPhieuKiemKeDialog : Form
    {
        private DialogMode mode;
        private int? maphieu;
        private PhieuKiemKeBUS phieuKiemKeBUS = new PhieuKiemKeBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();
        private SanPhamBUS sanPhamBUS = new SanPhamBUS();
        private List<ChiTietPhieuKiemKeDTO> danhSachChiTiet = new List<ChiTietPhieuKiemKeDTO>();

        public ChiTietPhieuKiemKeDialog(DialogMode mode, int? maphieu = null)
        {
            this.mode = mode;
            this.maphieu = maphieu;
            InitializeComponent();
            LoadData();
            SetupUIByMode();
        }

        private void SetupUIByMode()
        {
            switch (mode)
            {
                case DialogMode.View:
                    lblTitle.Text = "XEM CHI TIẾT PHIẾU KIỂM KÊ";
                    cboNhanVien.Enabled = false;
                    dtpThoiGian.Enabled = false;
                    dgvChiTiet.ReadOnly = true;
                    btnThemSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnLuu.Visible = false;
                    btnHuy.Text = "Đóng";
                    break;

                case DialogMode.Add:
                    lblTitle.Text = "THÊM PHIẾU KIỂM KÊ MỚI";
                    txtMaPhieu.Text = "(Tự động)";
                    txtTrangThai.Text = "Chờ duyệt";
                    dtpThoiGian.Value = DateTime.Now;
                    // Set current user as default - check if user is logged in
                    if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.MNV > 0)
                    {
                        cboNhanVien.SelectedValue = SessionManager.CurrentUser.MNV;
                    }
                    cboNhanVien.Enabled = false;
                    break;

                case DialogMode.Edit:
                    lblTitle.Text = "SỬA PHIẾU KIỂM KÊ";
                    cboNhanVien.Enabled = false;
                    break;
            }
        }

        private void LoadData()
        {
            try
            {
                // Load NV
                var nvList = nhanVienBUS.GetAll();
                if (nvList == null || nvList.Count == 0)
                {
                    MessageBox.Show("Không tải được danh sách nhân viên!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                cboNhanVien.DataSource = nvList;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";

                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    // Load phieu data
                    PhieuKiemKeDTO phieu = phieuKiemKeBUS.GetById(maphieu.Value);
                    if (phieu == null)
                    {
                        MessageBox.Show($"Không tìm thấy phiếu kiểm kê #{maphieu.Value}!", 
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    
                    txtMaPhieu.Text = phieu.MPKK.ToString();
                    cboNhanVien.SelectedValue = phieu.MNV;
                    dtpThoiGian.Value = phieu.TG;
                    txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                    // Load chi tiet
                    danhSachChiTiet = phieuKiemKeBUS.GetChiTietPhieu(maphieu.Value);
                    if (danhSachChiTiet == null)
                    {
                        danhSachChiTiet = new List<ChiTietPhieuKiemKeDTO>();
                    }
                    LoadChiTietGrid();
                }
                else
                {
                    danhSachChiTiet = new List<ChiTietPhieuKiemKeDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietGrid()
        {
            try
            {
                // Use BindingList for editable binding
                var displayList = new System.ComponentModel.BindingList<ChiTietKiemKeViewModel>();
                
                foreach (var ct in danhSachChiTiet)
                {
                    var sp = sanPhamBUS.GetByMaSP(ct.MSP);
                    if (sp == null)
                    {
                        Console.WriteLine($"Warning: Product MSP={ct.MSP} not found, skipping...");
                        continue;
                    }
                    
                    int soLuongThucTe = ct.TRANGTHAISP;  // Actual quantity found during audit
                    int tonHienTai = sp.SL;              // Current inventory in system
                    int chenhLech = soLuongThucTe - tonHienTai;
                    decimal giaTriChenhLech = chenhLech * sp.TIENN;

                    displayList.Add(new ChiTietKiemKeViewModel
                    {
                        MSP = ct.MSP,
                        TenSP = sp.TEN ?? "",
                        TonHienTai = tonHienTai,
                        SoLuongThucTe = soLuongThucTe,
                        GiaTriChenhLech = giaTriChenhLech,
                        GhiChu = ct.GHICHU ?? ""
                    });
                }

                // Clear and rebind to avoid ReadOnly issues
                dgvChiTiet.DataSource = null;
                dgvChiTiet.Columns.Clear();
                dgvChiTiet.DataSource = displayList;
                
                FormatDataGridView();
                CalculateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            try
            {
                if (dgvChiTiet.Columns.Count == 0) return;

                // Use safe Contains check before accessing columns
                if (dgvChiTiet.Columns.Contains("MSP"))
                {
                    dgvChiTiet.Columns["MSP"].HeaderText = "Mã SP";
                    dgvChiTiet.Columns["MSP"].Width = 70;
                    dgvChiTiet.Columns["MSP"].ReadOnly = true;
                }

                if (dgvChiTiet.Columns.Contains("TenSP"))
                {
                    dgvChiTiet.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                    dgvChiTiet.Columns["TenSP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvChiTiet.Columns["TenSP"].ReadOnly = true;
                }

                if (dgvChiTiet.Columns.Contains("TonHienTai"))
                {
                    dgvChiTiet.Columns["TonHienTai"].HeaderText = "Tồn hiện tại";
                    dgvChiTiet.Columns["TonHienTai"].Width = 100;
                    dgvChiTiet.Columns["TonHienTai"].ReadOnly = true;
                    dgvChiTiet.Columns["TonHienTai"].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvChiTiet.Columns["TonHienTai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvChiTiet.Columns.Contains("SoLuongThucTe"))
                {
                    dgvChiTiet.Columns["SoLuongThucTe"].HeaderText = "SL Thực tế ✎";
                    dgvChiTiet.Columns["SoLuongThucTe"].Width = 100;
                    dgvChiTiet.Columns["SoLuongThucTe"].ReadOnly = (mode == DialogMode.View);
                    
                    if (mode == DialogMode.View)
                    {
                        // View mode: Light gray background
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.BackColor = Color.LightGray;
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.SelectionBackColor = Color.Gray;
                    }
                    else
                    {
                        // Edit mode: White background with blue border effect
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.BackColor = Color.White;
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.ForeColor = Color.DarkBlue;
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.Font = new Font(dgvChiTiet.Font, FontStyle.Bold);
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
                        dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
                    }
                    
                    dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvChiTiet.Columns["SoLuongThucTe"].DefaultCellStyle.Padding = new Padding(5);
                }

                if (dgvChiTiet.Columns.Contains("ChenhLech"))
                {
                    dgvChiTiet.Columns["ChenhLech"].HeaderText = "Chênh lệch";
                    dgvChiTiet.Columns["ChenhLech"].Width = 100;
                    dgvChiTiet.Columns["ChenhLech"].ReadOnly = true;
                    dgvChiTiet.Columns["ChenhLech"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvChiTiet.Columns.Contains("GiaTriChenhLech"))
                {
                    dgvChiTiet.Columns["GiaTriChenhLech"].HeaderText = "Giá trị CL";
                    dgvChiTiet.Columns["GiaTriChenhLech"].Width = 120;
                    dgvChiTiet.Columns["GiaTriChenhLech"].ReadOnly = true;
                    dgvChiTiet.Columns["GiaTriChenhLech"].DefaultCellStyle.Format = "N0";
                    dgvChiTiet.Columns["GiaTriChenhLech"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvChiTiet.Columns.Contains("GhiChu"))
                {
                    dgvChiTiet.Columns["GhiChu"].HeaderText = "Ghi chú ✎";
                    dgvChiTiet.Columns["GhiChu"].Width = 200;
                    dgvChiTiet.Columns["GhiChu"].ReadOnly = (mode == DialogMode.View);
                    
                    if (mode == DialogMode.View)
                    {
                        // View mode: Light gray background
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.BackColor = Color.LightGray;
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.SelectionBackColor = Color.Gray;
                    }
                    else
                    {
                        // Edit mode: White background with green tint
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.BackColor = Color.White;
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.ForeColor = Color.DarkGreen;
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.Font = new Font(dgvChiTiet.Font, FontStyle.Italic);
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.SelectionBackColor = Color.LightGreen;
                        dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.SelectionForeColor = Color.DarkGreen;
                    }
                    
                    dgvChiTiet.Columns["GhiChu"].DefaultCellStyle.Padding = new Padding(5);
                }

                // Apply color coding for ChenhLech column
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.Cells["ChenhLech"]?.Value != null)
                    {
                        int chenhLech = Convert.ToInt32(row.Cells["ChenhLech"].Value);
                        if (chenhLech < 0)
                        {
                            row.Cells["ChenhLech"].Style.ForeColor = Color.Red;
                            row.Cells["ChenhLech"].Style.Font = new Font(dgvChiTiet.Font, FontStyle.Bold);
                            if (row.Cells["GiaTriChenhLech"] != null)
                                row.Cells["GiaTriChenhLech"].Style.ForeColor = Color.Red;
                        }
                        else if (chenhLech > 0)
                        {
                            row.Cells["ChenhLech"].Style.ForeColor = Color.Green;
                            row.Cells["ChenhLech"].Style.Font = new Font(dgvChiTiet.Font, FontStyle.Bold);
                            if (row.Cells["GiaTriChenhLech"] != null)
                                row.Cells["GiaTriChenhLech"].Style.ForeColor = Color.Green;
                        }
                        else
                        {
                            row.Cells["ChenhLech"].Style.ForeColor = Color.Gray;
                            if (row.Cells["GiaTriChenhLech"] != null)
                                row.Cells["GiaTriChenhLech"].Style.ForeColor = Color.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Cannot format grid: {ex.Message}");
            }
        }

        private void CalculateStatistics()
        {
            int tongSP = danhSachChiTiet.Count;
            int tongThieu = 0;
            int tongThua = 0;
            decimal giaTriThieu = 0;
            decimal giaTriThua = 0;

            foreach (var ct in danhSachChiTiet)
            {
                var sp = sanPhamBUS.GetByMaSP(ct.MSP);
                int tonHienTai = sp?.SL ?? 0;
                int soLuongThucTe = ct.TRANGTHAISP;  // Actual quantity
                int chenhLech = soLuongThucTe - tonHienTai;
                decimal giaSP = sp?.TIENN ?? 0;

                if (chenhLech < 0)
                {
                    tongThieu += Math.Abs(chenhLech);
                    giaTriThieu += Math.Abs(chenhLech) * giaSP;
                }
                else if (chenhLech > 0)
                {
                    tongThua += chenhLech;
                    giaTriThua += chenhLech * giaSP;
                }
            }

            lblTongSP.Text = $"Tổng SP: {tongSP}";
            lblThongKe.Text = $"Thiếu: {tongThieu} SP ({giaTriThieu:N0} đ) | Thừa: {tongThua} SP ({giaTriThua:N0} đ)";
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            ChonSanPhamKiemKeDialog dialog = new ChonSanPhamKiemKeDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (int msp in dialog.SelectedProductIds)
                {
                    // Check if already exists
                    if (danhSachChiTiet.Any(ct => ct.MSP == msp))
                    {
                        continue;
                    }

                    var sp = sanPhamBUS.GetByMaSP(msp);
                    if (sp != null)
                    {
                        // Add new item with current quantity as default
                        var newItem = new ChiTietPhieuKiemKeDTO
                        {
                            MPKK = maphieu ?? 0,
                            MSP = msp,
                            TRANGTHAISP = sp.SL,  // Default: actual quantity = current inventory
                            GHICHU = ""
                        };
                        danhSachChiTiet.Add(newItem);
                    }
                }

                LoadChiTietGrid();
            }
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int msp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
                danhSachChiTiet.RemoveAll(ct => ct.MSP == msp);
                LoadChiTietGrid();
            }
        }

        private void DgvChiTiet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || mode == DialogMode.View) return;

            try
            {
                string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
                
                // Show tooltip for editable columns
                if (columnName == "SoLuongThucTe")
                {
                    dgvChiTiet.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = 
                        "📝 Click để nhập số lượng thực tế (phím Enter để xác nhận)";
                }
                else if (columnName == "GhiChu")
                {
                    dgvChiTiet.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = 
                        "📝 Click để nhập ghi chú (ví dụ: hư hỏng, mất mát, sai sót...)";
                }
            }
            catch { }
        }

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || mode == DialogMode.View) return;

            try
            {
                // Get the current view model item
                var bindingList = dgvChiTiet.DataSource as System.ComponentModel.BindingList<ChiTietKiemKeViewModel>;
                if (bindingList == null || e.RowIndex >= bindingList.Count) return;
                
                var viewModel = bindingList[e.RowIndex];
                
                // Find corresponding DTO
                var item = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == viewModel.MSP);
                if (item == null) return;

                // Update SoLuongThucTe
                if (dgvChiTiet.Columns[e.ColumnIndex].Name == "SoLuongThucTe")
                {
                    int soLuongThucTe = viewModel.SoLuongThucTe;
                    
                    // Validation: Số lượng không được âm
                    if (soLuongThucTe < 0)
                    {
                        MessageBox.Show("Số lượng thực tế không được âm!\nVui lòng nhập lại.", 
                            "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        // Reset to previous value (ton hien tai)
                        viewModel.SoLuongThucTe = viewModel.TonHienTai;
                        dgvChiTiet.Refresh();
                        return;
                    }

                    // Update DTO
                    item.TRANGTHAISP = soLuongThucTe;
                    
                    // Update calculated fields in view model
                    var sp = sanPhamBUS.GetByMaSP(item.MSP);
                    int chenhLech = soLuongThucTe - viewModel.TonHienTai;
                    viewModel.GiaTriChenhLech = chenhLech * (sp?.TIENN ?? 0);
                    
                    // Refresh grid to update color coding
                    dgvChiTiet.Refresh();
                    CalculateStatistics();
                }

                // Update GhiChu
                if (dgvChiTiet.Columns[e.ColumnIndex].Name == "GhiChu")
                {
                    item.GHICHU = viewModel.GhiChu ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Final validation: check all quantities >= 0
                foreach (var item in danhSachChiTiet)
                {
                    if (item.TRANGTHAISP < 0)
                    {
                        MessageBox.Show("Có sản phẩm có số lượng âm!\nVui lòng kiểm tra lại.", 
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                PhieuKiemKeDTO phieu = new PhieuKiemKeDTO
                {
                    MNV = (int)cboNhanVien.SelectedValue,
                    TG = dtpThoiGian.Value,
                    TT = 2  // Chờ duyệt
                };

                if (mode == DialogMode.Add)
                {
                    if (phieuKiemKeBUS.Add(phieu, danhSachChiTiet))
                    {
                        MessageBox.Show("Thêm phiếu kiểm kê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else if (mode == DialogMode.Edit)
                {
                    // For edit: update phieu and details through BUS
                    phieu.MPKK = maphieu.Value;
                    
                    if (phieuKiemKeBUS.Update(phieu, danhSachChiTiet))
                    {
                        MessageBox.Show("Cập nhật phiếu kiểm kê thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật phiếu kiểm kê!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (!maphieu.HasValue)
                {
                    MessageBox.Show("Vui lòng lưu phiếu trước khi xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // TODO: Implement PDF export using writePDF helper
                MessageBox.Show("Chức năng xuất PDF đang được phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
