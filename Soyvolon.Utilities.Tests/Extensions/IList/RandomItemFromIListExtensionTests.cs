using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Soyvolon.Utilities.Extensions.IList;

namespace Soyvolon.Utilities.Tests.Extensions.IList
{
    public class RandomItemFromIListExtensionTests
    {
        [TestCase(1, 2, 3)]
        [TestCase("test", "two", "three")]
        [TestCase("test", 3, int.MinValue)]
        public void GetRandomItem(params object[] data)
        {
            var res = data.GetRandom();
            Assert.True(data.Contains(res));
        }
    }
}
