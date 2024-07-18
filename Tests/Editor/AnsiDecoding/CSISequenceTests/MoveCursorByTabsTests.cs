using System;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class MoveCursorByTabsTests : AnsiDecoderTest
    {
        private const int ScreenRows = 1;
        private const int ScreenColumns = 80;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(ScreenRows, ScreenColumns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new MoveCursorForwardTabsSequence());
        }

        [TestCase("1")]
        public void MoveCursorToNextTab_Moves_The_Cursor_To_Next_TabStop(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}