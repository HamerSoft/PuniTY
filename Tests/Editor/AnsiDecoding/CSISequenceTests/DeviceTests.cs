using System.Collections.Generic;
using System.Text;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.Device;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class DeviceTests : AnsiDecoderTest
    {
        private List<byte> _output;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _output = new List<byte>();
            Screen = new MockScreen(10, 10);
            Screen.Output += ScreenOnOutput;
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new DeviceStatusReportSequence());
        }

        [Test]
        public void DeviceStatusReport_Writes_CursorPosition_To_Output()
        {
            Screen.SetCursorPosition(new Position(4, 1));
            Decode($"{Escape}6n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}4;1;R"));
        }

        private string GetOutput()
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in _output)
                builder.Append((char)b);
            _output.Clear();
            return builder.ToString();
        }

        private void ScreenOnOutput(byte[] data)
        {
            _output.AddRange(data);
        }
    }
}