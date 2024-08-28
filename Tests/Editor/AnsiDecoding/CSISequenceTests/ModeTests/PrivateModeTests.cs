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