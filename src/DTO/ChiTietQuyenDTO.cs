using System;

namespace src.DTO
{
    public class ChiTietQuyenDTO
    {
        public int Manhomquyen { get; set; }
        public string Machucnang { get; set; }
        public string Hanhdong { get; set; }

        public ChiTietQuyenDTO() { }

        public ChiTietQuyenDTO(int manhomquyen, string machucnang, string hanhdong)
        {
            Manhomquyen = manhomquyen;
            Machucnang = machucnang;
            Hanhdong = hanhdong;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 83 * hash + Manhomquyen.GetHashCode();
                // Kiểm tra null cho string
                hash = 83 * hash + (Machucnang != null ? Machucnang.GetHashCode() : 0);
                hash = 83 * hash + (Hanhdong != null ? Hanhdong.GetHashCode() : 0);
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }

            ChiTietQuyenDTO other = (ChiTietQuyenDTO)obj;

            if (Manhomquyen != other.Manhomquyen)
            {
                return false;
            }

            if (Machucnang != other.Machucnang)
            {
                return false;
            }
            
            return Hanhdong == other.Hanhdong;
        }

        public override string ToString()
        {
            return $"ChiTietQuyen{{ manhomquyen={Manhomquyen}, machucnang={Machucnang}, hanhdong={Hanhdong} }}";
        }
    }
}