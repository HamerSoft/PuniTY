using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class CommandSeparatorTests
    {
        private EmptySequence _sequence;

        private class EmptySequence : ESCSequence
        {
            public override char Command { get; }

            public EmptySequence(HamerSoft.PuniTY.Logging.ILogger logger) : base(logger)
            {
            }

            public override void Execute(IScreen screen, string parameters)
            {
            }

            internal int[] GetCommandArguments(string parameters, int expectedAmount, int defaultValue)
            {
                return base.GetCommandArguments(parameters, expectedAmount, defaultValue);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _sequence = new EmptySequence(null);
        }

        [TestCase("1;1", 2, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(";1", 2, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase("1", 2, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(";2", 2, 3, ExpectedResult = new[] { 3, 2 })]
        [TestCase("2", 2, 4, ExpectedResult = new[] { 2, 4 })]
        [TestCase(";;1", 3, 3, ExpectedResult = new[] { 3, 3, 1 })]
        [TestCase("1", 3, 3, ExpectedResult = new[] { 1, 3, 3 })]
        [TestCase("", 2, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(";", 2, 1, ExpectedResult = new[] { 1, 1 })]
        public int[] Sequence_Can_Parse_All_Arguments(string parameters, int length, int defaultValue)
        {
            var actual = _sequence.GetCommandArguments(parameters, length, defaultValue);
            return actual;
        }
    }
}