using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs_Lib.utils {
    public static class LinqExtensions {
        public static T Min2<T, E>(this IEnumerable<T> enumeration, Func<T, E> extractor) where E : IComparable {
            T minItem = enumeration.FirstOrDefault();
            E minValue = extractor(minItem);

            foreach (T item in enumeration.Skip(1)) {
                E value = extractor(item);
                if (value.CompareTo(minValue) < 0) {
                    minItem = item;
                    minValue = value;
                }
            }

            return minItem;
        }
    }
}
