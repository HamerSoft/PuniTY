using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.SequenceTypes
{
    public abstract class OSCSequence : Sequence
    {
        public override SequenceType SequenceType => SequenceType.OSC;

        protected OSCSequence(ILogger logger) : base(logger)
        {
        }
    }
}