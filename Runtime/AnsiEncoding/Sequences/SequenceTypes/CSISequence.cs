using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.SequenceTypes
{
    public abstract class CSISequence : Sequence
    {
        public override SequenceType SequenceType => SequenceType.CSI;

        protected CSISequence(ILogger logger) : base(logger)
        {
        }
    }
}