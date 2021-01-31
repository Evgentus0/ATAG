using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Extentions
{
    public static class CollectionExtention
    {
        public static bool EqualsByElements<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if(first.Count() != second.Count())
                return false;

            for (int i = 0; i < first.Count(); i++)
            {
                if (!first.ElementAt(i).Equals(second.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HasSameElements<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first.Count() != second.Count())
                return false;

            foreach(var element in first)
            {
                if (!second.Contains(element))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
