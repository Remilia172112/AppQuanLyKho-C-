using System;

namespace src.DTO
{
    public class KhachHangDTO
    {
        public int MKH { get; set; }
        public string HOTEN { get; set; }
        public DateTime NGAYTHAMGIA { get; set; }
        public string DIACHI { get; set; }
        public string SDT { get; set; }
        public string EMAIL { get; set; }
        public int TT { get; set; }

        public KhachHangDTO() { }

        public KhachHangDTO(int mkh, string hoten, DateTime ngaythamgia, string diachi, string sdt, string email, int tt)
        {
            MKH = mkh;
            HOTEN = hoten;
            NGAYTHAMGIA = ngaythamgia;
            DIACHI = diachi;
            SDT = sdt;
            EMAIL = email;
            TT = tt;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            KhachHangDTO other = (KhachHangDTO)obj;
            return MKH == other.MKH;
        }

        public override int GetHashCode()
        {
            return MKH.GetHashCode();
        }
    }
}