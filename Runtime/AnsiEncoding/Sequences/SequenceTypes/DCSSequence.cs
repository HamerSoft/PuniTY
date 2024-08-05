using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.SequenceTypes
{
    public abstract class DCSSequence : Sequence
    {
        public override SequenceType SequenceType => SequenceType.DCS;

        protected DCSSequence(ILogger logger) : base(logger)
        {
        }
    }
}