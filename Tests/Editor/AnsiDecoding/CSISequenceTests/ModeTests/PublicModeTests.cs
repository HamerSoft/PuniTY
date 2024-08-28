using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class PublicModeTests : AnsiDecoderTest
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
        public void Public_SetMode_2h_Sets_KeyboardAction_Mode()
        {
            SetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.True);
        }

        [Test]
        public void Public_ResetMode_2l_Resets_KeyboardAction_Mode()
        {
            SetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.True);
            ResetMode(2);
            Assert.That(Screen.HasMode(AnsiMode.KeyBoardAction), Is.False);
        }

        [Test]
        public void Public_SetMode_4h_Sets_Insert_Mode()
        {
            SetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.Insert), Is.True);
        }

        [Test]
        public void Public_ResetMode_4l_Resets_Insert_Mode()
        {
            SetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.Insert), Is.True);
            ResetMode(4);
            Assert.That(Screen.HasMode(AnsiMode.Insert), Is.False);
        }

        [Test]
        public void Public_SetMode_12h_Sets_SendReceive_Mode()
        {
            SetMode(12);
            Assert.That(Screen.HasMode(AnsiMode.SendReceive), Is.True);
        }

        [Test]
        public void Public_ResetMode_12l_Resets_SendReceive_Mode()
        {
            SetMode(12);
            Assert.That(Screen.HasMode(AnsiMode.SendReceive), Is.True);
            ResetMode(12);
            Assert.That(Screen.HasMode(AnsiMode.SendReceive), Is.False);
        }

        [Test]
        public void Public_SetMode_20h_Sets_AutomaticNewLine_Mode()
        {
            SetMode(20);
            Assert.That(Screen.HasMode(AnsiMode.AutomaticNewLine), Is.True);
        }

        [Test]
        public void Public_ResetMode_20l_Resets_AutomaticNewLine_Mode()
        {
            SetMode(20);
            Assert.That(Screen.HasMode(AnsiMode.AutomaticNewLine), Is.True);
            ResetMode(20);
            Assert.That(Screen.HasMode(AnsiMode.AutomaticNewLine), Is.False);
        }


        private void SetMode(int command)
        {
            Decode($"{Escape}{command}h");
        }

        private void ResetMode(int command)
        {
            Decode($"{Escape}{command}l");
        }
    }
}