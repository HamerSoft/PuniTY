using System;
using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.EraseSequences;
using HamerSoft.PuniTY.AnsiEncoding.ScrollSequences;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            PopulateScreen();
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(ScreenRows, ScreenColumns, typeof(ScrollDownSequence),
                typeof(ScrollUpSequence));
        }

        private void PopulateScreen()
        {
            for (int i = 0; i < ScreenRows; i++)
            for (int j = 0; j < ScreenColumns; j++)
                Screen.AddCharacter(DefaultChar);
            Screen.SetCursorPosition(new Position(1, 1));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void When_Adding_Character_GreaterThan_ScreenMax_New_Row_Is_Added_And_OffSet_Is_Increased(
            int linesToScroll)
        {
            Scroll(linesToScroll, Direction.Up);
            Screen.SetCursorPosition(new Position(ScreenRows + 1 - linesToScroll, 1));
            for (int i = 0; i < linesToScroll; i++)
            for (int j = 0; j < ScreenColumns; j++)
                Screen.AddCharacter('g');

            PrintScreen();
            AssertScreen(ScreenRows + 1 - linesToScroll, 1, DefaultChar, 'g', true);
            Scroll(linesToScroll, Direction.Down);
            PrintScreen();
            AssertScreen(ScreenRows, ScreenColumns, DefaultChar, DefaultChar, true);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void When_Adding_Character_SmallerThan_ScreenMax_New_Row_Is_Added_And_OffSet_Is_Increased(
            int linesToScroll)
        {
            Scroll(linesToScroll, Direction.Down);
            Screen.SetCursorPosition(new Position(1, 1));
            for (int i = 0; i < linesToScroll; i++)
            for (int j = 0; j < ScreenColumns; j++)
                Screen.AddCharacter('g');

            PrintScreen();
            AssertScreen(linesToScroll + 1, ScreenColumns, 'g', DefaultChar, true);
            Scroll(linesToScroll, Direction.Up);
            PrintScreen();
            AssertScreen(ScreenRows, ScreenColumns, DefaultChar, DefaultChar, true);
        }

        [Test]
        public void ScrollDownSequence_Checks_For_MouseTracking_And_Logs_WarningNotImplemented()
        {
            Decode($"{Escape}1;8;2;3T");
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        private void Scroll(int lines, Direction direction)
        {
            Decode(
                $"{Escape}{lines}{(direction switch { Direction.Up => "S", Direction.Down => "T", _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null) })}");
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