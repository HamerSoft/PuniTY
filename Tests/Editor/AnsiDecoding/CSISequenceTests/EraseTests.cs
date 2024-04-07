using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.EraseSequences;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
                Screen.AddCharacter(DefaultChar);
            Screen.SetCursorPosition(new Position(1, 1));
        }

        [Test]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition_No_Arg()
        {
            var row = 2;
            var column = 3;
            Screen.SetCursorPosition(new Position(row, column));
            Decode($"{Escape}J");
            AssertScreen(row, column, DefaultChar, EmptyCharacter, true);
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(11, 4)]
        public void When_Erase_Display_Clears_Rest_Of_Screen_Based_On_CursorPosition(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(0);
            PrintScreen();
            AssertScreen(row, column, DefaultChar, EmptyCharacter, true);
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(11, 4)]
        public void When_Erase_Display_Clears_Before_Of_Screen_Based_On_CursorPosition(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(1);
            PrintScreen();
            AssertScreen(row, column, EmptyCharacter, DefaultChar, false);
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(11, 4)]
        public void When_Erase_Display_Clears_Everything_No_Matter_Row_Or_Column(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(2);
            AssertScreen(ScreenRows, ScreenColumns, EmptyCharacter, DefaultChar, false);
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));
        }

        [TestCase(2, 3)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(11, 4)]
        public void When_Erase_ScrollBuffer_On_Display_Logs_Not_Implemented(int row, int column)
        {
            Screen.SetCursorPosition(new Position(row, column));
            EraseScreen(3);
            LogAssert.Expect(LogType.Log, new Regex(""));
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

        private void EraseScreen(int jArg)
        {
            Decode($"{Escape}{jArg}J");
        }
    }
}