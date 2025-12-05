using System;

namespace src.DTO
{
    public class PhieuKiemKeDTO
    {
        public int MPKK { get; set; }
        public int MNV { get; set; }
        public DateTime TG { get; set; }
        public int TT { get; set; }

        public PhieuKiemKeDTO() { }

        public PhieuKiemKeDTO(int mpkk, int mnv, DateTime tg, int tt)
        {
            MPKK = mpkk;
            MNV = mnv;
            TG = tg;
            TT = tt;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 23 * hash + MPKK.GetHashCode();
                hash = 23 * hash + MNV.GetHashCode();
                hash = 23 * hash + TG.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            PhieuKiemKeDTO other = (PhieuKiemKeDTO)obj;
            return MPKK == other.MPKK && MNV == other.MNV && TG == other.TG;
        }
    }
}