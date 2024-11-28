using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CursorMode = HamerSoft.PuniTY.AnsiEncoding.CursorMode;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class AttributeSequenceTests : AnsiDecoderTest
    {
        private CursorMode _cursorMode;

        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(5, 5, typeof(AttributesSequence));
        }

        public override void SetUp()
        {
            base.SetUp();
            AnsiContext.TerminalModeContext.CursorModeChanged += TerminalModeContextOnCursorModeChanged;
        }

        private void TerminalModeContextOnCursorModeChanged(CursorMode cursorMode)
        {
            _cursorMode = cursorMode;
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(21)]
        [TestCase(22)]
        [TestCase(23)]
        public void AttributeSequence_ps_q_Led_Logs_Not_Supported(int arg)
        {
            Decode($"{Escape}{arg}q");
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [TestCase(0, CursorMode.BlinkingBlock)]
        [TestCase(1, CursorMode.BlinkingBlock)]
        [TestCase(2, CursorMode.SteadyBlock)]
        [TestCase(3, CursorMode.BlinkingUnderline)]
        [TestCase(4, CursorMode.SteadyUnderLine)]
        [TestCase(5, CursorMode.BlinkingBar)]
        [TestCase(6, CursorMode.SteadyBar)]
        public void AttributeSequence_ps_SP_q_SetsCursorMode(int arg, CursorMode expectedMode)
        {
            Decode($"{Escape}{arg} q");
            Assert.That(_cursorMode, Is.EqualTo(expectedMode));
        }

        [Test]
        public void AttributeSequence_ps_Quote_SetCharacterProtection()
        {
            Decode($"{Escape}\"q");
            Assert.Fail();
        }

        public override void TearDown()
        {
            AnsiContext.TerminalModeContext.CursorModeChanged -= TerminalModeContextOnCursorModeChanged;
            base.TearDown();
        }
    }
}