using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.MediaCopy;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class MediaCopySequenceTests : AnsiDecoderTest
    {
        private const int Rows = 5;
        private readonly int Columns = 10;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(Rows, Columns);
            AnsiDecoder = new AnsiDecoder(Screen, EscapeCharacterDecoder,
                CreateSequence(typeof(MediaCopySequence)));
        }

        [Test]
        public void MediaCopySequence_Normal_i0_PrintScreen_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(0, false);
        }

        [Test]
        public void MediaCopySequence_Normal_i4_TurnOffPrinterController_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(4, false);
        }

        [Test]
        public void MediaCopySequence_Normal_i5_TurnOnPrinterController_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(5, false);
        }

        [Test]
        public void MediaCopySequence_Normal_i10_HTMLScreenDump_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(10, false);
        }

        [Test]
        public void MediaCopySequence_Normal_i11_PrintScreen_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(11, false);
        }

        [Test]
        public void MediaCopySequence_DecSpecific_i1_PrintLineContainingCursor_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(1, true);
        }

        [Test]
        public void MediaCopySequence_DecSpecific_i4_TurnOffAutoPrint_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(4, true);
        }

        [Test]
        public void MediaCopySequence_DecSpecific_i5_TurnOnAutoPrint_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(5, true);
        }

        [Test]
        public void MediaCopySequence_DecSpecific_i10_PrintComposedDisplay_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(10, true);
        }

        [Test]
        public void MediaCopySequence_DecSpecific_i11_PrintAllPages_LogsNotImplemented()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            DecodeMediaCopy(11, true);
        }

        private void DecodeMediaCopy(int argument, bool isDecSpecific)
        {
            Decode($"{Escape}{(isDecSpecific ? "?" : "")}{argument}i");
        }
    }
}