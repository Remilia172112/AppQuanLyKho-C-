using System;

namespace src.DTO.ThongKe
{
    public class ThongKeDoanhThuDTO
    {
        // Sử dụng long thay cho Long (Java wrapper)
        public int Thoigian { get; set; } // nam, thang, ngay
        public long Von { get; set; }
        public long Doanhthu { get; set; }
        public long Loinhuan { get; set; }

        public ThongKeDoanhThuDTO()
        {
        }

        // Đã bỏ 'this.' bằng cách đặt tên tham số viết thường (camelCase)
        public ThongKeDoanhThuDTO(int thoigian, long von, long doanhthu, long loinhuan)
        {
            Thoigian = thoigian;
            Von = von;
            Doanhthu = doanhthu;
            Loinhuan = loinhuan;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 97 * hash + Thoigian.GetHashCode();
                hash = 97 * hash + Von.GetHashCode();
                hash = 97 * hash + Doanhthu.GetHashCode();
                hash = 97 * hash + Loinhuan.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            ThongKeDoanhThuDTO other = (ThongKeDoanhThuDTO)obj;
            
            if (Thoigian != other.Thoigian) return false;
            if (Von != other.Von) return false;
            if (Doanhthu != other.Doanhthu) return false;
            return Loinhuan == other.Loinhuan;
        }

        public override string ToString()
        {
            return $"ThongKeDoanhThu{{ thoigian={Thoigian}, von={Von}, doanhthu={Doanhthu}, loinhuan={Loinhuan} }}";
        }
    }
}