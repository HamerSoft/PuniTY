﻿using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;

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
            Screen = new MockScreen(5, 5);
            Screen.PointerModeChanged += ScreenOnPointerModeChanged;
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder, CreateSequence(typeof(ResourceSequence)));
        }

        private void ScreenOnPointerModeChanged(IPointerMode mode)
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
    }
}