using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class GraphicsRenditionTests : AnsiDecoderTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(10, 10);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new GraphicsRenditionSequence());
        }

        [Test]
        public void When_SGR_0_All_Attributes_Are_Reset()
        {
            Decode($"{Escape}0m");
            Assert.That(new GraphicAttributes(AnsiColor.White, AnsiColor.Black), Is.EqualTo(GetGraphicsAttributes()));
        }

        private GraphicAttributes GetGraphicsAttributes()
        {
            Screen.AddCharacter('a');
            return Screen.GetCharacter(new Position(1, 1)).GraphicAttributes;
        }
    }
}