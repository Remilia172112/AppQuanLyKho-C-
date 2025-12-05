using System;

namespace src.DTO.ThongKe
{
    public class ThongKeTonKhoDTO
    {
        public int Masp { get; set; }
        public int Maphienbansp { get; set; }
        public string Tensanpham { get; set; }
        public int Ram { get; set; }
        public int Rom { get; set; }
        public string Mausac { get; set; }
        public int Tondauky { get; set; }
        public int Nhaptrongky { get; set; }
        public int Xuattrongky { get; set; }
        public int Toncuoiky { get; set; }

        public ThongKeTonKhoDTO()
        {
        }

        public ThongKeTonKhoDTO(int masp, int maphienbansp, string tensanpham, int ram, int rom, string mausac, int tondauky, int nhaptrongky, int xuattrongky, int toncuoiky)
        {
            Masp = masp;
            Maphienbansp = maphienbansp;
            Tensanpham = tensanpham;
            Ram = ram;
            Rom = rom;
            Mausac = mausac;
            Tondauky = tondauky;
            Nhaptrongky = nhaptrongky;
            Xuattrongky = xuattrongky;
            Toncuoiky = toncuoiky;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 29 * hash + Masp.GetHashCode();
                hash = 29 * hash + Maphienbansp.GetHashCode();
                hash = 29 * hash + (Tensanpham != null ? Tensanpham.GetHashCode() : 0);
                hash = 29 * hash + Ram.GetHashCode();
                hash = 29 * hash + Rom.GetHashCode();
                hash = 29 * hash + (Mausac != null ? Mausac.GetHashCode() : 0);
                hash = 29 * hash + Tondauky.GetHashCode();
                hash = 29 * hash + Nhaptrongky.GetHashCode();
                hash = 29 * hash + Xuattrongky.GetHashCode();
                hash = 29 * hash + Toncuoiky.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ThongKeTonKhoDTO other = (ThongKeTonKhoDTO)obj;

            if (Masp != other.Masp) return false;
            if (Maphienbansp != other.Maphienbansp) return false;
            if (Ram != other.Ram) return false;
            if (Rom != other.Rom) return false;
            if (Tondauky != other.Tondauky) return false;
            if (Nhaptrongky != other.Nhaptrongky) return false;
            if (Xuattrongky != other.Xuattrongky) return false;
            if (Toncuoiky != other.Toncuoiky) return false;
            
            // So sánh chuỗi
            if (Tensanpham != other.Tensanpham) return false;
            return Mausac == other.Mausac;
        }

        public override string ToString()
        {
            return $"ThongKeTonKhoDTO{{ masp={Masp}, maphienbansp={Maphienbansp}, tensanpham={Tensanpham}, ram={Ram}, rom={Rom}, mausac={Mausac}, tondauky={Tondauky}, nhaptrongky={Nhaptrongky}, xuattrongky={Xuattrongky}, toncuoiky={Toncuoiky} }}";
        }
    }
}