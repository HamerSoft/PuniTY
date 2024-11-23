using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;
using Tests.Editor.AnsiDecoding.Stubs;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public class PointerModeTests : AnsiDecoderTest
    {
        private Rect _bounds = new Rect(0, 0, 100, 100);
        private StubPointer _stubPointer;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _stubPointer = new StubPointer(new NeverHide(), new Vector2(0, 0), _bounds);
            AnsiContext = new StubAnsiContext(new StubInput(_stubPointer, new StubKeyboard()),
                new Screen.DefaultScreenConfiguration(5, 5, 8, new FontDimensions(10, 10)), Logger);
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return DefaultSetup;
        }

        [Test]
        public void PointerMode_NeverHide_Shows_Pointer_In_And_Outside_Bounds()
        {
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.NeverHide);
            _stubPointer.SetPosition(new Vector2(10, 10), _bounds);
            Assert.That(_stubPointer.IsActive, Is.True);
            _stubPointer.SetPosition(new Vector2(200, 200), _bounds);
        }

        [Test]
        public void PointerMode_HideWhenTrackingDisabled_Hides_Pointer_When_Tracking_Is_Disabled()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.HideIfNotTracking);
            Assert.That(_stubPointer.IsActive, Is.False);
        }

        [Test]
        public void PointerMode_HideWhenTrackingDisabled_Shows_Pointer_When_Tracking_Is_Enabled()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.HideIfNotTracking);
            AnsiContext.TerminalModeContext.SetMode(AnsiMode.SendMouseXY);
            Assert.That(_stubPointer.IsActive, Is.True);
        }

        [Test]
        public void PointerMode_HideInWindow_Hides_Pointer_When_InsideBounds()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.AlwaysHideInWindow);
            Assert.That(_stubPointer.IsActive, Is.False);
        }

        [Test]
        public void PointerMode_HideInWindow_Show_Pointer_When_OutsideBounds()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.AlwaysHideInWindow);
            _stubPointer.SetPosition(new Vector2(200, 0), _bounds);
            Assert.That(_stubPointer.IsActive, Is.True);
        }

        [Test]
        public void PointerMode_AlwaysHide_Hides_Pointer_In_And_OutSide_Bounds()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            AnsiContext.TerminalModeContext.SetPointerMode(PointerMode.AlwaysHide);
            Assert.That(_stubPointer.IsActive, Is.False);
            _stubPointer.SetPosition(new Vector2(200, 0), _bounds);
            Assert.That(_stubPointer.IsActive, Is.False);
        }

        [Test]
        public void PointerMode_NeverHide_Shows_Pointer_In_And_OutSide_Bounds()
        {
            Assert.That(_stubPointer.IsActive, Is.True);
            _stubPointer.SetPosition(new Vector2(200, 0), _bounds);
            Assert.That(_stubPointer.IsActive, Is.True);
        }
    }
}