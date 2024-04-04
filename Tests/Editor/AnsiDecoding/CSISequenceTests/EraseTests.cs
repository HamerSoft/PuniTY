using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.EraseSequences;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class EraseTests : AnsiDecoderTest
    {
        private const int ScreenRows = 10;
        private const int ScreenColumns = 5;
        private const char DefaultChar = 'a';
        private const char EmptyCharacter = ' ';

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
                Screen.SetCharacter(new Character(DefaultChar), new Position(i + 1, j + 1));
        }

        [Test]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition_No_Arg()
        {
            var row = 2;
            var column = 3;
            Screen.SetCursorPosition(new Position(row, column));
            Decode($"{Escape}J");
            AssertScreen(row, column, DefaultChar, EmptyCharacter,true);
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(10, 4)]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(0);
            AssertScreen(row, column, DefaultChar, EmptyCharacter,true);
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(10, 4)]
        public void When_Erase_Display_Clears_Before_Of_Screen_Based_On_CursorPosition(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(1);
            PrintScreen();
            AssertScreen(row, column, EmptyCharacter, DefaultChar,false);
        }

        private void AssertScreen(int row, int column, char before, char after, bool toEnd)
        {
            // if (toEnd)
            // {
            //     for (int r = 0; r < ScreenRows; r++)
            //     for (int c = 0; c < ScreenColumns; c++)
            //         if (r < row || (r < row && c < column))
            //             Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo(before));
            //         else
            //             Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo(after));
            //
            // }

            for (int r = 1; r <= ScreenRows; r++)
            for (int c = 1; c <= ScreenColumns; c++)
                if (r < row || (toEnd ? (r < row && c < column) : (r <= row && c <= column)))
                    Assert.That(Screen.Character(new Position(r, c)).Char, Is.EqualTo(before));
                else
                    Assert.That(Screen.Character(new Position(r, c)).Char, Is.EqualTo(after));
        }

        private void EraseScreen(int jArg)
        {
            Decode($"{Escape}{jArg}J");
        }
    }
}