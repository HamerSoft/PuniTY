using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests.ModeTests;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class ModeTests : AnsiDecoderTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(2, 2, new AlwaysValidModeFactory());
        }

        [Test]
        public void IMode_SetMote_Enables_Mode()
        {
            Screen.SetMode(AnsiMode.Origin);
            Assert.IsTrue(Screen.HasMode(AnsiMode.Origin));
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

            Screen.ModeChanged += OnMode;
            Screen.SetMode(AnsiMode.Origin);

            Assert.That(invoked, Is.True);
            Assert.That(enabled, Is.True);
            Assert.That(resultMode, Is.EqualTo(AnsiMode.Origin));

            resultMode = AnsiMode.KeyBoardAction;
            invoked = false;
            Screen.ResetMode(AnsiMode.Origin);

            Assert.That(invoked, Is.True);
            Assert.That(enabled, Is.False);
            Assert.That(resultMode, Is.EqualTo(AnsiMode.Origin));
            Screen.ModeChanged -= OnMode;
        }

        [Test]
        public void IMode_SetMode_Can_Enable_Multiple_Modes()
        {
            Screen.SetMode(AnsiMode.Origin);
            Screen.SetMode(AnsiMode.SendReceive);
            Screen.SetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.HasMode(AnsiMode.Origin, AnsiMode.SendReceive, AnsiMode.ReverseVideo));
        }

        [Test]
        public void IMode_HasMode_Returns_False_If_Mode_Not_Set()
        {
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.False);
        }

        [Test]
        public void IMode_HasMode_Returns_True_If_Mode_Set()
        {
            Screen.SetMode(AnsiMode.Origin);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
        }

        [Test]
        public void IMode_HasMode_Returns_True_If_MultipleModes_Mode_Set()
        {
            Screen.SetMode(AnsiMode.Origin);
            Screen.SetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.True);
        }

        [Test]
        public void IMode_Reset_Mode_DoesNotThrow_If_Mode_Not_Set()
        {
            Assert.DoesNotThrow(() =>
            {
                Screen.ResetMode(AnsiMode.Origin);
                Assert.IsFalse(Screen.HasMode(AnsiMode.Origin));
            });
        }

        [Test]
        public void IMode_Reset_Mode_Removes_Mode()
        {
            Screen.SetMode(AnsiMode.Origin);
            Screen.SetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.HasMode(AnsiMode.Origin, AnsiMode.ReverseVideo));

            Screen.ResetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.False);

            Screen.ResetMode(AnsiMode.Origin);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.False);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.False);
        }
    }
}