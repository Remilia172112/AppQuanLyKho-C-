using System;

namespace src.DTO.ThongKe
{
    public class ThongKeNhaCungCapDTO
    {
        public int Mancc { get; set; }
        public string Tenncc { get; set; }
        public int Soluong { get; set; }
        public long Tongtien { get; set; }

        public ThongKeNhaCungCapDTO()
        {
        }

        public ThongKeNhaCungCapDTO(int mancc, string tenncc, int soluong, long tongtien)
        {
            Mancc = mancc;
            Tenncc = tenncc;
            Soluong = soluong;
            Tongtien = tongtien;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 59 * hash + Mancc.GetHashCode();
                hash = 59 * hash + (Tenncc != null ? Tenncc.GetHashCode() : 0);
                hash = 59 * hash + Soluong.GetHashCode();
                hash = 59 * hash + Tongtien.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ThongKeNhaCungCapDTO other = (ThongKeNhaCungCapDTO)obj;

            if (Mancc != other.Mancc) return false;
            if (Soluong != other.Soluong) return false;
            if (Tongtien != other.Tongtien) return false;
            return Tenncc == other.Tenncc;
        }

        public override string ToString()
        {
            return $"ThongKeNhaCungCapDTO{{ mancc={Mancc}, tenncc={Tenncc}, soluong={Soluong}, tongtien={Tongtien} }}";
        }
    }
}