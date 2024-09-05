using HamerSoft.PuniTY.AnsiEncoding;
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
            Screen = new MockScreen(2, 2);
        }

        [Test]
        public void IMode_By_Default_Is_None()
        {
            Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.None));
        }

        [Test]
        public void IMode_SetMote_Removes_Mode_None()
        {
            Screen.SetMode(AnsiMode.Origin);
            Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.Origin));
            Assert.That(Screen.Mode, Is.Not.EqualTo(AnsiMode.None));
        }

        [Test]
        public void IMode_SetMote_Enables_Mode()
        {
            Screen.SetMode(AnsiMode.Origin);
            Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.Origin));
        }

        [Test]
        public void IMode_SetMode_Can_Enable_Multiple_Modes()
        {
            Screen.SetMode(AnsiMode.Origin);
            Screen.SetMode(AnsiMode.SendReceive);
            Screen.SetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.Origin | AnsiMode.SendReceive | AnsiMode.ReverseVideo));
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
                Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.None));
            });
        }

        [Test]
        public void IMode_Reset_Mode_Removes_Mode()
        {
            Screen.SetMode(AnsiMode.Origin);
            Screen.SetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.Mode, Is.EqualTo(AnsiMode.Origin | AnsiMode.ReverseVideo));
            
            Screen.ResetMode(AnsiMode.ReverseVideo);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.True);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.False);
            
            Screen.ResetMode(AnsiMode.Origin);
            Assert.That(Screen.HasMode(AnsiMode.Origin), Is.False);
            Assert.That(Screen.HasMode(AnsiMode.ReverseVideo), Is.False);
            Assert.That(Screen.HasMode(AnsiMode.None), Is.True);
        }
    }
}