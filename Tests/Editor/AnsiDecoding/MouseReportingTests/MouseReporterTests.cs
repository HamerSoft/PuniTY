using System.Numerics;
using System.Text;
using AnsiEncoding.Input;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.MouseReportingTests
{
    [TestFixture]
    public class MouseReporterTests : AnsiDecoderTest
    {
        private static readonly Rect Bounds = new Rect(0, 0, 100, 100);
        private StringBuilder _output;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _output = new StringBuilder();
            AnsiContext.Pointer.EnableTracking();
            AnsiContext.InputTransmitter.Output += InputTransmitterOnOutput;
        }

        public override void TearDown()
        {
            AnsiContext.InputTransmitter.Output -= InputTransmitterOnOutput;
            base.TearDown();
        }

        private void InputTransmitterOnOutput(byte[] output)
        {
            foreach (byte b in output)
                _output.Append((char)b);
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(10, 2);
        }

        [Test]
        public void PointerReportStrategy_Does_Not_Report_When_Tracking_Disabled()
        {
            AnsiContext.Pointer.DisableTracking();
            AnsiContext.Pointer.SetPosition(new Vector2(50, 50), Bounds);
            Assert.That(_output.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Pointer_InPixelMode_Reports_Mouse_Position_As_PositionInPixels()
        {
            AnsiContext.InputTransmitter.SetMouseReportingMode(new PixelReportStrategy(AnsiContext.Pointer));
            AnsiContext.Pointer.SetPosition(new Vector2(50, 50), Bounds);
            Assert.That(_output.ToString(), Is.EqualTo($"{Escape}M05050"));
        }

        [Test]
        public void Pointer_InCellMode_Reports_Mouse_Position_As_0_0()
        {
            AnsiContext.Pointer.SetPosition(new Vector2(50, 50), Bounds);
            Assert.That(_output.ToString(), Is.EqualTo($"{Escape}M000"));
        }
    }
}