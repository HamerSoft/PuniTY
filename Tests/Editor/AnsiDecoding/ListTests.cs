using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class ListTests
    {
        [Test]
        public void Cannot_Set_IndexBased_Given_Capacity()
        {
            var list = new List<int>(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => { list[2] = 2; });
        }
    }
}