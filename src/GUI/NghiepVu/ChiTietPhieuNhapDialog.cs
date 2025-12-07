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
    public enum DialogMode
    {
        View,
        Add,
        Edit
    }

    public partial class ChiTietPhieuNhapDialog : Form
    {
        private DialogMode mode;
        private int? maphieu;
        private PhieuNhapBUS phieuNhapBUS = new PhieuNhapBUS();
        private NhaCungCapBUS nhaCungCapBUS = new NhaCungCapBUS();
        private NhanVienBUS nhanVienBUS = new NhanVienBUS();
        private List<ChiTietPhieuNhapDTO> danhSachChiTiet = new List<ChiTietPhieuNhapDTO>();

        public ChiTietPhieuNhapDialog(DialogMode mode, int? maphieu = null)
        {
            this.mode = mode;
            this.maphieu = maphieu;
            InitializeComponent();
            LoadData();      // Phải gọi TRƯỚC SetupUIByMode để load dữ liệu
            SetupUIByMode(); // Sau đó mới setup UI theo mode
        }

        private void SetupUIByMode()
        {
            switch (mode)
            {
                case DialogMode.View:
                    lblTitle.Text = "XEM CHI TIẾT PHIẾU NHẬP";
                    cboNhaCungCap.Enabled = false;
                    cboNhanVien.Enabled = false;
                    dtpThoiGian.Enabled = false;
                    btnThemSP.Visible = false;
                    btnSuaSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnLuu.Visible = false;
                    btnHuy.Text = "Đóng";
                    break;

                case DialogMode.Add:
                    lblTitle.Text = "THÊM PHIẾU NHẬP MỚI";
                    txtMaPhieu.Text = "(Tự động)";
                    txtTrangThai.Text = "Chờ duyệt";
                    dtpThoiGian.Value = DateTime.Now;
                    // Set current user as default
                    cboNhanVien.SelectedValue = SessionManager.CurrentUser?.MNV;
                    cboNhanVien.Enabled = false;
                    break;

                case DialogMode.Edit:
                    lblTitle.Text = "SỬA PHIẾU NHẬP";
                    cboNhanVien.Enabled = false; // Cannot change employee
                    break;
            }
        }

        private void LoadData()
        {
            try
            {
                // Load NCC
                var nccList = nhaCungCapBUS.GetAll();
                cboNhaCungCap.DataSource = nccList;
                cboNhaCungCap.DisplayMember = "TEN";
                cboNhaCungCap.ValueMember = "MNCC";

                // Load NV
                var nvList = nhanVienBUS.GetAll();
                cboNhanVien.DataSource = nvList;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MNV";

                if (mode != DialogMode.Add && maphieu.HasValue)
                {
                    // Load phieu data
                    PhieuNhapDTO phieu = phieuNhapBUS.GetById(maphieu.Value);
                    if (phieu != null)
                    {
                        txtMaPhieu.Text = phieu.MPN.ToString();
                        cboNhaCungCap.SelectedValue = phieu.MNCC;
                        cboNhanVien.SelectedValue = phieu.MNV;
                        dtpThoiGian.Value = phieu.TG;
                        txtTrangThai.Text = phieu.TT == 1 ? "Đã duyệt" : (phieu.TT == 2 ? "Chờ duyệt" : "Đã xóa");

                        // Load chi tiet
                        danhSachChiTiet = phieuNhapBUS.GetChiTietPhieu(maphieu.Value);
                        LoadChiTietGrid();
                    }
                }
                else
                {
                    danhSachChiTiet = new List<ChiTietPhieuNhapDTO>();
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
                    GIA = ct.TIENNHAP,
                    ThanhTien = ct.SL * ct.TIENNHAP
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
            decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENNHAP);
            txtTongTien.Text = tongTien.ToString("N0");
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            ChonSanPhamDialog dialog = new ChonSanPhamDialog();
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
                        existing.TIENNHAP = newItem.TIENNHAP;
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
                ChonSanPhamDialog dialog = new ChonSanPhamDialog(item);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var updatedItem = dialog.SelectedItem;
                    item.SL = updatedItem.SL;
                    item.TIENNHAP = updatedItem.TIENNHAP;
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
                if (cboNhaCungCap.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tongTien = danhSachChiTiet.Sum(ct => ct.SL * ct.TIENNHAP);

                if (mode == DialogMode.Add)
                {
                    // Validate HINHTHUC cho tất cả chi tiết
                    foreach (var ct in danhSachChiTiet)
                    {
                        if (ct.HINHTHUC == 0)
                        {
                            // Mặc định nếu chưa set (để tương thích code cũ)
                            ct.HINHTHUC = 0;
                        }
                    }

                    // Create new phieu
                    PhieuNhapDTO phieu = new PhieuNhapDTO
                    {
                        MNCC = (int)cboNhaCungCap.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Chờ duyệt
                    };

                    int newMPN = phieuNhapBUS.Add(phieu, danhSachChiTiet);
                    if (newMPN > 0)
                    {
                        MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm phiếu nhập thất bại! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (mode == DialogMode.Edit && maphieu.HasValue)
                {
                    // Update existing phieu
                    PhieuNhapDTO phieu = new PhieuNhapDTO
                    {
                        MPN = maphieu.Value,
                        MNCC = (int)cboNhaCungCap.SelectedValue,
                        MNV = (int)cboNhanVien.SelectedValue,
                        TG = dtpThoiGian.Value,
                        TIEN = (int)tongTien,
                        TT = 2 // Keep status
                    };

                    int result = phieuNhapBUS.Update(phieu, danhSachChiTiet);
                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật phiếu nhập thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
