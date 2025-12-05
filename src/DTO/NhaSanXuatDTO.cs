using System;

namespace src.DTO
{
    public class NhaSanXuatDTO
    {
        public int MSX { get; set; }
        public string TEN { get; set; }
        public string DIACHI { get; set; }
        public string EMAIL { get; set; }
        public string SDT { get; set; }
        public int TT { get; set; }

        public NhaSanXuatDTO() { }

        public NhaSanXuatDTO(int msx, string ten, string diachi, string email, string sdt, int tt)
        {
            MSX = msx;
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
            return MSX == other.MSX;
        }

        public override int GetHashCode()
        {
            return MSX.GetHashCode();
        }
    }
}