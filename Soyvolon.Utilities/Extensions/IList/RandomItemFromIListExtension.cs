using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soyvolon.Utilities.Extensions.IList
{
    public static class RandomItemFromIListExtension
    {
        public static T GetRandom<T>(this IList<T> collection)
        {
            var i = Utilities.Random.Next(0, collection.Count);
            T res = collection[i];
            return res;
        }
    }
}
