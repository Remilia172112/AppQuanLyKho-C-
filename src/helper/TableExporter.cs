using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using src.DTO;

namespace src.Helper
{
    public class TableExporter
    {
        // Tôi đã thêm tham số tùy chọn 'prefix' để bạn có thể đổi "NV" thành cái khác nếu muốn
        public static void ExportTableToExcel(DataGridView table, string prefix = "NU")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Chọn đường dẫn lưu file Excel";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.RestoreDirectory = true;

            // --- ĐOẠN CODE ĐƯỢC THÊM MỚI ---
            // Lấy ngày hiện tại theo định dạng dd-MM-yyyy
            string dateStr = DateTime.Now.ToString("dd-MM-yyyy");
            // Gán tên file mặc định: NV-12-12-2025
            saveFileDialog.FileName = $"{prefix}-{dateStr}";
            // ---------------------------------

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                
                try
                {
                    // Thiết lập LicenseContext cho EPPlus (Bắt buộc với bản mới)
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // 1. Header
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = table.Columns[i].HeaderText;
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        }

                        // 2. Data
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                object cellValue = table.Rows[i].Cells[j].Value;
                                // Kiểm tra null an toàn hơn
                                worksheet.Cells[i + 2, j + 1].Value = cellValue?.ToString() ?? "";
                            }
                        }

                        // Tự động điều chỉnh độ rộng cột
                        worksheet.Cells.AutoFitColumns();
                        
                        package.SaveAs(new FileInfo(filePath));
                    }

                    MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public class ExcelHelper
    {
        // Phương thức đọc file Excel và trả về List<LoaiSanPhamDTO>
        public static List<LoaiSanPhamDTO> ReadLoaiSanPhamFromExcel()
        {
            List<LoaiSanPhamDTO> listResult = new List<LoaiSanPhamDTO>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn file Excel để nhập liệu";
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Cấu hình License cho EPPlus (Bắt buộc)
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        // Lấy sheet đầu tiên
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        if (worksheet == null)
                        {
                            MessageBox.Show("File Excel không có dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }

                        // Giả sử dòng 1 là Header, dữ liệu bắt đầu từ dòng 2
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            // Đọc dữ liệu từng cột (Giả sử: Cột 1 = Tên Loại, Cột 2 = Ghi Chú)
                            string tenLoai = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            string ghiChu = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? "";

                            if (!string.IsNullOrEmpty(tenLoai))
                            {
                                LoaiSanPhamDTO lsp = new LoaiSanPhamDTO();
                                // Mã LSP sẽ tự tăng trong DB, không cần đọc từ Excel
                                lsp.TEN = tenLoai;
                                lsp.GHICHU = ghiChu;
                                lsp.TT = 1; // Mặc định là đang hoạt động

                                listResult.Add(lsp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đọc file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            return listResult;
        }
        public static List<SanPhamDTO> ReadSanPhamFromExcel()
        {
            List<SanPhamDTO> listResult = new List<SanPhamDTO>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn file Excel nhập Sản phẩm";
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        if (worksheet == null) return null;

                        int rowCount = worksheet.Dimension.Rows;

                        // Giả sử File Excel có cấu trúc cột:
                        // 1: Tên SP | 2: Danh mục | 3: Mã NSX | 4: Mã KV | 5: Mã Loại | 6: Giá Nhập | 7: Giá Xuất | 8: Số Lượng
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string tenSP = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            
                            if (!string.IsNullOrEmpty(tenSP))
                            {
                                SanPhamDTO sp = new SanPhamDTO();
                                sp.TEN = tenSP;
                                sp.DANHMUC = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? "";
                                
                                // Parse các cột số (có xử lý lỗi nếu ô trống hoặc sai định dạng)
                                int.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out int mnsx);
                                sp.MNSX = mnsx;

                                int.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out int mkvk);
                                sp.MKVK = mkvk;

                                int.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out int mlsp);
                                sp.MLSP = mlsp;

                                int.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out int gianhap);
                                sp.TIENN = gianhap;

                                int.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out int giaxuat);
                                sp.TIENX = giaxuat;

                                int.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out int soluong);
                                sp.SL = soluong;

                                sp.HINHANH = ""; // Mặc định trống ảnh
                                sp.TT = 1; // Hoạt động

                                listResult.Add(sp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            return listResult;
        }
    }
}