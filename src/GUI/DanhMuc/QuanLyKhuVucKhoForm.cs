using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.GUI.Components;
using src.Helper;

namespace src.GUI.DanhMuc
{
    // ViewModel cho hiển thị sản phẩm trong kho
    public class SanPhamKhoViewModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; } = "";
        public string LoaiSP { get; set; } = "";
        public int SoLuong { get; set; }
    }

    public partial class QuanLyKhuVucKhoForm : Form
    {
        private KhuVucKhoBUS khuVucKhoBUS;
        private SanPhamBUS sanPhamBUS;
        private LoaiSanPhamBUS loaiSanPhamBUS;
        private bool isEditing = false;
        private int currentMaKV = -1;

        public QuanLyKhuVucKhoForm()
        {
            InitializeComponent();
            khuVucKhoBUS = new KhuVucKhoBUS();
            sanPhamBUS = new SanPhamBUS();
            loaiSanPhamBUS = new LoaiSanPhamBUS();

            InitializeDataGridView();
            LoadData();
            SetButtonStates(false);
        }

        private void InitializeDataGridView()
        {
            // --- Grid Khu Vực Kho ---
            dgvKhuVucKho.Columns.Clear();
            
            // Mã KV
            dgvKhuVucKho.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "MKVK", 
                DataPropertyName = "MKVK", 
                HeaderText = "Mã KVK", 
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Tên KV
            dgvKhuVucKho.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TEN", 
                DataPropertyName = "TEN", 
                HeaderText = "Tên khu vực", 
                Width = 200 
            });

            // Ghi chú
            dgvKhuVucKho.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "GHICHU", 
                DataPropertyName = "GHICHU", 
                HeaderText = "Ghi chú", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
            });

            // Ẩn Trạng thái
            dgvKhuVucKho.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TT", 
                DataPropertyName = "TT", 
                Visible = false 
            });

            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Lưu ý: Grid Sản Phẩm đã được cấu hình trong Designer.cs
        }

        private void LoadData()
        {
            try
            {
                var khuVucKhoList = khuVucKhoBUS.GetAll();
                if (khuVucKhoList != null)
                {
                    dgvKhuVucKho.DataSource = new BindingList<KhuVucKhoDTO>(khuVucKhoList);
                }
                else
                {
                    dgvKhuVucKho.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. Sự kiện khi chọn dòng ở bảng Khu Vực
        private void DgvKhuVucKho_SelectionChanged(object? sender, EventArgs? e)
        {
            if (dgvKhuVucKho.CurrentRow != null && !isEditing)
            {
                DisplayKhuVucKhoInfo();

                // --- Logic Load Sản Phẩm ---
                try
                {
                    int mkvk = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                    string tenKhuVuc = dgvKhuVucKho.CurrentRow.Cells["TEN"].Value?.ToString() ?? "";

                    // Gọi BUS lấy sản phẩm theo khu vực
                    var listSP = sanPhamBUS.GetByMaKhuVuc(mkvk);

                    // Chuyển đổi sang ViewModel với tên loại sản phẩm
                    var viewModelList = new BindingList<SanPhamKhoViewModel>();
                    foreach (var sp in listSP)
                    {
                        var loaiSP = loaiSanPhamBUS.GetById(sp.MLSP);
                        viewModelList.Add(new SanPhamKhoViewModel
                        {
                            MaSP = sp.MSP,
                            TenSP = sp.TEN ?? "",
                            LoaiSP = loaiSP?.TEN ?? "Không xác định",
                            SoLuong = sp.SL
                        });
                    }

                    dgvSanPham.DataSource = viewModelList;
                    grpSanPham.Text = $"Sản phẩm tại: {tenKhuVuc} ({listSP.Count} sản phẩm)";
                }
                catch
                {
                    // Ignore errors during selection change
                }
            }
        }

        private void DisplayKhuVucKhoInfo()
        {
            try
            {
                if (dgvKhuVucKho.CurrentRow == null) return;

                var row = dgvKhuVucKho.CurrentRow;
                txtMaKV.Text = row.Cells["MKVK"].Value?.ToString() ?? "";
                txtTenKV.Text = row.Cells["TEN"].Value?.ToString() ?? "";
                txtGhiChu.Text = row.Cells["GHICHU"].Value?.ToString() ?? "";
            }
            catch { }
        }

        private void ClearForm()
        {
            txtMaKV.Text = khuVucKhoBUS.getAutoIncrement().ToString();
            txtTenKV.Clear();
            txtGhiChu.Clear();
            currentMaKV = -1;
            // Clear luôn bảng sản phẩm khi reset form
            dgvSanPham.DataSource = null;
            grpSanPham.Text = "Danh sách sản phẩm trong khu vực";
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;

            // Nhóm nút chức năng chính (Enable/Disable)
            btnThem.Enabled = !editing && SessionManager.CanCreate("khuvuckho");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("khuvuckho");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("khuvuckho");

            // Nhóm nút Lưu/Hủy (Ẩn/Hiện - Visible)
            btnLuu.Visible = editing;
            btnHuy.Visible = editing;

            // Khóa bảng khi đang sửa
            dgvKhuVucKho.Enabled = !editing;

            // Trạng thái các ô nhập liệu
            txtTenKV.ReadOnly = !editing;
            txtGhiChu.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtTenKV.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null)
            {
                currentMaKV = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                SetButtonStates(true);
                txtTenKV.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khu vực kho cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null)
            {
                // Kiểm tra xem khu vực có sản phẩm không trước khi xóa
                int mkvk = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                var listSP = sanPhamBUS.GetByMaKhuVuc(mkvk);
                if (listSP.Count > 0)
                {
                    MessageBox.Show($"Khu vực này đang chứa {listSP.Count} sản phẩm. Vui lòng chuyển sản phẩm sang kho khác trước khi xóa!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khu vực kho này?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // int index = dgvKhuVucKho.CurrentRow.Index;
                        var khuVucKho = khuVucKhoBUS.GetAll().Find(x => x.MKVK == mkvk);
                        if (khuVucKho != null && khuVucKhoBUS.Delete(khuVucKho))
                        {
                            MessageBox.Show("Xóa khu vực kho thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Xóa khu vực kho thất bại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khu vực kho cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khu vực!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKV.Focus();
                return;
            }

            try
            {
                var khuVucKho = new KhuVucKhoDTO
                {
                    TEN = txtTenKV.Text.Trim(),
                    GHICHU = txtGhiChu.Text.Trim(),
                    TT = 1
                };

                if (currentMaKV == -1) // Thêm mới
                {
                    if (khuVucKhoBUS.Add(khuVucKho))
                    {
                        MessageBox.Show("Thêm khu vực kho thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Thêm khu vực kho thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Cập nhật
                {
                    khuVucKho.MKVK = currentMaKV;
                    if (khuVucKhoBUS.Update(khuVucKho))
                    {
                        MessageBox.Show("Cập nhật khu vực kho thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                        SetButtonStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khu vực kho thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            // 1. Quay về chế độ xem
            SetButtonStates(false);

            // 2. Select lại dòng đầu tiên (Item 1)
            if (dgvKhuVucKho.Rows.Count > 0)
            {
                dgvKhuVucKho.ClearSelection();
                dgvKhuVucKho.Rows[0].Selected = true;
                
                // Đặt current cell về dòng đầu để focus chuẩn (tránh lỗi focus ảo)
                dgvKhuVucKho.CurrentCell = dgvKhuVucKho.Rows[0].Cells[0];
                
                // Gọi hàm hiển thị thông tin để load dữ liệu từ grid lên textbox
                DisplayKhuVucKhoInfo();
                
                // Trigger lại sự kiện selection changed để load sản phẩm của khu vực đó
                DgvKhuVucKho_SelectionChanged(null, null);
            }
            else
            {
                ClearForm();
            }
        }
        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string searchType = cboTimKiem.SelectedItem?.ToString() ?? "Tất cả";

            try
            {
                var result = khuVucKhoBUS.Search(keyword, searchType);
                dgvKhuVucKho.DataSource = new BindingList<KhuVucKhoDTO>(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTimKiem.SelectedIndex = 0;
            LoadData();
            ClearForm();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi Helper đọc file
                List<KhuVucKhoDTO> listNewData = ExcelHelper.ReadKhuVucKhoFromExcel();

                if (listNewData != null && listNewData.Count > 0)
                {
                    // Gọi BUS thêm hàng loạt
                    int count = khuVucKhoBUS.AddMany(listNewData);

                    if (count > 0)
                    {
                        MessageBox.Show($"Đã nhập thành công {count} khu vực kho!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại grid
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được dữ liệu nào (Có thể do lỗi DB).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhập Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKhuVucKho.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Xuất Grid Khu vực kho với Prefix "KVK"
                TableExporter.ExportTableToExcel(dgvKhuVucKho, "KVK");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}