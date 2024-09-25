using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests.ModeTests;
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
            Screen = new MockScreen(Rows, Columns, new AlwaysValidModeFactory());
            AnsiDecoder = new AnsiDecoder(Screen, EscapeCharacterDecoder,
                CreateSequence(
                    typeof(SetModeSequence),
                    typeof(ResetModeSequence)));
        }

        [TestCase(2, AnsiMode.KeyBoardAction)]
        [TestCase(4, AnsiMode.Insert)]
        [TestCase(12, AnsiMode.SendReceive)]
        [TestCase(20, AnsiMode.AutomaticNewLine)]
        public void Public_SetMode_Sets_AnsiMode(int command, AnsiMode expectedCommand)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedCommand), Is.True);
        }

        [TestCase(2, AnsiMode.KeyBoardAction)]
        [TestCase(4, AnsiMode.Insert)]
        [TestCase(12, AnsiMode.SendReceive)]
        [TestCase(20, AnsiMode.AutomaticNewLine)]
        public void Public_ResetMode_Resets_AnsiMode(int command, AnsiMode expectedCommand)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedCommand), Is.True);
            ResetMode(command);
            Assert.That(Screen.HasMode(expectedCommand), Is.False);
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