﻿using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public class PointerModeTests : AnsiDecoderTest
    {
        private PointerModeFactory _pointerModeFactory;

        private Rect _bounds = new Rect(0, 0, 100, 100);
        private StubPointer _stubPointer;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _stubPointer = new StubPointer(new NeverHide(), new Vector2(0, 0), _bounds);
            Screen = new MockScreen(5, 5);
            _pointerModeFactory = new PointerModeFactory();
        }

        [Test]
        public void PointerMode_NeverHide_Shows_Pointer_In_And_Outside_Bounds()
        {
            ((IPointer)_stubPointer).SetMode(new NeverHide());
            _stubPointer.SetPosition(new Vector2(10, 10), _bounds);
            Assert.That(_stubPointer.IsActive, Is.True);
            _stubPointer.SetPosition(new Vector2(200, 200), _bounds);
        }

        [Test]
        public void PointerMode_HideWhenTrackingDisabled_Hides_Pointer_When_Tracking_Is_Disabled()
        {
            ((IPointer)_stubPointer).SetMode(new HideWhenTrackingDisabled(Screen));
            Assert.That(_stubPointer.IsActive, Is.False);
            ((IPointerable)Screen).SetPointerMode(PointerMode.HideIfNotTracking);
            Screen.SetMode(AnsiMode.UseAllMotionMouseTracking);
            Assert.That(_stubPointer.IsActive, Is.True);
        }
    }
}