using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class CursorTests : AnsiDecoderTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(10, 10);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new SaveCursor(),
                new SaveCursorDec(),
                new RestoreCursor(),
                new RestoreCursorDec());
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