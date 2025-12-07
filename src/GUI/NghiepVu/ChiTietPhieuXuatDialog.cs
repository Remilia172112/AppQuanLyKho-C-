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
            InitializeData();
            SetupUIByMode();
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
                    // Set current user as default
                    if (SessionManager.CurrentEmployee != null)
                    {
                        cboNhanVien.SelectedValue = SessionManager.CurrentEmployee.MNV;
                    }
                    cboNhanVien.Enabled = false;
                    break;

                case DialogMode.Edit:
                    lblTitle.Text = "SỬA PHIẾU XUẤT";
                    cboNhanVien.Enabled = false; // Cannot change employee
                    break;
            }
        }

        private void InitializeData()
        {
            try
            {
                // Load Khách hàng - CHỈ LẤY KHÁCH HÀNG ĐANG HOẠT ĐỘNG (TT = 1)
                var khList = khachHangBUS.GetAll().Where(k => k.TT == 1).ToList();
                cboKhachHang.DataSource = khList;
                cboKhachHang.DisplayMember = "TEN";
                cboKhachHang.ValueMember = "MKH";

                // Load Nhân viên - CHỈ LẤY NHÂN VIÊN ĐANG HOẠT ĐỘNG (TT = 1)
                var nvList = nhanVienBUS.GetAll().Where(nv => nv.TT == 1).ToList();
                cboNhanVien.DataSource = nvList;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";

                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    // Load phieu data
                    PhieuXuatDTO phieu = phieuXuatBUS.GetById(maphieu.Value);
                    if (phieu != null)
                    {
                        txtMaPhieu.Text = phieu.MPX.ToString();
                        cboKhachHang.SelectedValue = phieu.MKH;
                        cboNhanVien.SelectedValue = phieu.MNV;
                        dtpThoiGian.Value = phieu.TG;
                        txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                        // Load chi tiet
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
                    TenSP = sanPhamBUS.GetByMaSP(ct.MSP)?.TEN ?? "",
                    SL = ct.SL,
                    GIA = ct.TIENXUAT,
                    ThanhTien = ct.SL * ct.TIENXUAT
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
            decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENXUAT);
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
                    // Check if product already exists
                    var existing = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == newItem.MSP);
                    if (existing != null)
                    {
                        // Update quantity
                        existing.SL += newItem.SL;
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

            int msp = Convert.ToInt32(dgvChiTiet.SelectedRows[0].Cells["MSP"].Value);
            var item = danhSachChiTiet.FirstOrDefault(ct => ct.MSP == msp);

            if (item != null)
            {
                ChonSanPhamXuatDialog dialog = new ChonSanPhamXuatDialog(item);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var updatedItem = dialog.SelectedItem;
                    item.SL = updatedItem.SL;
                    item.TIENXUAT = updatedItem.TIENXUAT;
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
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                danhSachChiTiet.RemoveAll(ct => ct.MSP == msp);
                LoadChiTietGrid();
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (cboKhachHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENXUAT);

                if (mode == DialogMode.Add)
                {
                    // Kiểm tra tồn kho trước khi thêm
                    if (!phieuXuatBUS.CheckSLPx(danhSachChiTiet))
                    {
                        MessageBox.Show("Không đủ hàng trong kho! Vui lòng kiểm tra lại số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create new phieu
                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MKH = (int)cboKhachHang.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Chờ duyệt
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
                    // Kiểm tra tồn kho trước khi sửa
                    if (!phieuXuatBUS.CheckSLPx(danhSachChiTiet))
                    {
                        MessageBox.Show("Không đủ hàng trong kho! Vui lòng kiểm tra lại số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update existing phieu
                    PhieuXuatDTO phieu = new PhieuXuatDTO
                    {
                        MPX = maphieu.Value,
                        MKH = (int)cboKhachHang.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Keep status
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
