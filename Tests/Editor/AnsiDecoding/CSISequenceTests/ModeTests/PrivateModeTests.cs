using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class PrivateModeTests : AnsiDecoderTest
    {
        public const int Rows = 25;
        public const int Columns = 80;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(Rows, Columns);
            AnsiDecoder = new AnsiDecoder(Screen, EscapeCharacterDecoder,
                CreateSequence(
                    typeof(SetModeSequence),
                    typeof(ResetModeSequence)));
        }

        [TestCase(1, AnsiMode.ApplicationCursorKeys)]
        [TestCase(2, AnsiMode.DECANM)]
        [TestCase(3, AnsiMode.DECCOLM)]
        [TestCase(4, AnsiMode.SmoothScroll)]
        [TestCase(5, AnsiMode.ReverseVideo)]
        [TestCase(6, AnsiMode.Origin)]
        [TestCase(7, AnsiMode.AutoWrap)]
        [TestCase(8, AnsiMode.AutoRepeatKeys)]
        [TestCase(9, AnsiMode.SendMouseXY)]
        [TestCase(10, AnsiMode.ShowToolbar)]
        [TestCase(12, AnsiMode.BlinkingCursor)]
        [TestCase(13, AnsiMode.StartBlinkingCursor)]
        [TestCase(14, AnsiMode.XORBlinkingCursor)]
        [TestCase(18, AnsiMode.PrintFormFeed)]
        [TestCase(19, AnsiMode.PrintExtentFullScreen)]
        public void Private_SetMode_Sets_Correct_AnsiMode(int command, AnsiMode expectedMode)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.True);
        }

        [TestCase(1, AnsiMode.ApplicationCursorKeys)]
        [TestCase(2, AnsiMode.DECANM)]
        [TestCase(3, AnsiMode.DECCOLM)]
        [TestCase(4, AnsiMode.SmoothScroll)]
        [TestCase(5, AnsiMode.ReverseVideo)]
        [TestCase(6, AnsiMode.Origin)]
        [TestCase(7, AnsiMode.AutoWrap)]
        [TestCase(8, AnsiMode.AutoRepeatKeys)]
        [TestCase(9, AnsiMode.SendMouseXY)]
        [TestCase(10, AnsiMode.ShowToolbar)]
        [TestCase(12, AnsiMode.BlinkingCursor)]
        [TestCase(13, AnsiMode.StartBlinkingCursor)]
        [TestCase(14, AnsiMode.XORBlinkingCursor)]
        [TestCase(18, AnsiMode.PrintFormFeed)]
        [TestCase(19, AnsiMode.PrintExtentFullScreen)]
        public void Private_ResetMode_Resets_Correct_AnsiMode(int command, AnsiMode expectedMode)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.True);
            ResetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.False);
        }

        private void SetMode(int command)
        {
            Decode($"{Escape}?{command}h");
        }

        private void ResetMode(int command)
        {
            Decode($"{Escape}?{command}l");
        }
    }
}