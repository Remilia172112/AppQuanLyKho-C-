using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class KhuVucKhoBUS
    {
        private readonly KhuVucKhoDAO kvkDAO = KhuVucKhoDAO.Instance;
        public List<KhuVucKhoDTO> listKVK = new List<KhuVucKhoDTO>();

        public KhuVucKhoBUS()
        {
            listKVK = kvkDAO.selectAll();
        }

        // Phương thức getInstance (nếu cần dùng kiểu Singleton, nhưng class này có public constructor)
        public static KhuVucKhoBUS GetInstance()
        {
            return new KhuVucKhoBUS();
        }

        public List<KhuVucKhoDTO> GetAll()
        {
            return this.listKVK;
        }

        public KhuVucKhoDTO GetByIndex(int index)
        {
            return this.listKVK[index];
        }

        // Hàm này bị trùng lặp logic với GetIndexByMaKVK, mình giữ lại và đổi tên cho rõ nghĩa
        // Trong Java: getIndexByMaLH (chắc copy paste chưa sửa tên)
        public int GetIndexByMaKhuVuc(int makhuvuc)
        {
            int i = 0;
            int vitri = -1;
            while (i < this.listKVK.Count && vitri == -1)
            {
                if (listKVK[i].MKVK == makhuvuc) // DTO C# dùng MKVK
                {
                    vitri = i;
                }
                else
                {
                    i++;
                }
            }
            return vitri;
        }

        public bool Add(KhuVucKhoDTO kvk)
        {
            bool check = kvkDAO.insert(kvk) != 0;
            if (check)
            {
                this.listKVK.Add(kvk);
            }
            return check;
        }

        public bool Delete(KhuVucKhoDTO kvk, int index)
        {
            bool check = kvkDAO.delete(kvk.MKVK.ToString()) != 0;
            if (check)
            {
                this.listKVK.RemoveAt(index);
            }
            return check;
        }

        public bool Update(KhuVucKhoDTO kvk)
        {
            bool check = kvkDAO.update(kvk) != 0;
            if (check)
            {
                int index = GetIndexByMaKhuVuc(kvk.MKVK);
                if (index != -1)
                {
                    this.listKVK[index] = kvk;
                }
            }
            return check;
        }

        public List<KhuVucKhoDTO> Search(string txt, string type)
        {
            List<KhuVucKhoDTO> result = new List<KhuVucKhoDTO>();
            txt = txt.ToLower();
            
            switch (type)
            {
                case "Tất cả":
                    foreach (KhuVucKhoDTO i in listKVK)
                    {
                        if (i.MKVK.ToString().Contains(txt) || i.TEN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Mã khu vực kho": // Sửa tên cho khớp ngữ cảnh
                    foreach (KhuVucKhoDTO i in listKVK)
                    {
                        if (i.MKVK.ToString().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
                case "Tên khu vực kho":
                    foreach (KhuVucKhoDTO i in listKVK)
                    {
                        if (i.TEN.ToLower().Contains(txt))
                        {
                            result.Add(i);
                        }
                    }
                    break;
            }
            return result;
        }

        public string[] GetArrTenKhuVuc()
        {
            int size = listKVK.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listKVK[i].TEN;
            }
            return result;
        }

        public string GetTenKhuVuc(int makhuvuc)
        {
            int index = GetIndexByMaKhuVuc(makhuvuc);
            if (index != -1)
            {
                return this.listKVK[index].TEN;
            }
            return "";
        }
    }
}