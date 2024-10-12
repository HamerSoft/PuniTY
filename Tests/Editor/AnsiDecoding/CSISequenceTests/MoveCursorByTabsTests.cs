using System;
using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class MoveCursorByTabsTests : AnsiDecoderTest
    {
        private const int ScreenRows = 2;
        private const int ScreenColumns = 80;

        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(ScreenRows, ScreenColumns, typeof(MoveCursorForwardTabsSequence),
                typeof(MoveCursorBackwardTabsSequence),
                typeof(ResetTabStopSequence),
                typeof(TabClearSequence));
        }

        [TestCase("", 1, 8)]
        [TestCase("1", 1, 8)]
        [TestCase("1", 3, 8)]
        [TestCase("1", 7, 8)]
        [TestCase("4", 7, 32)]
        [TestCase("2", 32, 48)]
        [TestCase("10", 7, 80)]
        public void MoveCursorToNextTab_Moves_The_Cursor_To_Next_TabStop(string parameter, int startPosition,
            int expectedPosition)
        {
            Screen.Cursor.SetPosition(new Position(1, startPosition));
            Decode($"{Escape}{parameter}I");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void MoveCursorToNextTab_Logs_Warning_When_Tab_OutOfBounds()
        {
            Screen.Cursor.SetPosition(new Position(1, 9));
            Decode($"{Escape}300I");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(9));
        }

        [TestCase("", 9, 1)]
        [TestCase("1", 9, 1)]
        [TestCase("2", 50, 32)]
        [TestCase("10", 80, 1)]
        public void MoveCursorToPreviousTab_Moves_The_Cursor_To_Backward_TabStop(string parameter, int startPosition,
            int expectedPosition)
        {
            Screen.Cursor.SetPosition(new Position(1, startPosition));
            Decode($"{Escape}{parameter}Z");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void MoveCursorToPreviousTab_Logs_Warning_When_Moving_Forward()
        {
            Screen.Cursor.SetPosition(new Position(1, 9));
            Decode($"{Escape}3Z");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(9));
        }

        [Test]
        public void MoveCursorToPreviousTab_Logs_Warning_When_Tab_OutOfBounds()
        {
            Screen.Cursor.SetPosition(new Position(1, 9));
            Decode($"{Escape}300Z");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(9));
        }

        [TestCase(2, 24)]
        [TestCase(4, 40)]
        [TestCase(10, 80)]
        public void MoveCursorToNextTab_Skips_Cleared_TabStops(int tabStopsToClear, int expectedColumn)
        {
            for (int i = 0; i < tabStopsToClear; i++)
                Screen.ClearTabStop(i * 8);
            Decode($"{Escape}1I");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(expectedColumn));
        }

        [TestCase(2, 56)]
        [TestCase(4, 40)]
        [TestCase(10, 1)]
        public void MoveCursorToPreviousTab_Skips_Cleared_TabStops(int tabStopsToClear, int expectedColumn)
        {
            Screen.SetCursorPosition(new Position(1, 80));
            for (int i = 0; i < tabStopsToClear; i++)
                Screen.ClearTabStop(80 - i * 8);
            Decode($"{Escape}1Z");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(expectedColumn));
        }

        [Test]
        public void ResetTabStops_Removes_Cleared_TabStops()
        {
            Screen.ClearTabStop(8);
            Decode($"{Escape}1I");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(16));
            Decode($"{Escape}?5W");
            Screen.SetCursorPosition(new Position(1, 1));
            Decode($"{Escape}1I");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(8));
        }

        [Test]
        public void TabClearSequence_Clears_By_Next_TabStop_By_Default()
        {
            Decode($"{Escape}g");
            Assert.That(Screen.GetNextTabStop(1), Is.EqualTo(2));
        }

        [TestCase(14, 3)]
        [TestCase(24, 4)]
        [TestCase(1, 2)]
        public void TabClearSequence_Clears_By_Next_TabStop(int column, int expectedTabStop)
        {
            Screen.SetCursorPosition(new Position(1, column));
            Decode($"{Escape}0g");
            Assert.That(Screen.GetNextTabStop(column), Is.EqualTo(expectedTabStop));
        }
    }
}