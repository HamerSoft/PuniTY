using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class CursorTests : AnsiDecoderTest
    {
        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(10, 10, typeof(SaveCursor),
                typeof(SaveCursorDec),
                typeof(RestoreCursor),
                typeof(RestoreCursorDec));
        }

        [Test]
        public void When_SaveCursor_Position_Is_Saved_It_Can_Be_Restored()
        {
            Screen.SetCursorPosition(new Position(5, 5));
            SaveCursor(false);
            Screen.SetCursorPosition(new Position(8, 8));
            RestoreCursor(false);
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(5, 5)));
        }

        [Test]
        public void When_SaveCursorDecimals_Position_Is_Saved_It_Can_Be_Restored()
        {
            Screen.SetCursorPosition(new Position(5, 5));
            SaveCursor(true);
            Screen.SetCursorPosition(new Position(8, 8));
            RestoreCursor(true);
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(5, 5)));
        }

        private void SaveCursor(bool isDecimal)
        {
            Decode(isDecimal ? $"\x001b7" : $"{Escape}s");
        }

        private void RestoreCursor(bool isDecimal)
        {
            Decode(isDecimal ? $"\x001b8" : $"{Escape}u");
        }
    }
}