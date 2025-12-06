using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

namespace src.Helper
{
    public class JTableExporter
    {
        public static void ExportJTableToExcel(DataGridView table)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Chọn đường dẫn lưu file Excel";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                
                try
                {

                    ExcelPackage.License.SetNonCommercialPersonal("Student");

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
                                worksheet.Cells[i + 2, j + 1].Value = cellValue?.ToString() ?? "";
                            }
                        }

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
}