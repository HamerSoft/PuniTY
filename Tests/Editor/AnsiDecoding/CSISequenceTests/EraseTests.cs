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
        private const char EmptyCharacter = '\0';

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(ScreenRows, ScreenColumns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                CreateSequence(typeof(EraseDisplaySequence),
                    typeof(EraseLineSequence)));
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

        [TestCase(null)]
        [TestCase(0)]
        public void When_EraseLine_null_or_0_Clears_To_End_Of_line(int? kArg)
        {
            Screen.SetCursorPosition(new Position(3, 3));
            EraseLine(kArg);
            PrintScreen();
            AssertLine(3, 3, DefaultChar, EmptyCharacter, true);
        }

        [Test]
        public void When_EraseLine_1_Clears_To_beginning_Of_line()
        {
            Screen.SetCursorPosition(new Position(3, 3));
            EraseLine(1);
            PrintScreen();
            AssertLine(3, 3, DefaultChar, EmptyCharacter, false);
        }

        [Test]
        public void When_EraseLine_2_Clears_Entire_line()
        {
            Screen.SetCursorPosition(new Position(3, 3));
            EraseLine(2);
            PrintScreen();
            AssertLine(3, 1, DefaultChar, EmptyCharacter, true);
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
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, before));
                else if (r == row && toEnd && c < column)
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, before));
                else
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(after),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, after));
        }

        private void AssertLine(int row, int column, char before, char after, bool toEnd)
        {
            for (int r = 1; r <= ScreenRows; r++)
            for (int c = 1; c <= ScreenColumns; c++)
                if (r < row || r > row)
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, before));
                else if (toEnd)
                    if (c >= column)
                        Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(after),
                            GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, after));
                    else
                        Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before),
                            GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, before));
                else if (c <= column)
                {
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(after),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, after));
                }
                else
                {
                    Assert.That(Screen.GetCharacter(new Position(r, c)).Char, Is.EqualTo(before),
                        GetLogMessage(r, c, Screen.GetCharacter(new Position(r, c)).Char, before));
                }
        }

        private string GetLogMessage(int row, int column, char actual, char expected)
        {
            return $"Row {row}, Column {column}, Expected '{expected}' but was actual '{actual}'.";
        }

        private void EraseScreen(int jArg)
        {
            Decode($"{Escape}{jArg}J");
        }

        private void EraseLine(int? kArg)
        {
            if (kArg.HasValue)
                Decode($"{Escape}{kArg}K");
            else
                Decode($"{Escape}K");
        }
    }
}