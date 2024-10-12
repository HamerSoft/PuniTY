using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class ModeTests : AnsiDecoderTest
    {
        private IModeable _modeContext;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _modeContext = AnsiContext.TerminalModeContext;
        }

        protected override DefaultTestSetup DoTestSetup()
        {
            return DefaultSetup;
        }

        [Test]
        public void IMode_SetMote_Enables_Mode()
        {
            _modeContext.SetMode(AnsiMode.Origin);
            Assert.IsTrue(_modeContext.HasMode(AnsiMode.Origin));
        }

        [Test]
        public void IMode_When_Modes_Are_Toggled_Events_Are_Raised()
        {
            var resultMode = AnsiMode.KeyBoardAction;
            var invoked = false;
            var enabled = false;

            void OnMode(AnsiMode mode, bool isActive)
            {
                resultMode = mode;
                enabled = isActive;
                invoked = true;
            }

            _modeContext.ModeChanged += OnMode;
            _modeContext.SetMode(AnsiMode.Origin);

            Assert.That(invoked, Is.True);
            Assert.That(enabled, Is.True);
            Assert.That(resultMode, Is.EqualTo(AnsiMode.Origin));

            resultMode = AnsiMode.KeyBoardAction;
            invoked = false;
            _modeContext.ResetMode(AnsiMode.Origin);

            Assert.That(invoked, Is.True);
            Assert.That(enabled, Is.False);
            Assert.That(resultMode, Is.EqualTo(AnsiMode.Origin));
            _modeContext.ModeChanged -= OnMode;
        }

        [Test]
        public void IMode_SetMode_Can_Enable_Multiple_Modes()
        {
            _modeContext.SetMode(AnsiMode.Origin);
            _modeContext.SetMode(AnsiMode.SendReceive);
            _modeContext.SetMode(AnsiMode.ReverseVideo);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin, AnsiMode.SendReceive, AnsiMode.ReverseVideo));
        }

        [Test]
        public void IMode_HasMode_Returns_False_If_Mode_Not_Set()
        {
            Assert.That(_modeContext.HasMode(AnsiMode.Origin), Is.False);
        }

        [Test]
        public void IMode_HasMode_Returns_True_If_Mode_Set()
        {
            _modeContext.SetMode(AnsiMode.Origin);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin), Is.True);
        }

        [Test]
        public void IMode_HasMode_Returns_True_If_MultipleModes_Mode_Set()
        {
            _modeContext.SetMode(AnsiMode.Origin);
            _modeContext.SetMode(AnsiMode.ReverseVideo);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin), Is.True);
            Assert.That(_modeContext.HasMode(AnsiMode.ReverseVideo), Is.True);
        }

        [Test]
        public void IMode_Reset_Mode_DoesNotThrow_If_Mode_Not_Set()
        {
            Assert.DoesNotThrow(() =>
            {
                _modeContext.ResetMode(AnsiMode.Origin);
                Assert.IsFalse(_modeContext.HasMode(AnsiMode.Origin));
            });
        }

        [Test]
        public void IMode_Reset_Mode_Removes_Mode()
        {
            _modeContext.SetMode(AnsiMode.Origin);
            _modeContext.SetMode(AnsiMode.ReverseVideo);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin, AnsiMode.ReverseVideo));

            _modeContext.ResetMode(AnsiMode.ReverseVideo);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin), Is.True);
            Assert.That(_modeContext.HasMode(AnsiMode.ReverseVideo), Is.False);

            _modeContext.ResetMode(AnsiMode.Origin);
            Assert.That(_modeContext.HasMode(AnsiMode.Origin), Is.False);
            Assert.That(_modeContext.HasMode(AnsiMode.ReverseVideo), Is.False);
        }
    }
}