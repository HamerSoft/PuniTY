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
                CreateSequence(typeof(MoveCursorForwardTabsSequence)));
        }

        [TestCase("1", 1, 8)]
        [TestCase("1", 3, 8)]
        [TestCase("1", 7, 8)]
        [TestCase("4", 7, 32)]
        [TestCase("10", 7, 80)]
        public void MoveCursorToNextTab_Moves_The_Cursor_To_Next_TabStop(string parameter, int startPosition,
            int expectedPosition)
        {
            Screen.Cursor.SetPosition(new Position(1, startPosition));
            Decode($"{Escape}{parameter}I");
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void MoveCursorToNextTab_Logs_Warning_When_Moving_Backwards()
        {
            Screen.Cursor.SetPosition(new Position(1, 9));
            Decode($"{Escape}1I");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(9));
        }

        [Test]
        public void MoveCursorToNextTab_Logs_Warning_When_Tab_OutOfBounds()
        {
            Screen.Cursor.SetPosition(new Position(1, 9));
            Decode($"{Escape}300I");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(9));
        }
    }
}