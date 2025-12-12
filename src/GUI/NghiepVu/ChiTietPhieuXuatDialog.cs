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
    public partial class ChiTietPhieuXuatDialog : Form
    {
        private DialogMode mode;
        private int? maphieu;
        private PhieuXuatBUS phieuXuatBUS = new PhieuXuatBUS();
        private KhachHangBUS khachHangBUS = new KhachHangBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();
        private List<ChiTietPhieuXuatDTO> danhSachChiTiet = new List<ChiTietPhieuXuatDTO>();

        public ChiTietPhieuXuatDialog(DialogMode mode, int? maphieu = null)
        {
            this.mode = mode;
            this.maphieu = maphieu;
            InitializeComponent();
            InitializeData(); // Load dữ liệu cho ComboBox trước
            SetupUIByMode();  // Sau đó setup giao diện
        }

        private void SetupUIByMode()
        {
            switch (mode)
            {
                case DialogMode.View:
                    lblTitle.Text = "XEM CHI TIẾT PHIẾU XUẤT";
                    cboKhachHang.Enabled = false;
                    cboNhanVien.Enabled = false;
                    dtpThoiGian.Enabled = false;
                    btnThemSP.Visible = false;
                    btnSuaSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnLuu.Visible = false;
                    btnHuy.Text = "Đóng";
                    break;

                case DialogMode.Add:
                    lblTitle.Text = "THÊM PHIẾU XUẤT MỚI";
                    txtMaPhieu.Text = "(Tự động)";
                    txtTrangThai.Text = "Chờ duyệt";
                    dtpThoiGian.Value = DateTime.Now;
                    
                    // Mặc định chọn nhân viên đang đăng nhập
                    if (SessionManager.CurrentEmployee != null)
                    {
                        cboNhanVien.SelectedValue = SessionManager.CurrentEmployee.MNV;
                    }
                    cboNhanVien.Enabled = false; // Không cho đổi nhân viên tạo
                    break;

                case DialogMode.Edit:
                    lblTitle.Text = "SỬA PHIẾU XUẤT";
                    cboNhanVien.Enabled = false; 
                    break;
            }
        }

        private void InitializeData()
        {
            try
            {
                // --- 1. LOAD KHÁCH HÀNG ---
                var khList = khachHangBUS.GetAll().Where(k => k.TT == 1).ToList();
                cboKhachHang.DataSource = khList;
                // Hiển thị Tên (HOTEN), lấy giá trị là Mã (MKH)
                cboKhachHang.DisplayMember = "HOTEN"; 
                cboKhachHang.ValueMember = "MKH";

                // --- 2. LOAD NHÂN VIÊN ---
                var nvList = nhanVienBUS.GetAll().Where(nv => nv.TT == 1).ToList();
                cboNhanVien.DataSource = nvList;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";

                // --- 3. LOAD DỮ LIỆU PHIẾU (NẾU SỬA/XEM) ---
                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    PhieuXuatDTO phieu = phieuXuatBUS.GetById(maphieu.Value);
                    if (phieu != null)
                    {
                        txtMaPhieu.Text = phieu.MPX.ToString();
                        
                        // Tự động chọn đúng Khách hàng và Nhân viên dựa trên ValueMember (MKH, MNV)
                        cboKhachHang.SelectedValue = phieu.MKH;
                        cboNhanVien.SelectedValue = phieu.MNV;
                        
                        dtpThoiGian.Value = phieu.TG;
                        txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                        // Load chi tiết sản phẩm
                        danhSachChiTiet = phieuXuatBUS.SelectCTP(maphieu.Value);
                        LoadChiTietGrid();
                    }
                }
                else
                {
                    danhSachChiTiet = new List<ChiTietPhieuXuatDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietGrid()
        {
            try
            {
                var sanPhamBUS = new SanPhamBUS();
                var displayList = danhSachChiTiet.Select(ct => new
                {
                    MSP = ct.MSP,
                    TenSP = sanPhamBUS.GetByMaSP(ct.MSP)?.TEN ?? "Sản phẩm không tồn tại",
                    SL = ct.SL,
                    GIA = ct.TIENXUAT,
                    ThanhTien = (long)ct.SL * ct.TIENXUAT // Ép kiểu long để tránh tràn số
                }).ToList();

                dgvChiTiet.DataSource = displayList;
                CalculateTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTongTien()
        {
            decimal tongTien = danhSachChiTiet.Sum(ct => (decimal)ct.SL * ct.TIENXUAT);
            txtTongTien.Text = tongTien.ToString("N0");
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            ChonSanPhamXuatDialog dialog = new ChonSanPhamXuatDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newItem = dialog.SelectedItem;
                if (newItem != null)
                {
                    var existing = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == newItem.MSP);
                    if (existing != null)
                    {
                        existing.SL += newItem.SL;
                        // Giá xuất thường cố định hoặc lấy mới nhất, ở đây ta cập nhật theo cái mới chọn
                        existing.TIENXUAT = newItem.TIENXUAT; 
                    }
                    else
                    {
                        danhSachChiTiet.Add(newItem);
                    }
                    LoadChiTietGrid();
                }
            }
        }

        private void BtnSuaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oldMsp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
            var oldItem = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == oldMsp);

            if (oldItem != null)
            {
                // Truyền bản sao (Clone) để tránh sửa trực tiếp vào list khi chưa bấm OK
                var cloneItem = new ChiTietPhieuXuatDTO
                {
                    MSP = oldItem.MSP,
                    SL = oldItem.SL,
                    TIENXUAT = oldItem.TIENXUAT,
                    MKM = oldItem.MKM
                };

                ChonSanPhamXuatDialog dialog = new ChonSanPhamXuatDialog(cloneItem);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var newItem = dialog.SelectedItem;

                    // Trường hợp 1: Người dùng vẫn giữ nguyên sản phẩm cũ, chỉ sửa số lượng/giá
                    if (newItem.MSP == oldMsp)
                    {
                        oldItem.SL = newItem.SL;
                        oldItem.TIENXUAT = newItem.TIENXUAT;
                    }
                    // Trường hợp 2: Người dùng chọn sang sản phẩm khác
                    else
                    {
                        // Xóa sản phẩm cũ
                        danhSachChiTiet.Remove(oldItem);

                        // Kiểm tra xem sản phẩm mới đã có trong danh sách chưa
                        var existingNewItem = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == newItem.MSP);
                        if (existingNewItem != null)
                        {
                            // Nếu đã có -> Cộng dồn hoặc ghi đè (ở đây chọn cộng dồn số lượng, giá lấy mới nhất)
                            existingNewItem.SL += newItem.SL;
                            existingNewItem.TIENXUAT = newItem.TIENXUAT;
                        }
                        else
                        {
                            // Nếu chưa có -> Thêm mới
                            danhSachChiTiet.Add(newItem);
                        }
                    }
                    LoadChiTietGrid();
                }
            }
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int msp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi phiếu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                danhSachChiTiet.RemoveAll(ct => ct.MSP == msp);
                LoadChiTietGrid();
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validate chọn Khách hàng
                if (cboKhachHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKhachHang.Focus();
                    return;
                }

                // 2. Validate danh sách sản phẩm
                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tính tổng tiền
                long tongTien = (long)danhSachChiTiet.Sum(ct => (decimal)ct.SL * ct.TIENXUAT);

                // Lấy ID Khách hàng và Nhân viên từ ComboBox
                // .SelectedValue sẽ trả về object (do ValueMember="MKH"), cần ép kiểu về int
                int maKhachHang = Convert.ToInt32(cboKhachHang.SelectedValue);
                int maNhanVien = Convert.ToInt32(cboNhanVien.SelectedValue);

                if (mode == DialogMode.Add)
                {
                    // Kiểm tra tồn kho (Optional: tùy logic nghiệp vụ có cho phép xuất âm hay không)
                    if (!phieuXuatBUS.CheckSLPx(danhSachChiTiet))
                    {
                        MessageBox.Show("Không đủ hàng trong kho! Vui lòng kiểm tra lại số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MKH = maKhachHang, // Lưu Mã KH
                        MNV = maNhanVien,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Mặc định Chờ duyệt
                    };

                    int newMPX = phieuXuatBUS.Insert(phieu, danhSachChiTiet);
                    if (newMPX > 0)
                    {
                        MessageBox.Show("Thêm phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (mode == DialogMode.Edit && maphieu.HasValue)
                {
                    // Logic kiểm tra tồn kho khi sửa (nếu cần)
                    // ...

                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MPX = maphieu.Value,
                        MKH = maKhachHang, // Lưu Mã KH
                        MNV = maNhanVien,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Giữ trạng thái hoặc reset về chờ duyệt tùy nghiệp vụ
                    };

                    bool success = phieuXuatBUS.Update(phieu, danhSachChiTiet);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật phiếu xuất thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}