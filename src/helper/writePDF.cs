using System.Diagnostics;   // Dùng để mở file sau khi xuất
using iTextSharp.text;      // Thư viện PDF
using iTextSharp.text.pdf;  // Thư viện PDF
using src.DAO;
using src.DTO;

namespace src.Helper
{
    public class WritePDF
    {
        // Font chữ
        BaseFont bf;
        iTextSharp.text.Font fontNormal10;
        iTextSharp.text.Font fontBold15;
        iTextSharp.text.Font fontBold25;
        iTextSharp.text.Font fontBoldItalic15;
        public WritePDF()
        {
            try
            {
                // Cấu hình Font chữ: Cố gắng lấy font Times New Roman từ hệ thống để hỗ trợ Tiếng Việt
                // Nếu chạy trên Windows, font nằm ở C:\Windows\Fonts\times.ttf
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                
                // Nếu không tìm thấy font hệ thống, bạn có thể trỏ đến file font trong thư mục project của bạn
                if (!File.Exists(fontPath))
                {
                    // Fallback hoặc throw error, ở đây mình giả định là có font
                    fontPath = "Arial"; // Fallback tạm
                }

                // BaseFont.IDENTITY_H là quan trọng để hiển thị tiếng Việt Unicode
                bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                fontNormal10 = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                fontBold15 = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD);
                fontBold25 = new iTextSharp.text.Font(bf, 25, iTextSharp.text.Font.BOLD);
                fontBoldItalic15 = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLDITALIC);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo Font: " + ex.Message);
            }
        }

        // Hàm mở hộp thoại chọn nơi lưu file
        private string GetFile(string defaultName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất PDF";
            saveFileDialog.FileName = defaultName;
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        // Hàm mở file sau khi lưu
        private void OpenFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở file: " + ex.Message);
            }
        }

        // Hàm tạo khoảng trắng
        public static Chunk CreateWhiteSpace(int length)
        {
            return new Chunk(new string(' ', length));
        }

        // --- 1. XUẤT PHIẾU NHẬP (PN) ---
        public void WritePN(int maphieu)
        {
            string url = GetFile("PN" + maphieu);
            if (string.IsNullOrEmpty(url)) return;

            try
            {
                using (FileStream fs = new FileStream(url, FileMode.Create))
                {
                    Document document = new Document();
                    PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Header Công ty
                    Paragraph company = new Paragraph("Hệ thống quản lý kho hàng BestKho", fontBold15);
                    company.Add(CreateWhiteSpace(20));
                    company.Add(new Chunk("Thời gian in phiếu: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontNormal10));
                    company.Alignment = Element.ALIGN_LEFT;
                    document.Add(company);
                    document.Add(Chunk.NEWLINE);

                    // Tiêu đề
                    Paragraph header = new Paragraph("THÔNG TIN PHIẾU NHẬP", fontBold25);
                    header.Alignment = Element.ALIGN_CENTER;
                    document.Add(header);
                    document.Add(Chunk.NEWLINE);

                    // Thông tin chung
                    PhieuNhapDTO pn = PhieuNhapDAO.Instance.selectById(maphieu.ToString());
                    NhaCungCapDTO ncc = NhaCungCapDAO.Instance.selectById(pn.MNCC.ToString());
                    NhanVienDTO nv = NhanVienDAO.Instance.selectById(pn.MNV.ToString());

                    Paragraph p1 = new Paragraph("Mã phiếu: PN-" + pn.MPN, fontNormal10);
                    
                    Paragraph p2 = new Paragraph($"Nhà cung cấp: {ncc.TEN}", fontNormal10);
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk("-"));
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk(ncc.DIACHI, fontNormal10));

                    Paragraph p3 = new Paragraph($"Người thực hiện: {nv.HOTEN}", fontNormal10);
                    p3.Add(CreateWhiteSpace(5));
                    p3.Add(new Chunk("-"));
                    p3.Add(CreateWhiteSpace(5));
                    p3.Add(new Chunk($"Mã nhân viên: {pn.MNV}", fontNormal10));

                    Paragraph p4 = new Paragraph("Thời gian nhập: " + pn.TG.ToString("dd/MM/yyyy HH:mm"), fontNormal10);

                    document.Add(p1);
                    document.Add(p2);
                    document.Add(p3);
                    document.Add(p4);
                    document.Add(Chunk.NEWLINE);

                    // Bảng Chi tiết
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 30f, 35f, 20f, 20f });

                    table.AddCell(new PdfPCell(new Phrase("Tên sản phẩm", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Giá", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Số lượng", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Tổng tiền", fontBold15)));

                    // Dữ liệu bảng
                    var listChiTiet = ChiTietPhieuNhapDAO.Instance.selectAll(maphieu.ToString());
                    foreach (var ctp in listChiTiet)
                    {
                        SanPhamDTO sp = SanPhamDAO.Instance.selectById(ctp.MSP.ToString());
                        
                        table.AddCell(new PdfPCell(new Phrase(sp.TEN, fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(Formater.FormatVND(ctp.TIENNHAP), fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(ctp.SL.ToString(), fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(Formater.FormatVND(ctp.SL * ctp.TIENNHAP), fontNormal10)));
                    }

                    document.Add(table);
                    document.Add(Chunk.NEWLINE);

                    // Tổng tiền
                    Paragraph paraTong = new Paragraph(new Phrase("Tổng thành tiền: " + Formater.FormatVND(pn.TIEN), fontBold15));
                    paraTong.IndentationLeft = 300;
                    document.Add(paraTong);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    // Chữ ký
                    PdfPTable signTable = new PdfPTable(3);
                    signTable.WidthPercentage = 100;
                    signTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    signTable.AddCell(new PdfPCell(new Phrase("Người lập phiếu", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("Nhân viên nhận", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("Nhà cung cấp", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    document.Add(signTable);
                    document.Close();
                }

                OpenFile(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file: " + ex.Message);
            }
        }

        // --- 2. XUẤT PHIẾU XUẤT (PX) ---
        public void WritePX(int maphieu)
        {
            string url = GetFile("PX" + maphieu);
            if (string.IsNullOrEmpty(url)) return;

            try
            {
                using (FileStream fs = new FileStream(url, FileMode.Create))
                {
                    Document document = new Document();
                    PdfWriter.GetInstance(document, fs);
                    document.Open();

                    Paragraph company = new Paragraph("Hệ thống quản lý kho hàng BestKhos", fontBold15);
                    company.Add(CreateWhiteSpace(20));
                    company.Add(new Chunk("Thời gian in phiếu: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontNormal10));
                    company.Alignment = Element.ALIGN_LEFT;
                    document.Add(company);
                    document.Add(Chunk.NEWLINE);

                    Paragraph header = new Paragraph("THÔNG TIN PHIẾU XUẤT", fontBold25);
                    header.Alignment = Element.ALIGN_CENTER;
                    document.Add(header);
                    document.Add(Chunk.NEWLINE);

                    PhieuXuatDTO px = PhieuXuatDAO.Instance.selectById(maphieu.ToString());
                    KhachHangDTO kh = KhachHangDAO.Instance.selectById(px.MKH.ToString());
                    NhanVienDTO nv = NhanVienDAO.Instance.selectById(px.MNV.ToString());

                    Paragraph p1 = new Paragraph("Mã phiếu: PX-" + px.MPX, fontNormal10);
                    
                    Paragraph p2 = new Paragraph($"Khách hàng: {kh.HOTEN}", fontNormal10);
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk("-"));
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk(kh.DIACHI, fontNormal10));

                    Paragraph p3 = new Paragraph($"Người thực hiện: {nv.HOTEN}", fontNormal10);
                    p3.Add(CreateWhiteSpace(5));
                    p3.Add(new Chunk("-"));
                    p3.Add(CreateWhiteSpace(5));
                    p3.Add(new Chunk($"Mã nhân viên: {px.MNV}", fontNormal10));

                    Paragraph p4 = new Paragraph("Thời gian xuất: " + px.TG.ToString("dd/MM/yyyy HH:mm"), fontNormal10);

                    document.Add(p1);
                    document.Add(p2);
                    document.Add(p3);
                    document.Add(p4);
                    document.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 30f, 35f, 20f, 20f });

                    table.AddCell(new PdfPCell(new Phrase("Tên sản phẩm", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Giá", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Số lượng", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Tổng tiền", fontBold15)));

                    var listChiTiet = ChiTietPhieuXuatDAO.Instance.selectAll(maphieu.ToString());
                    foreach (var ctp in listChiTiet)
                    {
                        SanPhamDTO sp = SanPhamDAO.Instance.selectById(ctp.MSP.ToString());
                        table.AddCell(new PdfPCell(new Phrase(sp.TEN, fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(Formater.FormatVND(ctp.TIENXUAT), fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(ctp.SL.ToString(), fontNormal10)));
                        table.AddCell(new PdfPCell(new Phrase(Formater.FormatVND(ctp.SL * ctp.TIENXUAT), fontNormal10)));
                    }

                    document.Add(table);
                    document.Add(Chunk.NEWLINE);

                    Paragraph paraTong = new Paragraph(new Phrase("Tổng thành tiền: " + Formater.FormatVND(px.TIEN), fontBold15));
                    paraTong.IndentationLeft = 300;
                    document.Add(paraTong);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    // Chữ ký
                    PdfPTable signTable = new PdfPTable(2);
                    signTable.WidthPercentage = 100;
                    signTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    signTable.AddCell(new PdfPCell(new Phrase("Người lập phiếu", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("Khách hàng", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    document.Add(signTable);
                    document.Close();
                }
                OpenFile(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file: " + ex.Message);
            }
        }

        // --- 3. XUẤT PHIẾU KIỂM KÊ (PKK) ---
        public void WritePKK(int maphieu)
        {
            string url = GetFile("PKK" + maphieu);
            if (string.IsNullOrEmpty(url)) return;

            try
            {
                using (FileStream fs = new FileStream(url, FileMode.Create))
                {
                    Document document = new Document();
                    PdfWriter.GetInstance(document, fs);
                    document.Open();

                    Paragraph company = new Paragraph("Hệ thống quản lý kho hàng BestKho", fontBold15);
                    company.Add(CreateWhiteSpace(20));
                    company.Add(new Chunk("Thời gian in phiếu: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontNormal10));
                    company.Alignment = Element.ALIGN_LEFT;
                    document.Add(company);
                    document.Add(Chunk.NEWLINE);

                    Paragraph header = new Paragraph("THÔNG TIN PHIẾU KIỂM KÊ", fontBold25);
                    header.Alignment = Element.ALIGN_CENTER;
                    document.Add(header);
                    document.Add(Chunk.NEWLINE);

                    PhieuKiemKeDTO pkk = PhieuKiemKeDAO.Instance.selectById(maphieu.ToString());
                    NhanVienDTO nv = NhanVienDAO.Instance.selectById(pkk.MNV.ToString());

                    Paragraph p1 = new Paragraph("Mã phiếu: PKK-" + pkk.MPKK, fontNormal10);
                    Paragraph p2 = new Paragraph($"Người thực hiện: {nv.HOTEN}", fontNormal10);
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk("-"));
                    p2.Add(CreateWhiteSpace(5));
                    p2.Add(new Chunk($"Mã nhân viên: {pkk.MNV}", fontNormal10));
                    Paragraph p3 = new Paragraph("Thời gian kiểm kê: " + pkk.TG.ToString("dd/MM/yyyy HH:mm"), fontNormal10);

                    document.Add(p1);
                    document.Add(p2);
                    document.Add(p3);
                    document.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(3);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 40f, 20f, 40f });

                    table.AddCell(new PdfPCell(new Phrase("Tên sản phẩm", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Trạng thái SP", fontBold15)));
                    table.AddCell(new PdfPCell(new Phrase("Ghi chú", fontBold15)));

                    var listChiTiet = ChiTietPhieuKiemKeDAO.Instance.selectAll(maphieu.ToString());
                    foreach (var ctp in listChiTiet)
                    {
                        SanPhamDTO sp = SanPhamDAO.Instance.selectById(ctp.MSP.ToString());
                        
                        table.AddCell(new PdfPCell(new Phrase(sp.TEN, fontNormal10)));
                        // Code cũ: ctp.getMSP() cho trạng thái -> Có thể bạn muốn hiện ID hoặc tên trạng thái?
                        // Ở đây mình giữ nguyên logic hiển thị Mã SP vào cột Trạng thái như code gốc Java
                        // Nhưng thường thì TRANGTHAISP mới đúng logic (ví dụ: 1-Tốt, 0-Hỏng)
                        table.AddCell(new PdfPCell(new Phrase(ctp.TRANGTHAISP.ToString(), fontNormal10))); 
                        
                        // GHICHU trong DB của bạn là String
                        table.AddCell(new PdfPCell(new Phrase(ctp.GHICHU?.ToString() ?? "", fontNormal10)));
                    }

                    document.Add(table);
                    document.Add(Chunk.NEWLINE);

                    PdfPTable signTable = new PdfPTable(2);
                    signTable.WidthPercentage = 100;
                    signTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    signTable.AddCell(new PdfPCell(new Phrase("Người lập phiếu", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("Nhân viên nhận", fontBoldItalic15)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    signTable.AddCell(new PdfPCell(new Phrase("(Ký và ghi rõ họ tên)", fontNormal10)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    document.Add(signTable);
                    document.Close();
                }
                OpenFile(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file: " + ex.Message);
            }
        }
    }
}