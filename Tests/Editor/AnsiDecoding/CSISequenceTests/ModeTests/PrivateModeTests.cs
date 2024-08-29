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

        [Test]
        public void Private_SetMode_1h_Sets_ApplicationCursorKeys_Mode()
        {
            SetMode(1);
            Assert.That(Screen.HasMode(AnsiMode.ApplicationCursorKeys), Is.True);
        }

        [Test]
        public void Private_ResetMode_1l_Resets_ApplicationCursorKeys_Mode()
        {
            SetMode(1);
            Assert.That(Screen.HasMode(AnsiMode.ApplicationCursorKeys), Is.True);
            ResetMode(1);
            Assert.That(Screen.HasMode(AnsiMode.ApplicationCursorKeys), Is.False);
        }

        [Test]
        public void Private_SetMode_2h_Sets_DECANM_Mode()
        {
            SetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.DECANM), Is.True);
        }

        [Test]
        public void Private_ResetMode_2l_Resets_DECANM_Mode()
        {
            SetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.DECANM), Is.True);
            ResetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.DECANM), Is.False);
        }

        [Test]
        public void Private_SetMode_3h_Sets_DECCOLM_Mode()
        {
            SetMode(3);
            Assert.That(Screen.HasMode(AnsiMode.DECCOLM), Is.True);
        }

        [Test]
        public void Private_ResetMode_3l_Resets_DECCOLM_Mode()
        {
            SetMode(3);
            Assert.That(Screen.HasMode(AnsiMode.DECCOLM), Is.True);
            ResetMode(3);
            Assert.That(Screen.HasMode(AnsiMode.DECCOLM), Is.False);
        }

        [Test]
        public void Private_SetMode_4h_Sets_SmoothScroll_Mode()
        {
            SetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.SmoothScroll), Is.True);
        }

        [Test]
        public void Private_ResetMode_4l_Resets_SmoothScroll_Mode()
        {
            SetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.SmoothScroll), Is.True);
            ResetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.SmoothScroll), Is.False);
        }
        
        [Test]
        public void Private_SetMode_5h_Sets_ReverseVideo_Mode()
        {
            SetMode(5);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.True);
        }

        [Test]
        public void Private_ResetMode_5l_Resets_ReverseVideo_Mode()
        {
            SetMode(5);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.True);
            ResetMode(5);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.False);
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