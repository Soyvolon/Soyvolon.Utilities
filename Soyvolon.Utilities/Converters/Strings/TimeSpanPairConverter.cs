using System;
using System.Linq;

namespace Soyvolon.Utilities.Converters.Strings
{
    public static class TimeSpanPairConverter
    {
        public static bool TryParse(string input, out Tuple<TimeSpan, TimeSpan>? timeSpanPair)
        {
            if (input is null || input == "") // No input, return defaults
            {
                timeSpanPair = null;
                return false;
            }

            int start = 0;
            int end = 0;
            // Follows rules for AO3 Date Search
            // Split the item by blank spaces
            var items = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            // Create a counter to keep track of position
            int c = 0;

            // Time to find the actuall timeframe, whats the start and end numbers.
            // See if the first item is a number.
            if (int.TryParse(items[c], out int res1))
            {
                // If it is, assign it to the start positon.
                start = res1;
                // increase C
                c++;
            }

            if (items.Length > c)
            {
                if (int.TryParse(items[c], out var endRes))
                {
                    end = endRes;
                }
                // See if the first item is a symbol.
                else if (items[c].StartsWith("<") || items[c].StartsWith(">"))
                {
                    // If it is, split the grouping into symbol and number.
                    var startStr = items[c][0..1];
                    var dataStr = items[c][1..];
                    // And find out if the number is actually a number.
                    if (int.TryParse(dataStr, out int res2))
                    { // If it is, test for the direction of the symbol
                        if (startStr.Equals("<"))
                        { // If its less than, start becomes the result and end is 0.
                            start = res2;
                            end = 0;
                        }
                        else
                        { // If its greater than, end becomes the result and start is 0.
                            start = 0;
                            end = res2;
                        }
                    }
                    else
                    { // If it is not a number, fail the check and reutrn the default value.
                        timeSpanPair = null;
                        return false;
                    }
                }
                // ... if the value is exactly a dash ...
                if (items[c].Equals("-"))
                { // ... and there is another item ...
                    if (items.Length > c + 1)
                    { // ... and that next item is a number ...
                        var endStr = items[c + 1];
                        if (int.TryParse(endStr, out var num))
                        { // ... save the number as the end point ...
                            end = num;
                        }
                    }
                }
                // ... see if it starts with a dash ...
                else if (items[c].StartsWith("-"))
                { // ... and take the raw value without the dash ...
                    var endStr = items[c][1..];
                    // ... then try and convrt it to a number ...
                    if (int.TryParse(endStr, out var num))
                    {
                        // then save the number as the end value.
                        end = num;
                    }
                }
                // Otherwise, see if it is a range and contains the range operator
                else if (items[c].Contains("-"))
                { // if it is, split the item by the range operator
                    var split = items[c].Split("-", StringSplitOptions.RemoveEmptyEntries);

                    string startStr;
                    string endStr;
                    if (split.Length != 2)
                    {
                        // the dash had no leading space, but a trailing one. Attempt to get another vaue from items.

                        startStr = split[0];
                        // make sure we have another item ...
                        if (items.Length > c + 1)
                        {
                            // increase C for accuracy.
                            endStr = items[++c];
                        }
                        else
                        { // looks like the second item was a lie,
                          // set the endStr to a blank string for defaults.
                            endStr = "";
                        }
                    }
                    else
                    { // otherwise, build from the split values
                        startStr = split[0];
                        endStr = split[1];
                    }
                    // Check both the start number and end number to ensure they are numbers.
                    if (int.TryParse(startStr, out int resStart))
                    {   // Assign the start to start ...
                        start = resStart;
                        if (int.TryParse(endStr, out int resEnd))
                        {  // ... and end to end
                            end = resEnd;
                        }
                    } // If either fails, ignore it, they both wind up being zero and default will be returned later.
                }
                else if (input.Contains("-"))
                {
                    // this is an invalid configuration.
                    timeSpanPair = null;
                    return false;
                }
            }

            // If nothing was changed from start, a value was not found. Return the default value.
            if (start == 0 && end == 0)
            {
                timeSpanPair = null;
                return false;
            }

            // the start must be larger than the end value when both are not zero.
            if(start != 0 && end != 0 && start <= end)
            {
                timeSpanPair = null;
                return false;
            }

            TimeSpan startSpan;
            TimeSpan endSpan;

           // get the last value for modifiers.
            var edited = items[^1].Trim().ToLower();

            if (edited.Contains("week"))
            {
                startSpan = TimeSpan.FromDays(start * 7);
                endSpan = TimeSpan.FromDays(end * 7);
            }
            else if (edited.Contains("month"))
            {
                startSpan = TimeSpan.FromDays(start * 30);
                endSpan = TimeSpan.FromDays(end * 30);
            }
            else if (edited.Contains("year"))
            {
                startSpan = TimeSpan.FromDays(start * 365);
                endSpan = TimeSpan.FromDays(end * 365);
            }
            else
            { // Nothing matches, assume days
                startSpan = TimeSpan.FromDays(start);
                endSpan = TimeSpan.FromDays(end);
            }

            timeSpanPair = new Tuple<TimeSpan, TimeSpan>(startSpan, endSpan);
            return true;
        }

        public static Tuple<DateTime, DateTime> ConvertToDateTimePair(this Tuple<TimeSpan, TimeSpan> span)
        {
            var start = DateTime.MinValue;
            var end = DateTime.MinValue;

            if (span.Item1 != TimeSpan.Zero)
            {
                start = DateTime.Now - span.Item1;
            }

            if (span.Item2 != TimeSpan.Zero)
            {
                end = DateTime.Now - span.Item2;
            }

            return new(start, end);
        }
    }
}
