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
        [TestCase("?!>. <")]
        public void AnsiDecoder_Can_Return_Output_When_No_Commands_Are_Given(string input)
        {
            Decode(input);
            Assert.That(GetOutput(), Is.EqualTo(input));
        }

        [TestCase("\x001b[?2004hhello world", "hello world")]
        [TestCase("\x001b[?2004h!hello world", "!hello world")]
        [TestCase("\x001b[?2004h!>hello !world", "!>hello !world")]
        
        [TestCase("\x001b[2Jhello world", "hello world")]
        [TestCase("\x001b[2J!hello world", "!hello world")]
        [TestCase("\x001b[2J!>hello !world", "!>hello !world")]
        
        [TestCase("\x001b7hello world", "hello world")]
        [TestCase("\x001b7!hello world", "!hello world")]
        [TestCase("\x001b7!>hello !world", "!>hello !world")]
        
        [TestCase("\x001b[mhello world", "hello world")]
        [TestCase("\x001b[m!hello world", "!hello world")]
        [TestCase("\x001b[m!>hello !world", "!>hello !world")]
        public void AnsiDecoder_Can_Return_Output_After_Commands_Are_Parsed(string input, string output)
        {
            Decode(input);
            Assert.That(GetOutput(), Is.EqualTo(output));
        }

        private string GetOutput()
        {
            return AnsiContext.Screen.ToString().Replace('\0', ' ').TrimEnd();
        }
    }
}