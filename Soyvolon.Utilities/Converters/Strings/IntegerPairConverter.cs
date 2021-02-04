using System;
using System.Diagnostics.CodeAnalysis;

namespace Soyvolon.Utilities.Converters.Strings
{
    public static class IntegerPairConverter
    {
        public static bool TryParse(string s, out Tuple<int, int> integerPair)
        {
            // make sure the string has a value.
            if(string.IsNullOrWhiteSpace(s))
            {
                integerPair = null;
                return false;
            }

            // split the two numbers at the middle ...
            var parts = s.Trim().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            // ... see if there is only a single part ...
            if (parts.Length <= 1)
            { // ... if there is, try and convert it to a number ...
                if (int.TryParse(parts[0], out int res))
                { // ... if it exsists, set the min value to the result and the max to 0 (or no limit).
                    integerPair = new Tuple<int, int>(res, 0);
                    return true;
                }// else return a null value
                else
                {
                    integerPair = null;
                    return false;
                }
            } 

            // ... check to see if the first part is a number ...
            if (int.TryParse(parts[0], out int resLower))
            { // ... and the second part ...
                if (int.TryParse(parts[1], out int resHigher))
                { // ... then set them to lower-higher limits ...
                    integerPair = new Tuple<int, int>(resLower, resHigher);
                    return true;
                }
                else
                { // the second value isnt there, so set it as 0.
                    integerPair = new Tuple<int, int>(resLower, 0);
                    return true;
                }
            }
            else
            { // check to see if the second value is a number ...
                if(int.TryParse(parts[1], out int resHigher))
                { // ... and assign it to a (no min)-higher limit ...
                    integerPair = new Tuple<int, int>(0, resHigher);
                    return true;
                }
            }

            // ... if nothing works, return null.
            integerPair = null;
            return false;
        }
    }
}
