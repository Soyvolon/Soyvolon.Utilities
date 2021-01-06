using System;

using NUnit.Framework;

using Soyvolon.Utilities.Converters.Strings;

namespace Soyvolon.Utilities.Tests.Converters.Strings
{
    public class DateTimePairConverterTests
    {
        [TestCase("5 days", 5)]
        [TestCase("5 weeks", 5 * 7)]
        [TestCase("5 months", 5 * 30)] // months uses a static 30
        [TestCase("5 years", 5 * 365)] // years use a static 365
        [TestCase("5 things", 5)]
        public void ConvertSingleValueTest(string arg, int finalDaysBehind)
        {
            Assert.True(TimeSpanPairConverter.TryParse(arg, out var pair),
                "Parse returned false");
            Assert.True(pair.Item1.Days == finalDaysBehind,
                $"Parse Check Failed: {pair.Item1.Days} != {finalDaysBehind}", arg, finalDaysBehind);
            Assert.True(pair.Item2 == TimeSpan.Zero,
                "Second value was not null");
        }

        [TestCase("10-5 days", 10, 5)]
        [TestCase("10-5 weeks", 10 * 7, 5 * 7)]
        [TestCase("10 -5 months", 10 * 30, 5 * 30)] // months uses a static 30
        [TestCase("10- 5 years", 10 * 365, 5 * 365)] // years use a static 365
        [TestCase("10 - 5 things", 10, 5)]
        public void ConvertValuePairTest(string arg, int startBehind, int endBehind)
        {
            Assert.True(TimeSpanPairConverter.TryParse(arg, out var pair),
                "Parse returned false");
            Assert.True(pair.Item1.Days == startBehind,
                $"Start value did not match: {pair.Item1.Days} != {startBehind}", arg, startBehind);
            Assert.True(pair.Item2.Days == endBehind,
                $"End value did not match: {pair.Item2.Days} != {endBehind}", arg, endBehind);
        }

        [TestCase("10-x days", 10, 0)]
        [TestCase("10 -x days", 10, 0)]
        [TestCase("10 - x days", 10, 0)]
        public void ConvertBadSecondValueTest(string arg, int startBehind, int endBehind)
        {
            Assert.True(TimeSpanPairConverter.TryParse(arg, out var pair),
                "Parse returned false");
            Assert.True(pair.Item1.Days == startBehind,
                $"Start value did not match: {pair.Item1.Days} != {startBehind}", arg, startBehind);
            Assert.True(pair.Item2.Days == endBehind,
                $"End value did not match: {pair.Item2.Days} != {endBehind}", arg, endBehind);
        }

        [TestCase("blah-blah days")]
        [TestCase("blah - blah days")]
        [TestCase("blah- blah days")]
        [TestCase("blah -blah days")]
        [TestCase("blah- 10 days")]
        [TestCase("5 -10 days")]
        [TestCase("10 - 10 days")]
        [TestCase("")]
        public void ConvertBadDataTest(string arg)
        {
            Assert.False(TimeSpanPairConverter.TryParse(arg, out _),
                "Parse returned true when it should be false");
        }
    }
}
