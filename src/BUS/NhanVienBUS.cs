using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using src.DAO;
using src.DTO;
using src.Helper;

namespace src.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAO nhanVienDAO = NhanVienDAO.Instance;
        private readonly TaiKhoanDAO taiKhoanDAO = TaiKhoanDAO.Instance;
        private List<NhanVienDTO> listNv = new List<NhanVienDTO>();

        public NhanVienBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listNv = nhanVienDAO.selectAll() ?? new List<NhanVienDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadData NhanVienBUS: {ex.Message}");
                listNv = new List<NhanVienDTO>();
            }
        }

        public List<NhanVienDTO> GetAll() => listNv;

        public NhanVienDTO GetByIndex(int index) => listNv[index];

        // LINQ: Tìm index theo mã nhân viên
        public int GetIndexById(int manv)
            => listNv.FindIndex(nv => nv.MNV == manv);

        // LINQ: Lấy tên theo mã (từ cache)
        public string GetNameById(int manv)
            => listNv.FirstOrDefault(nv => nv.MNV == manv)?.HOTEN ?? "";

        // Lấy thông tin nhân viên theo ID từ DB
        public NhanVienDTO? GetById(int manv)
            => nhanVienDAO.selectById(manv.ToString());

        // LINQ: Lấy nhân viên từ cache theo mã
        public NhanVienDTO? GetFromCacheById(int manv)
            => listNv.FirstOrDefault(nv => nv.MNV == manv);

        // LINQ: Lấy mảng tên nhân viên
        public string[] GetArrTenNhanVien()
            => listNv.Select(nv => nv.HOTEN).ToArray();

        // Thêm nhân viên mới
        public bool Add(NhanVienDTO nv)
        {
            if (nhanVienDAO.insert(nv) > 0)
            {
                LoadData(); // Reload để lấy đúng ID auto increment
                return true;
            }
            return false;
        }

        // Cập nhật thông tin nhân viên
        public bool Update(NhanVienDTO nv)
        {
            if (nhanVienDAO.update(nv) > 0)
            {
                int index = GetIndexById(nv.MNV);
                if (index != -1)
                {
                    listNv[index] = nv;
                }
                return true;
            }
            return false;
        }

        // [Deprecated] Chỉ cập nhật cache - sử dụng Add() thay thế
        [Obsolete("Use Add() instead")]
        public void InsertNv(NhanVienDTO nv) => listNv.Add(nv);

        // [Deprecated] Chỉ cập nhật cache - sử dụng Update() thay thế
        [Obsolete("Use Update() instead")]
        public void UpdateNv(int index, NhanVienDTO nv) => listNv[index] = nv;

        // Xóa nhân viên
        public bool DeleteNv(NhanVienDTO nv)
        {
            // Xóa tài khoản trước (nếu có khóa ngoại)
            taiKhoanDAO.delete(nv.MNV.ToString());

            // Xóa nhân viên
            nhanVienDAO.delete(nv.MNV.ToString());

            // LINQ: Xóa khỏi danh sách bộ nhớ
            listNv.RemoveAll(n => n.MNV == nv.MNV);
            return true;
        }

        // LINQ: Search với nhiều tiêu chí
        public List<NhanVienDTO> Search(string text, string type)
        {
            text = text.ToLower();
            IEnumerable<NhanVienDTO> query = listNv;

            switch (type)
            {
                case "Họ tên":
                    query = query.Where(nv => nv.HOTEN.ToLower().Contains(text));
                    break;
                case "Email":
                    query = query.Where(nv => nv.EMAIL.ToLower().Contains(text));
                    break;
                case "Số điện thoại":
                    query = query.Where(nv => nv.SDT.ToLower().Contains(text));
                    break;
                default: // Tất cả
                    query = query.Where(nv =>
                        nv.HOTEN.ToLower().Contains(text) ||
                        nv.EMAIL.ToLower().Contains(text) ||
                        nv.SDT.ToLower().Contains(text));
                    break;
            }
            return query.ToList();
        }

        // LINQ: Tìm nhân viên theo email
        public NhanVienDTO? FindByEmail(string email)
            => listNv.FirstOrDefault(nv => nv.EMAIL.Equals(email, StringComparison.OrdinalIgnoreCase));

        // LINQ: Tìm nhân viên theo số điện thoại
        public NhanVienDTO? FindByPhone(string phone)
            => listNv.FirstOrDefault(nv => nv.SDT == phone);

        // --- Xử lý Excel ---

        // Xuất danh sách nhân viên ra file Excel
        public void ExportToExcelFile(List<NhanVienDTO> list, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Nhân viên");

                // Header
                string[] header = { "MãNV", "Tên nhân viên", "Email nhân viên", "Số điện thoại", "Giới tính", "Ngày sinh" };
                for (int i = 0; i < header.Length; i++)
                {
                    sheet.Cells[1, i + 1].Value = header[i];
                    sheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // LINQ: Ghi data với index
                int rowIndex = 2;
                foreach (var nv in list)
                {
                    sheet.Cells[rowIndex, 1].Value = nv.MNV;
                    sheet.Cells[rowIndex, 2].Value = nv.HOTEN;
                    sheet.Cells[rowIndex, 3].Value = nv.EMAIL;
                    sheet.Cells[rowIndex, 4].Value = nv.SDT;
                    sheet.Cells[rowIndex, 5].Value = nv.GIOITINH == 1 ? "Nam" : "Nữ";
                    sheet.Cells[rowIndex, 6].Value = nv.NGAYSINH.ToString("dd/MM/yyyy");
                    rowIndex++;
                }

                sheet.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Nhập danh sách nhân viên từ file Excel
        public int ImportFromExcelFile(string filePath)
        {
            int errorCount = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                int rowCount = sheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        int id = NhanVienDAO.Instance.getAutoIncrement();
                        string tennv = sheet.Cells[row, 1].Text;
                        string gioitinhStr = sheet.Cells[row, 2].Text;
                        int gt = gioitinhStr.Equals("Nam", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

                        DateTime ngaysinh;
                        var cellDate = sheet.Cells[row, 3].Value;
                        if (cellDate is DateTime dt)
                        {
                            ngaysinh = dt;
                        }
                        else if (!DateTime.TryParse(sheet.Cells[row, 3].Text, out ngaysinh))
                        {
                            errorCount++;
                            continue;
                        }

                        string sdt = sheet.Cells[row, 4].Text;
                        string email = sheet.Cells[row, 5].Text;

                        // Validate dữ liệu
                        if (Validation.IsEmpty(tennv) ||
                            Validation.IsEmpty(email) || !Validation.IsEmail(email) ||
                            Validation.IsEmpty(sdt) || !Validation.IsPhoneNumber(sdt))
                        {
                            errorCount++;
                            continue;
                        }

                        NhanVienDTO nvdto = new NhanVienDTO(id, tennv, gt, ngaysinh, sdt, 1, email);
                        NhanVienDAO.Instance.insert(nvdto);
                        listNv.Add(nvdto);
                    }
                    catch
                    {
                        errorCount++;
                    }
                }
            }

            return errorCount;
        }
    }
}
