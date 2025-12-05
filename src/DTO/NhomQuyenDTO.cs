using System;

namespace src.DTO
{
    public class NhomQuyenDTO
    {
        public int Manhomquyen { get; set; }
        public string Tennhomquyen { get; set; }

        public NhomQuyenDTO()
        {
        }

        public NhomQuyenDTO(int manhomquyen, string tennhomquyen)
        {
            Manhomquyen = manhomquyen;
            Tennhomquyen = tennhomquyen;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = 37 * hash + Manhomquyen.GetHashCode();
                hash = 37 * hash + (Tennhomquyen != null ? Tennhomquyen.GetHashCode() : 0);
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

            NhomQuyenDTO other = (NhomQuyenDTO)obj;
            if (this.Manhomquyen != other.Manhomquyen)
            {
                return false;
            }
            return this.Tennhomquyen == other.Tennhomquyen;
        }

        public override string ToString()
        {
            return $"NhomQuyenDTO{{ manhomquyen={Manhomquyen}, tennhomquyen={Tennhomquyen} }}";
        }
    }
}