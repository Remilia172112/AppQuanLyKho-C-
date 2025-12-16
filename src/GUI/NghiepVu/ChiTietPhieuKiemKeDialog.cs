using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.NghiepVu
{
    // ViewModel để hiển thị trên lưới
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
            try
            {
                InitializeComponent();
                
                // 1. Tạo cột thủ công (Quan trọng để tránh lỗi cột không tồn tại)
                InitializeDataGridView(); 
                
                // 2. Load dữ liệu
                LoadData();
                
                // 3. Setup giao diện (Ẩn/Hiện nút)
                SetupUIByMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridView()
        {
            dgvChiTiet.Columns.Clear();
            dgvChiTiet.AutoGenerateColumns = false; // Tắt tự động tạo cột

            // Cột Mã SP
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MSP",
                DataPropertyName = "MSP",
                HeaderText = "Mã SP",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Tên SP
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                DataPropertyName = "TenSP",
                HeaderText = "Tên sản phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            // Cột Tồn hiện tại
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TonHienTai",
                DataPropertyName = "TonHienTai",
                HeaderText = "Tồn hệ thống",
                Width = 110,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, BackColor = Color.FromArgb(240, 240, 240) }
            });

            // Cột SL Thực tế (Cho phép sửa)
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongThucTe",
                DataPropertyName = "SoLuongThucTe",
                HeaderText = "SL Thực tế ✎",
                Width = 110,
                ReadOnly = (mode == DialogMode.View), // Chỉ cho sửa khi không phải chế độ Xem
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Blue,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            // Cột Chênh lệch
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ChenhLech",
                DataPropertyName = "ChenhLech",
                HeaderText = "Chênh lệch",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Giá trị chênh lệch
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaTriChenhLech",
                DataPropertyName = "GiaTriChenhLech",
                HeaderText = "Giá trị CL",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Ghi chú (Cho phép sửa)
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                DataPropertyName = "GhiChu",
                HeaderText = "Ghi chú ✎",
                Width = 150,
                ReadOnly = (mode == DialogMode.View)
            });
        }

        private void SetupUIByMode()
        {
            try
            {
                // Cấu hình chung
                cboNhanVien.Enabled = false;
                dtpThoiGian.Enabled = (mode == DialogMode.Add);
                
                switch (mode)
                {
                    case DialogMode.View:
                        lblTitle.Text = "XEM CHI TIẾT PHIẾU KIỂM KÊ";
                        dgvChiTiet.ReadOnly = true;
                        
                        btnThemSP.Visible = false;
                        btnXoaSP.Visible = false;
                        btnLuu.Visible = false;
                        
                        btnXuatPDF.Visible = true; // Hiện nút PDF
                        btnHuy.Text = "Đóng";
                        break;

                    case DialogMode.Add:
                        lblTitle.Text = "THÊM PHIẾU KIỂM KÊ MỚI";
                        txtMaPhieu.Text = phieuKiemKeBUS.getAutoIncrement().ToString();
                        txtTrangThai.Text = "Chờ duyệt";
                        dtpThoiGian.Value = DateTime.Now;
                        
                        if (SessionManager.CurrentEmployee != null)
                            cboNhanVien.SelectedValue = SessionManager.CurrentEmployee.MNV;
                        
                        btnLuu.Visible = true;
                        btnLuu.Text = "Tạo";
                        btnXuatPDF.Visible = false; // Ẩn nút PDF
                        break;

                    case DialogMode.Edit:
                        lblTitle.Text = "SỬA PHIẾU KIỂM KÊ";
                        
                        btnLuu.Visible = true;
                        btnLuu.Text = "Lưu";
                        btnXuatPDF.Visible = false; // Ẩn nút PDF
                        break;
                }
            }
            catch { }
        }

        private void LoadData()
        {
            try
            {
                // Load Nhân viên
                var nvList = nhanVienBUS.GetAll();
                if (cboNhanVien != null)
                {
                    cboNhanVien.DataSource = nvList ?? new List<NhanVienDTO>();
                    cboNhanVien.DisplayMember = "HOTEN";
                    cboNhanVien.ValueMember = "MNV";
                }

                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    PhieuKiemKeDTO phieu = phieuKiemKeBUS.GetById(maphieu.Value);
                    if (phieu != null)
                    {
                        txtMaPhieu.Text = phieu.MPKK.ToString();
                        cboNhanVien.SelectedValue = phieu.MNV;
                        dtpThoiGian.Value = phieu.TG;
                        txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                        // Load chi tiết
                        danhSachChiTiet = phieuKiemKeBUS.GetChiTietPhieu(maphieu.Value) ?? new List<ChiTietPhieuKiemKeDTO>();
                        LoadChiTietGrid();
                    }
                }
                else
                {
                    danhSachChiTiet = new List<ChiTietPhieuKiemKeDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietGrid()
        {
            try
            {
                var displayList = new System.ComponentModel.BindingList<ChiTietKiemKeViewModel>();
                
                foreach (var ct in danhSachChiTiet)
                {
                    var sp = sanPhamBUS.GetByMaSP(ct.MSP);
                    string tenSP = sp?.TEN ?? "Sản phẩm không tồn tại";
                    int tonHienTai = sp?.SL ?? 0;
                    decimal giaNhap = sp?.TIENN ?? 0;

                    int soLuongThucTe = ct.TRANGTHAISP; 
                    int chenhLech = soLuongThucTe - tonHienTai;
                    decimal giaTriCL = chenhLech * giaNhap; 

                    displayList.Add(new ChiTietKiemKeViewModel
                    {
                        MSP = ct.MSP,
                        TenSP = tenSP,
                        TonHienTai = tonHienTai,
                        SoLuongThucTe = soLuongThucTe,
                        GiaTriChenhLech = giaTriCL,
                        GhiChu = ct.GHICHU ?? ""
                    });
                }

                dgvChiTiet.DataSource = displayList;
                
                // Tô màu các dòng chênh lệch
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.Cells["ChenhLech"].Value != null)
                    {
                        int cl = 0;
                        int.TryParse(row.Cells["ChenhLech"].Value.ToString(), out cl);
                        if (cl < 0) 
                        {
                            row.Cells["ChenhLech"].Style.ForeColor = Color.Red;
                            row.Cells["GiaTriChenhLech"].Style.ForeColor = Color.Red;
                        }
                        else if (cl > 0) 
                        {
                            row.Cells["ChenhLech"].Style.ForeColor = Color.Green;
                            row.Cells["GiaTriChenhLech"].Style.ForeColor = Color.Green;
                        }
                    }
                }

                CalculateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị lưới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateStatistics()
        {
            if (lblTongSP == null || lblThongKe == null) return;

            int tongSP = danhSachChiTiet.Count;
            int slThieu = 0;
            int slThua = 0;
            decimal giaTriThieu = 0;
            decimal giaTriThua = 0;

            var bindingList = dgvChiTiet.DataSource as System.ComponentModel.BindingList<ChiTietKiemKeViewModel>;
            if (bindingList != null)
            {
                foreach (var item in bindingList)
                {
                    if (item.ChenhLech < 0)
                    {
                        slThieu += Math.Abs(item.ChenhLech);
                        giaTriThieu += Math.Abs(item.GiaTriChenhLech);
                    }
                    else if (item.ChenhLech > 0)
                    {
                        slThua += item.ChenhLech;
                        giaTriThua += item.GiaTriChenhLech;
                    }
                }
            }

            lblTongSP.Text = $"Tổng SP: {tongSP}";
            lblThongKe.Text = $"Thiếu: {slThieu} ({(giaTriThieu):N0}đ) | Thừa: {slThua} ({(giaTriThua):N0}đ)";
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            ChonSanPhamKiemKeDialog dialog = new ChonSanPhamKiemKeDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (int msp in dialog.SelectedProductIds)
                {
                    if (danhSachChiTiet.Any(x => x.MSP == msp)) continue;

                    var sp = sanPhamBUS.GetByMaSP(msp);
                    if (sp != null)
                    {
                        danhSachChiTiet.Add(new ChiTietPhieuKiemKeDTO
                        {
                            MSP = msp,
                            MPKK = maphieu ?? 0,
                            TRANGTHAISP = sp.SL, 
                            GHICHU = ""
                        });
                    }
                }
                LoadChiTietGrid();
            }
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null) return;
            if (dgvChiTiet.CurrentRow.Cells["MSP"].Value == null) return;

            int msp = Convert.ToInt32(dgvChiTiet.CurrentRow.Cells["MSP"].Value);
            var item = danhSachChiTiet.FirstOrDefault(x => x.MSP == msp);
            
            if (item != null)
            {
                danhSachChiTiet.Remove(item);
                LoadChiTietGrid();
            }
        }

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;

            if (dgvChiTiet.Rows[e.RowIndex].DataBoundItem is ChiTietKiemKeViewModel item)
            {
                if (item.SoLuongThucTe < 0)
                {
                    MessageBox.Show("Số lượng thực tế không được âm!", "Cảnh báo");
                    item.SoLuongThucTe = item.TonHienTai;
                }

                var sp = sanPhamBUS.GetByMaSP(item.MSP);
                decimal giaNhap = sp?.TIENN ?? 0;
                item.GiaTriChenhLech = (item.SoLuongThucTe - item.TonHienTai) * giaNhap;

                dgvChiTiet.Refresh();
                CalculateStatistics();
            }
        }

        private void DgvChiTiet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvChiTiet.Columns.Contains("SoLuongThucTe") && e.ColumnIndex == dgvChiTiet.Columns["SoLuongThucTe"].Index)
            {
                dgvChiTiet.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Nhập số lượng kiểm đếm thực tế tại đây";
            }
        }

        private void SyncDataFromGridToDTO()
        {
            var bindingList = dgvChiTiet.DataSource as System.ComponentModel.BindingList<ChiTietKiemKeViewModel>;
            if (bindingList == null) return;

            danhSachChiTiet.Clear();
            foreach (var viewItem in bindingList)
            {
                danhSachChiTiet.Add(new ChiTietPhieuKiemKeDTO
                {
                    MSP = viewItem.MSP,
                    MPKK = maphieu ?? 0,
                    TRANGTHAISP = viewItem.SoLuongThucTe,
                    GHICHU = viewItem.GhiChu ?? ""
                });
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                SyncDataFromGridToDTO();

                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Danh sách kiểm kê đang trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PhieuKiemKeDTO phieu = new PhieuKiemKeDTO
                {
                    MNV = (int)cboNhanVien.SelectedValue,
                    TG = dtpThoiGian.Value,
                    TT = 2 
                };

                bool success = false;

                if (mode == DialogMode.Add)
                {
                    success = phieuKiemKeBUS.Add(phieu, danhSachChiTiet); 
                }
                else if (mode == DialogMode.Edit)
                {
                    phieu.MPKK = maphieu.Value;
                    success = phieuKiemKeBUS.Update(phieu, danhSachChiTiet);
                }

                if (success)
                {
                    MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnXuatPDF_Click(object sender, EventArgs e)
        {
            if (maphieu.HasValue)
            {
                try
                {
                    new WritePDF().WritePKK(maphieu.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất PDF: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng lưu phiếu trước khi xuất PDF!", "Thông báo");
            }
        }
    }
}