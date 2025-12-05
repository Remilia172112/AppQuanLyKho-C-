using System;

namespace src.DTO
{
    public class NhaSanXuatDTO
    {
        public int MNSX { get; set; }
        public string TEN { get; set; }
        public string DIACHI { get; set; }
        public string EMAIL { get; set; }
        public string SDT { get; set; }
        public int TT { get; set; }

        public NhaSanXuatDTO() { }

        public NhaSanXuatDTO(int mnsx, string ten, string diachi, string email, string sdt, int tt)
        {
            MNSX = mnsx;
            TEN = ten;
            DIACHI = diachi;
            EMAIL = email;
            SDT = sdt;
            TT = tt;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            NhaSanXuatDTO other = (NhaSanXuatDTO)obj;
            return MNSX == other.MNSX;
        }

        public override int GetHashCode()
        {
            return MNSX.GetHashCode();
        }
    }
}