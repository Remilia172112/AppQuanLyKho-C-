using System;
using System.Text.RegularExpressions; // Thư viện xử lý Regex

namespace src.Helper
{
    public class Validation
    {
        // 1. Kiểm tra chuỗi rỗng
        public static bool IsEmpty(string input)
        {
            // string.IsNullOrEmpty kiểm tra cả null và ""
            return string.IsNullOrEmpty(input);
        }

        // 2. Kiểm tra ngày tháng (Dùng DateTime? để cho phép null)
        public static bool IsEmpty(DateTime? input)
        {
            return input == null;
        }

        // 3. Kiểm tra Email
        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            
            // Regex email chuẩn
            string emailRegex = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
            
            return Regex.IsMatch(email, emailRegex);
        }

        // 4. Kiểm tra có phải số nguyên dương hay không
        public static bool IsNumber(string num)
        {
            if (string.IsNullOrEmpty(num)) return false;

            // Dùng TryParse hiệu quả hơn try-catch
            if (long.TryParse(num, out long k))
            {
                if (k < 0) return false;
                return true;
            }
            
            return false;
        }

        // 5. Kiểm tra số điện thoại
        public static bool IsPhoneNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            // Loại bỏ khoảng trắng và các ký tự đặc biệt
            str = str.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

            // Kiểm tra xem chuỗi có phải là 10 chữ số hay không
            // Lưu ý: Trong code Java gốc, sau khi replace hết các ký tự đặc biệt, 
            // thì các dòng check "-" hay "()" bên dưới sẽ không bao giờ khớp nữa.
            // Nên chỉ cần check \d{10} là đủ cho logic tương đương.
            
            if (Regex.IsMatch(str, @"^\d{10}$")) 
            { 
                return true;
            } 
            
            // Giữ lại các logic cũ nếu bạn muốn validate format gốc chưa replace (tùy nhu cầu),
            // nhưng theo logic code Java bạn gửi thì dòng này là unreachable code (code chết).
            /*
            else if (Regex.IsMatch(str, @"^\d{3}-\d{3}-\d{4}$")) 
            { 
                return true;
            } 
            else if (Regex.IsMatch(str, @"^\(\d{3}\)\d{3}-\d{4}$")) 
            { 
                return true;
            } 
            */
            
            return false; 
        }
    }
}