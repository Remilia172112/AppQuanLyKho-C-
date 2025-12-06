using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.DanhMuc
{
    public partial class QuanLyKhuVucKhoForm : Form
    {
        private KhuVucKhoBUS khuVucKhoBUS;
        private bool isEditing = false;
        private int currentMaKV = -1;

        public QuanLyKhuVucKhoForm()
        {
            InitializeComponent();
            khuVucKhoBUS = new KhuVucKhoBUS();
            InitializeDataGridView();
            LoadData();
            SetButtonStates(false);
        }

        private void InitializeDataGridView()
        {
            dgvKhuVucKho.Columns.Clear();

            // Column MKVK
            DataGridViewTextBoxColumn colMKVK = new DataGridViewTextBoxColumn
            {
                Name = "MKVK",
                DataPropertyName = "MKVK",
                HeaderText = "Mã khu vực",
                Width = 100
            };
            dgvKhuVucKho.Columns.Add(colMKVK);

            // Column TEN
            DataGridViewTextBoxColumn colTEN = new DataGridViewTextBoxColumn
            {
                Name = "TEN",
                DataPropertyName = "TEN",
                HeaderText = "Tên khu vực",
                Width = 200
            };
            dgvKhuVucKho.Columns.Add(colTEN);

            // Column GHICHU
            DataGridViewTextBoxColumn colGHICHU = new DataGridViewTextBoxColumn
            {
                Name = "GHICHU",
                DataPropertyName = "GHICHU",
                HeaderText = "Ghi chú",
                Width = 300
            };
            dgvKhuVucKho.Columns.Add(colGHICHU);

            // Column TT
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn
            {
                Name = "TT",
                DataPropertyName = "TT",
                HeaderText = "Trạng thái",
                Width = 120
            };
            dgvKhuVucKho.Columns.Add(colTT);

            // Style header
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvKhuVucKho.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void DgvKhuVucKho_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKhuVucKho.CurrentRow != null && !isEditing)
            {
                DisplayKhuVucKhoInfo();
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
            catch (Exception ex)
            {
                // Silent catch - không hiển thị lỗi cho user khi display info
            }
        }

        private void ClearForm()
        {
            txtMaKV.Clear();
            txtTenKV.Clear();
            txtGhiChu.Clear();
            currentMaKV = -1;
        }

        private void SetButtonStates(bool editing)
        {
            isEditing = editing;
            btnThem.Enabled = !editing && SessionManager.CanCreate("khuvuckho");
            btnSua.Enabled = !editing && SessionManager.CanUpdate("khuvuckho");
            btnXoa.Enabled = !editing && SessionManager.CanDelete("khuvuckho");
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            dgvKhuVucKho.Enabled = !editing;
            
            txtTenKV.ReadOnly = !editing;
            txtGhiChu.ReadOnly = !editing;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetButtonStates(true);
            txtMaKV.Text = "(Tự động)";
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
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa khu vực kho này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maKV = Convert.ToInt32(dgvKhuVucKho.CurrentRow.Cells["MKVK"].Value);
                        int index = dgvKhuVucKho.CurrentRow.Index;
                        var khuVucKho = khuVucKhoBUS.GetAll().Find(x => x.MKVK == maKV);
                        if (khuVucKho != null && khuVucKhoBUS.Delete(khuVucKho, index))
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
            SetButtonStates(false);
            if (dgvKhuVucKho.CurrentRow != null)
            {
                DisplayKhuVucKhoInfo();
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
    }
}
