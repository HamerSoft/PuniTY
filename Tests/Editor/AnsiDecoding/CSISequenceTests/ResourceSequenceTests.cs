using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;
using Tests.Editor.AnsiDecoding.Stubs;
using UnityEngine;
using UnityEngine.TestTools;
using Rect = HamerSoft.PuniTY.AnsiEncoding.Rect;
using Screen = HamerSoft.PuniTY.AnsiEncoding.Screen;
using Vector2 = System.Numerics.Vector2;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class ResourceSequenceTests : AnsiDecoderTest
    {
        private Rect _rect = new Rect(0, 0, 100, 100);
        private StubPointer _pointer;
        private IPointerMode _currentMode;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _pointer = new StubPointer(new NeverHide(), new Vector2(0, 0), _rect);
            AnsiContext = new StubAnsiContext(new StubInput(_pointer, new StubKeyboard()),
                new Screen.DefaultScreenConfiguration(5, 5, 8, new FontDimensions(10, 10)),
                Logger,
                new ResourceSequence());
            AnsiContext.TerminalModeContext.PointerModeChanged += ContextOnPointerModeChanged;
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return DefaultSetup;
        }

        private void ContextOnPointerModeChanged(IPointerMode mode)
        {
            ((IPointer)_pointer).SetMode(mode);
            _currentMode = mode;
        }

        [TestCase(0, PointerMode.NeverHide)]
        [TestCase(1, PointerMode.HideIfNotTracking)]
        [TestCase(2, PointerMode.AlwaysHideInWindow)]
        [TestCase(3, PointerMode.AlwaysHide)]
        public void ResourceSequence_x_p_Sets_PointerMode(int argument, PointerMode expectedMode)
        {
            Decode($"{Escape}>{argument}p");
            Assert.That(_currentMode.Mode, Is.EqualTo(expectedMode));
        }

        [Test]
        public void ResourceSequence_SoftTerminalReset_Not_ImplementedWarning()
        {
            Decode($"{Escape}>!p");
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        public override void TearDown()
        {
            AnsiContext.TerminalModeContext.PointerModeChanged -= ContextOnPointerModeChanged;
            base.TearDown();
        }
    }
}