using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class ListTests
    {
        [Test]
        public void Check_If_Can_Insert_Based_On_Capacity()
        {
            var list = new List<int>(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => { list[2] = 2; });
        }
    }
}