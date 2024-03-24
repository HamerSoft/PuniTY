using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using EscapeCharacterDecoder = HamerSoft.PuniTY.AnsiEncoding.EscapeCharacterDecoder;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public class MockEscapeCharacterDecoder
    {
        public string Characters = "";
        public char Command = ' ';
        public string Parameters = "";
        internal readonly IEscapeCharacterDecoder EscapeCharacterDecorder;

        public MockEscapeCharacterDecoder(IEscapeCharacterDecoder decoder)
        {
            EscapeCharacterDecorder = decoder;
            EscapeCharacterDecorder.ProcessOutput += OnCharacters;
            EscapeCharacterDecorder.ProcessCommand += OnProcessCommand;
        }

        private void OnCharacters(byte[] data)
        {
            int charCount = EscapeCharacterDecorder.Encoding.GetCharCount(data, 0, 1);
            char[] characters = new char[charCount];
            EscapeCharacterDecorder.Encoding.GetChars(data, 0, 1, characters, 0);

            Characters += characters;
        }

        private void OnProcessCommand(byte command, string parameter)
        {
            Command = (char)command;
            Parameters = parameter;
        }
    }

    [TestFixture]
    public class EscapeCharacterDecoderTests : AnsiDecoderTest
    {
        private MockEscapeCharacterDecoder _decoder;

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
            _decoder = new MockEscapeCharacterDecoder(EscapeCharacterDecoder);
        }

        [Test]
        public void EscapeCharacterDecoder_Can_Detect_Private_CSI_Command()
        {
            string command = "\x001b[?2004h";
            Decode(command);
            Assert.That(_decoder.Command, Is.EqualTo('h'));
            Assert.That(_decoder.Parameters, Is.EqualTo("?2004"));
        }

        [Test]
        public void EscapeCharacterDecoder_Can_Detect_CSI_Command()
        {
            string command = "\x001b[2J";
            Decode(command);
            Assert.That(_decoder.Command, Is.EqualTo('J'));
            Assert.That(_decoder.Parameters, Is.EqualTo("2"));
        }

        [Test]
        public void EscapeCharacterDecoder_Can_Detect_CSI_Command_WithoutParams()
        {
            string command = "\x001b[m";
            Decode(command);
            Assert.That(_decoder.Command, Is.EqualTo('m'));
            Assert.That(_decoder.Parameters, Is.EqualTo(""));
        }

        [Test]
        public void EscapeCharacterDecoder_Can_Detect_OSC_Command()
        {
            string command = "\x001b]0;MINGW64:/c/Users/ruben/Projects/Unity/PuniTY";
            Decode(command, 0x07);
            Assert.That('\a', Is.EqualTo(_decoder.Command));
            Assert.That("0;MINGW64:/c/Users/ruben/Projects/Unity/PuniTY", Is.EqualTo(_decoder.Parameters));
        }

        [Test]
        public void TestByteRange()
        {
            var myByte = 0x45;
            var start = 0x40;
            var end = 0x5F;
            bool inRange = myByte >= start && myByte <= end;
            bool outOfRange = myByte >= start && myByte <= 0x42;
            bool belowOfRange = 0x30 >= start && 0x30 <= end;
            Assert.IsTrue(inRange);
            Assert.IsFalse(outOfRange);
            Assert.IsFalse(belowOfRange);
        }
    }
}