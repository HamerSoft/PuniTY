using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.EraseSequences;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class ScrollingTests : AnsiDecoderTest
    {
        private const int ScreenRows = 10;
        private const int ScreenColumns = 5;
        private const char DefaultChar = 'a';

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(ScreenRows, ScreenColumns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new EraseDisplaySequence());
            PopulateScreen();
        }

        private void PopulateScreen()
        {
            for (int i = 0; i < ScreenRows; i++)
            for (int j = 0; j < ScreenColumns; j++)
                Screen.AddCharacter(DefaultChar);
            Screen.SetCursorPosition(new Position(1, 1));
        }

        [Test]
        public void When_Adding_Character_GreaterThan_ScreenMax_New_Row_Is_Added_And_OffSet_Is_Increased()
        {
            Screen.SetCursorPosition(new Position(ScreenRows, ScreenColumns));
            Screen.AddCharacter('g');
            Screen.AddCharacter('g');
            Screen.AddCharacter('g');
            Screen.AddCharacter('g');
            Screen.AddCharacter('g');
            PrintScreen();
            AssertScreen(10, 1, DefaultChar, 'g', true);
        }

        private void AssertScreen(int row, int column, char before, char after, bool toEnd)
        {
            if (row < 1 || row > ScreenRows || column < 1 || column > ScreenColumns)
            {
                row = 1;
                column = 1;
            }

            for (int r = 1; r <= ScreenRows; r++)
            for (int c = 1; c <= ScreenColumns; c++)
                if (r < row || (toEnd ? (r < row && c < column) : (r <= row && c <= column)))
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before));
                else
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(after));
        }
    }
}