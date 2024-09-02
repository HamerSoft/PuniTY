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

        [Test]
        public void Private_SetMode_6h_Sets_Origin_Mode()
        {
            SetMode(6);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
        }

        [Test]
        public void Private_ResetMode_6l_Resets_Origin_Mode()
        {
            SetMode(6);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
            ResetMode(6);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.False);
        }

        [Test]
        public void Private_SetMode_7h_Sets_Origin_Mode()
        {
            SetMode(7);
            Assert.That(Screen.HasMode(AnsiMode.AutoWrap), Is.True);
        }

        [Test]
        public void Private_ResetMode_7l_Resets_Origin_Mode()
        {
            SetMode(7);
            Assert.That(Screen.HasMode(AnsiMode.AutoWrap), Is.True);
            ResetMode(7);
            Assert.That(Screen.HasMode(AnsiMode.AutoWrap), Is.False);
        }

        [Test]
        public void Private_SetMode_8h_Sets_Origin_Mode()
        {
            SetMode(8);
            Assert.That(Screen.HasMode(AnsiMode.AutoRepeatKeys), Is.True);
        }

        [Test]
        public void Private_ResetMode_8l_Resets_Origin_Mode()
        {
            SetMode(8);
            Assert.That(Screen.HasMode(AnsiMode.AutoRepeatKeys), Is.True);
            ResetMode(8);
            Assert.That(Screen.HasMode(AnsiMode.AutoRepeatKeys), Is.False);
        }

        [Test]
        public void Private_SetMode_9h_Sets_Origin_Mode()
        {
            SetMode(9);
            Assert.That(Screen.HasMode(AnsiMode.SendMouseXY), Is.True);
        }

        [Test]
        public void Private_ResetMode_9l_Resets_Origin_Mode()
        {
            SetMode(9);
            Assert.That(Screen.HasMode(AnsiMode.SendMouseXY), Is.True);
            ResetMode(9);
            Assert.That(Screen.HasMode(AnsiMode.SendMouseXY), Is.False);
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