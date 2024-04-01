using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.EraseSequences;
using NUnit.Framework;
using UnityEngine;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class EraseTests : AnsiDecoderTest
    {
        private const int rows = 10;
        private const int columns = 5;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(rows, columns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new EraseDisplaySequence());
            PopulateScreen();
        }

        private void PopulateScreen()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    Screen.SetCharacter(new Character('a'), new Position(i + 1, j + 1));
            }
        }

        [Test]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition_No_Arg()
        {
            var row = 2;
            var column = 3;
            Screen.SetCursorPosition(new Position(row, column));
            Decode($"{Escape}J");
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (r < row)
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo('a'));
                    else if (r < row && c < column)
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo('a'));
                    else
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo(' '));
                }
            }
        }

        [Test]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition()
        {
            var row = 2;
            var column = 3;
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(0);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (r < row)
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo('a'));
                    else if (r < row && c < column)
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo('a'));
                    else
                        Assert.That(Screen.Character(new Position(r + 1, c + 1)).Char, Is.EqualTo(' '));
                }
            }
        }


        private void EraseScreen(int jArg)
        {
            Decode($"{Escape}{jArg}J");
        }
    }
}