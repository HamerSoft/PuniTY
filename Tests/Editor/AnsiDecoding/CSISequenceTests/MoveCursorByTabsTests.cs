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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(ScreenRows, ScreenColumns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                CreateSequence(typeof(MoveCursorForwardTabsSequence),
                    typeof(MoveCursorBackwardTabsSequence),
                    typeof(ResetTabStopSequence)));
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

        [Test]
        public void ResetTabStops_Logs_Warning_Not_Supported()
        {
            Decode($"{Escape}?5W");
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }
    }
}