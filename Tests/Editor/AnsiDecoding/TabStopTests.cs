using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class TabStopTests : AnsiDecoderTest
    {
        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(2, 80);
        }

        [Test]
        public void Screen_ClearTabStop_Makes_CurrentTabStop_Skip()
        {
            Screen.ClearTabStop(1);
            Assert.That(Screen.GetCurrentTabStop(1), Is.EqualTo(0));
        }

        [TestCase(2, 2)]
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        public void Screen_GetCurrentTabStop_Skips_Cleared_TabStops(int tabStopsToClear, int expectedTabStop)
        {
            for (int i = 0; i < tabStopsToClear; i++)
                Screen.ClearTabStop(i * 8);
            Assert.That(Screen.GetCurrentTabStop(1), Is.EqualTo(expectedTabStop));
        }

        [TestCase(2, 3)]
        [TestCase(5, 6)]
        [TestCase(10, 10)]
        public void Screen_GetNextTabStop_Skips_Cleared_TabStops(int tabStopsToClear, int expectedTabStop)
        {
            for (int i = 0; i <= tabStopsToClear; i++)
                Screen.ClearTabStop(i * 8);
            Assert.That(Screen.GetNextTabStop(1), Is.EqualTo(expectedTabStop));
        }

        [TestCase(2, 7)]
        [TestCase(5, 4)]
        [TestCase(10, 0)]
        public void Screen_GetPreviousTabStop_Skips_Cleared_TabStops(int tabStopsToClear, int expectedTabStop)
        {
            const int columns = 80;
            for (int i = 0; i <= tabStopsToClear; i++)
                Screen.ClearTabStop(columns - i * 8);
            Assert.That(Screen.GetPreviousTabStop(columns), Is.EqualTo(expectedTabStop));
        }
    }
}