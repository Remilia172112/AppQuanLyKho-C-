using System;

namespace src.DTO
{
    public class NhaCungCapDTO
    {
        public int MNCC { get; set; }
        public string TEN { get; set; }
        public string DIACHI { get; set; }
        public string EMAIL { get; set; }
        public string SDT { get; set; }
        public int TT { get; set; }

        public NhaCungCapDTO() { }

        public NhaCungCapDTO(int mncc, string ten, string diachi, string email, string sdt, int tt)
        {
            MNCC = mncc;
            TEN = ten;
            DIACHI = diachi;
            EMAIL = email;
            SDT = sdt;
            TT = tt;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 23 * hash + MNCC.GetHashCode();
                hash = 23 * hash + (TEN != null ? TEN.GetHashCode() : 0);
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            NhaCungCapDTO other = (NhaCungCapDTO)obj;
            return MNCC == other.MNCC && TEN == other.TEN;
        }
    }
}