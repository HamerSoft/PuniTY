using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class MoveAndScrollTests : AnsiDecoderTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(10, 10);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                CreateSequence(typeof(MoveAndScrollUp)));
        }

        [Test]
        public void When_Cursor_MoveAndScroll_CursorMoves_1_Line_Up()
        {
            Screen.SetCursorPosition(new Position(2, 2));
            Decode($"\x001bM");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 2)));
        }
    }
}