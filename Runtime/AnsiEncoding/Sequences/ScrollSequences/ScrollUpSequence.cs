namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public class ScrollUpSequence : ScrollSequence
    {
        public override char Command => 'S';
        protected override Direction Direction => Direction.Up;
    }
}