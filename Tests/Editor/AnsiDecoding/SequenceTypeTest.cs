using System;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class SequenceTypeTest : AnsiDecoderTest
    {
        private const char CommandChar = 'R';

        private class ESCSequence : AnsiEncoding.SequenceTypes.ESCSequence
        {
            private readonly Action _callback;

            public ESCSequence(ILogger logger, Action callback) : base(logger)
            {
                _callback = callback;
            }

            public override char Command => CommandChar;

            public override void Execute(IScreen screen, string parameters)
            {
                _callback?.Invoke();
            }
        }

        private class CSISequence : AnsiEncoding.SequenceTypes.CSISequence
        {
            private readonly Action _callback;

            public CSISequence(ILogger logger, Action callback) : base(logger)
            {
                _callback = callback;
            }

            public override char Command => CommandChar;

            public override void Execute(IScreen screen, string parameters)
            {
                _callback?.Invoke();
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(10, 10);
        }

        [Test]
        public void AnsiDecoder_Throws_Exception_When_DuplicateCommand_Args_Are_Given()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                AnsiDecoder = new AnsiDecoder(Screen,
                    EscapeCharacterDecoder, new ESCSequence(null, null), new ESCSequence(null, null));
            });
        }


        [Test]
        public void Sequence_With_Correct_ESC_SequenceType_Is_Executed_When_Escaped_Width_ESC()
        {
            bool escExecuted = false;

            void ESC()
            {
                escExecuted = true;
            }

            bool csiExecuted = false;

            void CSI()
            {
                csiExecuted = true;
            }

            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder, new ESCSequence(null, ESC), new CSISequence(null, CSI));
            Decode($"\x001b{CommandChar}");
            Assert.That(escExecuted, Is.True);
            Assert.That(csiExecuted, Is.False);
        }

        [Test]
        public void Sequence_With_Correct_CSI_SequenceType_Is_Executed_When_Escaped_Width_CSI()
        {
            bool escExecuted = false;

            void ESC()
            {
                escExecuted = true;
            }

            bool csiExecuted = false;

            void CSI()
            {
                csiExecuted = true;
            }

            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder, new ESCSequence(null, ESC), new CSISequence(null, CSI));
            Decode($"{Escape}{CommandChar}");
            Assert.That(escExecuted, Is.False);
            Assert.That(csiExecuted, Is.True);
        }
    }
}