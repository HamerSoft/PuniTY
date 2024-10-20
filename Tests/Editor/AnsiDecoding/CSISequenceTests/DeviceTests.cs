using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.Device;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            AnsiContext.InputTransmitter.Output += ScreenOnOutput;
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(10, 10, typeof(DeviceStatusReportSequence));
        }

        [Test]
        public void DeviceStatusReport_5n_Writes_Ok_To_Output()
        {
            Decode($"{Escape}5n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}0n"));
        }

        [Test]
        public void DeviceStatusReport_6n_Writes_CursorPosition_To_Output()
        {
            Screen.SetCursorPosition(new Position(4, 1));
            Decode($"{Escape}6n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}4;1;R"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_6n_Writes_CursorPosition_To_Output()
        {
            Screen.SetCursorPosition(new Position(4, 1));
            Decode($"{Escape}?6n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}4;1;R"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_15n_Reports_PrinterNotReady()
        {
            Decode($"{Escape}?15n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}11n"));
        }


        [Test]
        public void DeviceStatusReport_DecSpecific_25n_Reports_UDK_Unlocked()
        {
            Decode($"{Escape}?25n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}20n"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_26n_Reports_No_Errors()
        {
            Decode($"{Escape}?26n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}27;0n"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_55n_Reports_No_MouseIdentifier()
        {
            Decode($"{Escape}?55n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}57n"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_62n_Reports_Default_VT420Memory()
        {
            Decode($"{Escape}?62n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}" + "2048*{"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_63n_Reports_NoMacro_Memory()
        {
            Decode($"{Escape}?63n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}DCS0~0ST"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_75n_Reports_DataIntegrity_OK()
        {
            Decode($"{Escape}?75n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}70n"));
        }

        [Test]
        public void DeviceStatusReport_DecSpecific_83n_Reports_MultipleSessionSupport_No()
        {
            Decode($"{Escape}?83n");
            Assert.That(GetOutput(), Is.EqualTo($"{Escape}83n"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void DeviceStatusReport_DisableKeyModifiers_Logs_Not_Supported(int argument)
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            LogAssert.Expect(LogType.Warning,
                new Regex("Disable Key Modifier Options not supported! Skipping command."));
            Decode($"{Escape}>{argument.ToString()}n");
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

        public override void TearDown()
        {
            AnsiContext.InputTransmitter.Output -= ScreenOnOutput;
            base.TearDown();
        }
    }
}