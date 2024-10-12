using System;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Logging;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
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
            public override char Command => CommandChar;

            public ESCSequence(Action callback)
            {
                _callback = callback;
            }
            public override void Execute(IAnsiContext context, string parameters)
            {
                _callback?.Invoke();
            }
        }
        private class CSISequence : AnsiEncoding.SequenceTypes.CSISequence
        {
            private readonly Action _callback;
            public override char Command => CommandChar;

            public CSISequence(Action callback)
            {
                _callback = callback;
            }
            public override void Execute(IAnsiContext context, string parameters)
            {
                _callback?.Invoke();
            }
        }
        
        
        protected override DefaultTestSetup DoTestSetup()
        {
            return DefaultSetup;
        }
        
        [Test]
        public void AnsiDecoder_Throws_Exception_When_DuplicateCommand_Args_Are_Given()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new StubAnsiContext(2,10, Logger,new ESCSequence(null), new ESCSequence(null));
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

            AnsiContext= new StubAnsiContext(2,10, Logger,new ESCSequence(ESC), new CSISequence(CSI));
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

            AnsiContext= new StubAnsiContext(2,10, Logger,new ESCSequence(ESC), new CSISequence(CSI));
            Decode($"{Escape}{CommandChar}");
            Assert.That(escExecuted, Is.False);
            Assert.That(csiExecuted, Is.True);
        }

    }
}