using System;

using NUnit.Framework;

using Soyvolon.Utilities.Data;

namespace Soyvolon.Utilities.Tests.Data
{
    public class GooglePolylineConverterTests
    {
        [TestCase(41.82445, -87.67786, "yyg~FrqcvO")]
        [TestCase(-54.26256, -36.50701, "~cejIxgy}E")]
        [TestCase(38.5, -120.2, "_p~iF~ps|U")]
        [TestCase(65.11655, 134.16528, "mamlK_d{qX")]
        [TestCase(-75.03671, -69.74225, "lr~hM`pthL")]
        [TestCase(-54.29393, -36.32949, "`hkjIhrv|E")]
        public void TestEncode(double lat, double lng, string res)
        {
            var data = GooglePolylineEncoder.Encode(lat, lng);

            Assert.True(data.Equals(res), $"Data: {data} does not equal the expected value: {res}");
        }
    }
}
