using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soyvolon.Utilities.Data.Extensions
{
    public static class GooglePolylineEncoderExtensions
    {
        public static string GetPolyline(this Tuple<double, double> pair)
            => GooglePolylineEncoder.Encode(pair.Item1, pair.Item2);

        public static string GetPolyline(this double val)
            => GooglePolylineEncoder.Encode(val);
    }
}
