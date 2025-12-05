using System;

namespace src.DTO
{
    public class SanPhamDTO
    {
        public int MSP { get; set; }
        public string TEN { get; set; }
        public string HINHANH { get; set; }
        public string DANHMUC { get; set; }
        public int MSX { get; set; }
        public int MKVK { get; set; }
        public int MLSP { get; set; }
        public int TIENX { get; set; }
        public int TIENN { get; set; }
        public int SL { get; set; }
        public int TT { get; set; }

        public SanPhamDTO() { }

        public SanPhamDTO(int msp, string ten, string hinhanh, string danhmuc, int msx, int mkvk, int mlsp, int tienx, int tienn, int sl, int tt)
        {
            MSP = msp;
            TEN = ten;
            HINHANH = hinhanh;
            DANHMUC = danhmuc;
            MSX = msx;
            MKVK = mkvk;
            MLSP = mlsp;
            TIENX = tienx;
            TIENN = tienn;
            SL = sl;
            TT = tt;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 23 * hash + MSP.GetHashCode();
                hash = 23 * hash + (TEN != null ? TEN.GetHashCode() : 0);
                hash = 23 * hash + MLSP.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            SanPhamDTO other = (SanPhamDTO)obj;
            return MSP == other.MSP;
        }
    }
}