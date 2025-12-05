using System;

namespace src.Helper 
{
    public static class BCrypts
    {
        // 2. Hàm tạo Salt (Ánh xạ từ gensalt -> GenerateSalt)
        public static string gensalt(int log_rounds)
        {
            // Gọi thư viện gốc
            return global::BCrypt.Net.BCrypt.GenerateSalt(log_rounds);
        }

        // 3. Hàm băm mật khẩu (Ánh xạ từ hashpw -> HashPassword)
        public static string hashpw(string password, string salt)
        {
            // Gọi thư viện gốc
            return global::BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        // 4. Hàm kiểm tra (Ánh xạ từ checkpw -> Verify)
        public static bool checkpw(string plaintext, string hashed)
        {
            return global::BCrypt.Net.BCrypt.Verify(plaintext, hashed);
        }
    }
}