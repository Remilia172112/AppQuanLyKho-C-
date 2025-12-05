using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml; // Thư viện EPPlus cho Excel
using src.DAO;
using src.DTO;
using src.Helper;

namespace src.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAO nhanVienDAO = NhanVienDAO.Instance;
        private readonly TaiKhoanDAO taiKhoanDAO = TaiKhoanDAO.Instance;
        public List<NhanVienDTO> listNv = new List<NhanVienDTO>();

        public NhanVienBUS()
        {
            // Lấy danh sách nhân viên đang hoạt động (TT=1)
            listNv = nhanVienDAO.selectAll();
        }

        public List<NhanVienDTO> GetAll()
        {
            return this.listNv;
        }

        public NhanVienDTO GetByIndex(int index)
        {
            return this.listNv[index];
        }

        public int GetIndexById(int manv)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listNv.Count && vitri == -1)
            {
                if (listNv[i].MNV == manv)
                {
                    vitri = i;
                }
                else
                {
                    i++;
                }
            }
            return vitri;
        }

        public string GetNameById(int manv)
        {
            var nv = nhanVienDAO.selectById(manv.ToString());
            return nv != null ? nv.HOTEN : "";
        }

        // Lấy thông tin nhân viên theo ID
        public NhanVienDTO? GetById(int manv)
        {
            return nhanVienDAO.selectById(manv.ToString());
        }

        public string[] GetArrTenNhanVien()
        {
            int size = listNv.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listNv[i].HOTEN;
            }
            return result;
        }

        // --- Các hàm xử lý logic (Thay thế cho actionPerformed) ---

        // Thêm nhân viên mới
        public bool Add(NhanVienDTO nv)
        {
            int result = nhanVienDAO.insert(nv);
            if (result > 0)
            {
                listNv.Add(nv);
                return true;
            }
            return false;
        }

        // Cập nhật thông tin nhân viên
        public bool Update(NhanVienDTO nv)
        {
            int result = nhanVienDAO.update(nv);
            if (result > 0)
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
        public void InsertNv(NhanVienDTO nv)
        {
            listNv.Add(nv);
        }

        // [Deprecated] Chỉ cập nhật cache - sử dụng Update() thay thế
        public void UpdateNv(int index, NhanVienDTO nv)
        {
            listNv[index] = nv;
        }

        public bool DeleteNv(NhanVienDTO nv)
        {
            // Xóa nhân viên và tài khoản liên quan
            var nvDAO = NhanVienDAO.Instance;
            var tkDAO = TaiKhoanDAO.Instance;

            // Xóa tài khoản trước (nếu có khóa ngoại)
            tkDAO.delete(nv.MNV.ToString());
            
            // Xóa nhân viên
            nvDAO.delete(nv.MNV.ToString());

            // Xóa khỏi danh sách bộ nhớ
            listNv.RemoveAll(n => n.MNV == nv.MNV);
            return true;
        }

        // --- Tìm kiếm ---

        // Tìm kiếm theo text và tiêu chí (ComboBox)
        public List<NhanVienDTO> Search(string text, string type)
        {
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            text = text.ToLower();

            switch (type)
            {
                case "Tất cả":
                    foreach (NhanVienDTO i in this.listNv)
                    {
                        if (i.HOTEN.ToLower().Contains(text) || 
                            i.EMAIL.ToLower().Contains(text) || 
                            i.SDT.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Họ tên":
                    foreach (NhanVienDTO i in this.listNv)
                    {
                        if (i.HOTEN.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Email":
                    foreach (NhanVienDTO i in this.listNv)
                    {
                        if (i.EMAIL.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Số điện thoại": // Thêm case này cho đủ bộ nếu cần
                     foreach (NhanVienDTO i in this.listNv)
                    {
                        if (i.SDT.ToLower().Contains(text))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }
            return result;
        }

        // --- Xử lý Excel ---

        // Xuất danh sách nhân viên ra file Excel
        // Trả về true nếu thành công, throw exception nếu lỗi
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

                // Data
                int rowIndex = 2;
                foreach (NhanVienDTO nv in list)
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
        // Trả về số dòng lỗi, throw exception nếu lỗi nghiêm trọng
        public int ImportFromExcelFile(string filePath)
        {
            int errorCount = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                int rowCount = sheet.Dimension.Rows;

                // Bắt đầu từ dòng 2 (bỏ header)
                for (int row = 2; row <= rowCount; row++)
                {
                    try 
                    {
                        int id = NhanVienDAO.Instance.getAutoIncrement();
                        string tennv = sheet.Cells[row, 1].Text;
                        string gioitinhStr = sheet.Cells[row, 2].Text;
                        int gt = (gioitinhStr.Equals("Nam", StringComparison.OrdinalIgnoreCase)) ? 1 : 0;
                        
                        // Xử lý ngày sinh (Excel lưu ngày là số hoặc chuỗi)
                        DateTime ngaysinh;
                        var cellDate = sheet.Cells[row, 3].Value;
                        if (cellDate is DateTime dt)
                        {
                            ngaysinh = dt;
                        }
                        else
                        {
                            // Thử parse chuỗi
                            if (!DateTime.TryParse(sheet.Cells[row, 3].Text, out ngaysinh))
                            {
                                errorCount++;
                                continue; // Lỗi ngày
                            }
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

                        // Tạo DTO và Insert
                        NhanVienDTO nvdto = new NhanVienDTO(id, tennv, gt, ngaysinh, sdt, 1, email);
                        NhanVienDAO.Instance.insert(nvdto);
                        
                        // Thêm vào list hiện tại để cập nhật GUI
                        listNv.Add(nvdto);
                    }
                    catch 
                    {
                        errorCount++; // Lỗi dòng này thì bỏ qua, đếm lỗi
                    }
                }
            }
            
            return errorCount;
        }
    }
}