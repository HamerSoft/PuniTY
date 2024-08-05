using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.SequenceTypes
{
    public abstract class ESCSequence : Sequence
    {
        public override SequenceType SequenceType => SequenceType.ESC;

        protected ESCSequence(ILogger logger) : base(logger)
        {
        }
    }
}