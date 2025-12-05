using System;

namespace src.DTO
{
    public class PhieuNhapDTO
    {
        public int MPN { get; set; }
        public int MNCC { get; set; }
        public int MNV { get; set; }
        public int TIEN { get; set; }
        public DateTime TG { get; set; }
        public int TT { get; set; }

        public PhieuNhapDTO() { }

        public PhieuNhapDTO(int mpn, int mncc, int mnv, int tien, DateTime tg, int tt)
        {
            MPN = mpn;
            MNCC = mncc;
            MNV = mnv;
            TIEN = tien;
            TG = tg;
            TT = tt;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            PhieuNhapDTO other = (PhieuNhapDTO)obj;
            return MPN == other.MPN;
        }

        public override int GetHashCode()
        {
            return MPN.GetHashCode();
        }
    }
}