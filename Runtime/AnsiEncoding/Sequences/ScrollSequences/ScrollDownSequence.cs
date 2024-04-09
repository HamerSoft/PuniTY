namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public class ScrollDownSequence : ScrollSequence
    {
        public override char Command => 'T';
        protected override Direction Direction => Direction.Down;
    }
}