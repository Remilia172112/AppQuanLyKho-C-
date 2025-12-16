using System;

namespace src.DTO
{
    public class LoaiSanPhamDTO
    {
        public int MLSP { get; set; }
        public string TEN { get; set; }
        public int TLGX { get; set; } // <--- MỚI
        public string GHICHU { get; set; }
        public int TT { get; set; }

        public LoaiSanPhamDTO() { }

        public LoaiSanPhamDTO(int mlsp, string ten, int tlgx, string ghichu, int tt)
        {
            MLSP = mlsp;
            TEN = ten;
            TLGX = tlgx; // <--- MỚI
            GHICHU = ghichu;
            TT = tt;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            LoaiSanPhamDTO other = (LoaiSanPhamDTO)obj;
            return MLSP == other.MLSP;
        }

        public override int GetHashCode()
        {
            return MLSP.GetHashCode();
        }
    }
}