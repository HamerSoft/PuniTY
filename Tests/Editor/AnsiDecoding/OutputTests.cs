using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public class OutputTests : AnsiDecoderTest
    {
        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(25, 80);
        }

        [TestCase("hello world")]
        [TestCase("hello! world")]
        [TestCase("hello! world!")]
        [TestCase("!hello! world!")]
        [TestCase("?!><")]
        public void AnsiDecoder_Can_Return_Output_When_No_Commands_Are_Given(string input)
        {
            Decode(input);
            Assert.That(AnsiContext.Screen.ToString().Replace('\0', ' ').TrimEnd(), Is.EqualTo(input));
        }

        [Test]
        public void AnsiDecoder_Can_Return_Output_When_No_Commands_Are_Given()
        {
            const string helloWorld = "!hello world";
            Decode(helloWorld);
            Assert.That(AnsiContext.Screen.ToString().Replace('\0', ' ').TrimEnd(), Is.EqualTo(helloWorld));
        }
    }
}