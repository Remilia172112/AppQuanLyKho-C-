using System;

namespace src.DTO
{
    public class KhuVucKhoDTO
    {
        public int MKVK { get; set; }
        public string TEN { get; set; }
        public string GHICHU { get; set; }
        public int TT { get; set; }

        public KhuVucKhoDTO()
        {
        }

        public KhuVucKhoDTO(int mkvk, string ten, string ghichu, int tt)
        {
            MKVK = mkvk;
            TEN = ten;
            GHICHU = ghichu;
            TT = tt;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 23 * hash + MKVK.GetHashCode();
                hash = 23 * hash + (TEN != null ? TEN.GetHashCode() : 0);
                hash = 23 * hash + (GHICHU != null ? GHICHU.GetHashCode() : 0);
                hash = 23 * hash + TT.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            KhuVucKhoDTO other = (KhuVucKhoDTO)obj;

            if (MKVK != other.MKVK) return false;
            if (TT != other.TT) return false;
            if (TEN != other.TEN) return false;
            return GHICHU == other.GHICHU;
        }

        public override string ToString()
        {
            return $"KhuVucKhoDTO{{ MKVK={MKVK}, TEN={TEN}, GHICHU={GHICHU}, TT={TT} }}";
        }
    }
}