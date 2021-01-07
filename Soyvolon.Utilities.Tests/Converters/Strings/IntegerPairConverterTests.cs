using NUnit.Framework;

using Soyvolon.Utilities.Converters.Strings;

namespace Soyvolon.Utilities.Tests.Converters.Strings
{
    public class IntegerPairConverterTests
    {
        [TestCase("100", 100, 0)]
        [TestCase("100-200", 100, 200)]
        [TestCase("blank-200", 0, 200)]
        [TestCase("100 - 200", 100, 200)]
        [TestCase("100- 200", 100, 200)]
        [TestCase("100 -200", 100, 200)]
        [TestCase("100-x", 100, 0)]
        public void TestValidInputs(string input, int first, int second)
        {
            Assert.True(IntegerPairConverter.TryParse(input, out var res),
                "Converter returned false when true was expected.");
            Assert.That(res.Item1 == first,
                $"Expecte {first}, recevied {res.Item1} for first value.");
            Assert.That(res.Item2 == second,
                $"Expecte {second}, recevied {res.Item2} for first value.");
        }

        [TestCase("num-num")]
        [TestCase("x-")]
        [TestCase("")]
        public void TestInvalidInputs(string input)
        {
            Assert.False(IntegerPairConverter.TryParse(input, out _),
                "Converter returned true when false was expected.");
        }
    }
}
