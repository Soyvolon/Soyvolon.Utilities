using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soyvolon.Utilities.Data
{
    /// <summary>
    /// Encode and Decode methods for Googles Polyline Algorithm Format based off of: https://developers.google.com/maps/documentation/utilities/polylinealgorithm
    /// </summary>
    public static class GooglePolylineEncoder
    {
        /// <summary>
        /// Encodes a latitude longitude pair into the Polyline string
        /// </summary>
        /// <param name="lat">Latitude in degrees</param>
        /// <param name="lng">Longitude in degrees</param>
        /// <returns>Polyline string</returns>
        public static string Encode(double lat, double lng)
            => Encode(lat) + Encode(lng);

        /// <summary>
        /// Encodes a single longitude/latitude value into its Polyline string.
        /// </summary>
        /// <param name="var">Global position part in degrees</param>
        /// <returns>Polyline string for half of a pair</returns>
        public static string Encode(double var)
        {
            // Take the decimal value and multiply it by 1E5, rouding the result ....
            var high = (int)Math.Round(var * 1E5);
            // ... build a place to hold our results ...
            var str = new StringBuilder();
            // ... convert the variable to binary ...
            uint b = (uint)high;
            // ... left shift the value by one bit ...
            uint shifted = b << 1;
            // ... if the initial value is negative ...
            if (var < 0) // ... invert the encoding ...
                shifted = ~shifted;
            // ... store the remaining bits for later ...
            uint rem = shifted;
            // ... while the remaining bits is greater than 0x20 ...
            while (rem >= 0x20)
            {
                // ... get the 5 bit batch to run the next operation on ...
                uint batch = (rem & 0x1f);
                // ... OR the batch by 0x20 ...
                uint or = 0x20 | batch;
                // ... get the char integer by adding 63 ...
                var cint = (int)or + 63;
                // ... conert it to a char ...
                var c = (char)cint;
                // ... and append it to the string ...
                str.Append(c);
                // ... then shift the remainder right by 5 for the next batch ...
                rem >>= 5;
            }
            // ... add the last value without the OR operation ...
            str.Append((char)(rem + 63));
            // ... then return the completed string.
            return str.ToString();
        }

        /// <summary>
        /// Encodes a list of coords
        /// </summary>
        /// <param name="coords">Coordnate pairs to encode.</param>
        /// <returns>Polyline string</returns>
        public static string Encode(IList<Tuple<double, double>> coords)
        {
            string output = "";
            Tuple<double, double> lastPair = new Tuple<double, double>(0, 0);
            foreach (var c in coords)
            {
                output += Encode(c.Item1 - lastPair.Item1, c.Item2 - lastPair.Item2);
                lastPair = c;
            }

            return output;
        }
    }
}
