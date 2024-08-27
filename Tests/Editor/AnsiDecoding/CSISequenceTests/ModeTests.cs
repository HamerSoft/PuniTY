using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class ModeTests : AnsiDecoderTest
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
        public void Public_SetMode_2h_Sets_KeyboardActionMode()
        {
            SetMode(2, true);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.True);
        }

        [Test]
        public void Public_ResetMode_2l_Resets_KeyboardActionMode()
        {
            SetMode(2, true);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.True);
            ResetMode(2, true);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.False);
        }

        private void SetMode(int command, bool isPublic)
        {
            Decode($"{Escape}{(isPublic ? "" : "?")}{command}h");
        }

        private void ResetMode(int command, bool isPublic)
        {
            Decode($"{Escape}{(isPublic ? "" : "?")}{command}l");
        }
    }
}