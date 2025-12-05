using System.Collections.Generic;

namespace src.DAO
{
    public interface ChiTietInterface<T>
    {
        int insert(List<T> t);
        int delete(string t);
        int update(List<T> t, string pk);
        List<T> selectAll(string t);
    }
}