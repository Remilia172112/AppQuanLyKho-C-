using System;
using System.IO;
using System.Windows.Forms; // Dùng cho DataGridView, SaveFileDialog
using OfficeOpenXml; // Thư viện EPPlus

namespace src.Helper
{
    public class JTableExporter
    {
        // Hàm export DataGridView ra Excel
        public static void ExportJTableToExcel(DataGridView table)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Chọn đường dẫn lưu file Excel";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx"; // Chỉ cho phép file xlsx
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                
                try
                {
                    // Cấu hình License (Bắt buộc với EPPlus 5+)
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage package = new ExcelPackage())
                    {
                        // Tạo sheet mới
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // 1. Ghi tiêu đề cột (Header)
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            // Excel bắt đầu từ dòng 1, cột 1
                            worksheet.Cells[1, i + 1].Value = table.Columns[i].HeaderText;
                            
                            // Style cho đẹp (In đậm)
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        }

                        // 2. Ghi dữ liệu dòng (Data Rows)
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                // Lấy giá trị ô
                                object cellValue = table.Rows[i].Cells[j].Value;
                                
                                // Ghi vào Excel (Dòng bắt đầu từ 2)
                                worksheet.Cells[i + 2, j + 1].Value = cellValue?.ToString() ?? "";
                            }
                        }

                        // 3. Tự động giãn cột cho đẹp (AutoFit)
                        worksheet.Cells.AutoFitColumns();

                        // 4. Lưu file xuống ổ cứng
                        FileInfo fileInfo = new FileInfo(filePath);
                        package.SaveAs(fileInfo);
                    }

                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}