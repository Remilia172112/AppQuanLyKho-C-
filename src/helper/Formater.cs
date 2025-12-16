using System;
using System.Globalization;

namespace src.Helper
{
    public class Formater
    {
        // 1. Định dạng tiền VND (Ví dụ: 1,000,000đ hoặc 1.000.000đ tùy cài đặt máy)
        public static string FormatVND(double vnd)
        {
            // Cách 1: Dùng định dạng số cơ bản (giống hệt Java "###,###,###")
            // "#,###" sẽ tự động dùng dấu phân cách hàng nghìn theo cài đặt máy tính của bạn
            return vnd.ToString("#,###") + "đ";

            /* * Mở rộng: Nếu bạn muốn BẮT BUỘC dùng dấu chấm (.) phân cách hàng nghìn (Chuẩn Việt Nam) 
             * bất kể máy tính đang cài đặt tiếng Anh hay Việt, thì dùng code này:
             * * CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
             * return vnd.ToString("#,###", cul.NumberFormat) + "đ";
             */
        }

        // 2. Định dạng thời gian (Timestamp -> String)
        public static string FormatTime(DateTime thoigian)
        {
            // Trong C#, java.sql.Timestamp tương ứng với DateTime
            // Format "dd/MM/yyyy HH:mm" (Lưu ý: yyyy viết thường là chuẩn C#)
            return thoigian.ToString("dd/MM/yyyy HH:mm");
        }
        
        // Bonus: Hàm chuyển đổi ngược từ chuỗi tiền về số (nếu cần xử lý nhập liệu)
        // Ví dụ: "100,000" -> 100000
        public static double UnformatVND(string tien)
        {
            try 
            {
                string cleanStr = tien.Replace("đ", "").Replace(",", "").Replace(".", "").Trim();
                return double.Parse(cleanStr);
            }
            catch 
            {
                return 0;
            }
        }
        public static void FormatTextBoxMoney(TextBox txt)
        {
            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            try
            {
                // 1. Giữ vị trí con trỏ chuột tương đối so với cuối chuỗi
                // (Để khi thêm dấu phẩy, con trỏ không bị nhảy về đầu)
                int cursorFromEnd = txt.Text.Length - txt.SelectionStart;

                // 2. Xóa hết dấu phẩy/chấm cũ để lấy số thô
                string rawText = txt.Text.Replace(",", "").Replace(".", "").Trim();

                if (long.TryParse(rawText, out long value))
                {
                    // 3. Format lại có dấu phân cách (N0)
                    txt.Text = value.ToString("N0");

                    // 4. Tính lại vị trí con trỏ mới
                    int newCursorPos = txt.Text.Length - cursorFromEnd;
                    if (newCursorPos < 0) newCursorPos = 0;
                    
                    txt.SelectionStart = newCursorPos;
                }
            }
            catch { }
        }
    }
}